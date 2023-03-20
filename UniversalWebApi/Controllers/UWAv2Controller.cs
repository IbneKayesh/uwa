using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using UniversalWebApi.Models;
using UniversalWebApi.ModelsV2;

namespace UniversalWebApi.Controllers
{
    public class UWAv2Controller : ApiController
    {
        [HttpPost]
        [Route("api/v2/Execute")]
        [ResponseType(typeof(EQResultSet_v2))]
        public EQResultSet_v2 v2_Execute(UWA_BODY_M uwa_body)
        {
            if (ModelState.IsValid && uwa_body != null)
            {
                //header
                UWA_HEADER header = Services.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);

                bool IsValidAuth = true;
                UWA_BRANCH Branch_Auth = Services.ApiDb.getBranch(header);
                List<UWA_CONNECTION> ConnectList = Services.ApiDb.getActiveConnection(header.BRANCH_TOKEN, header.PAYLOAD_TOKEN);

                if (Branch_Auth.BRANCH_STATUS == 1)
                {
                    if (ConnectList.Count > 0)
                    {
                        if (header.BRANCH_TOKEN != ConnectList.First().BRANCH_TOKEN)
                        {
                            IsValidAuth = false;
                        }
                    }
                    else
                    {
                        IsValidAuth = false;
                    }
                }
                else if (Branch_Auth.BRANCH_STATUS == 2)
                {
                    if (ConnectList.Count > 0)
                    {
                        IsValidAuth = ConnectList.Any(x => x.BRANCH_TOKEN == header.BRANCH_TOKEN);
                    }
                    else
                    {
                        IsValidAuth = false;
                    }
                }

                if (!IsValidAuth)
                {
                    return new EQResultSet_v2() { DataResult = { SUCCESS = false, TABLES = 0, MESSAGES = "Login expired" } };
                }

                UWA_PAYLOAD payload = Services.ApiDb.getDbConnectionString(header.PAYLOAD_TOKEN);

                //body
                List<SQL_PLIST_v2> _sqlParam = new List<SQL_PLIST_v2>();
                foreach (UWA_BODY_D item in uwa_body.UWA_BODY_D)
                {
                    SQL_PLIST_v2 obj = new SQL_PLIST_v2();
                    obj.TABLE = item.RESULT_TABLE;
                    obj.SQL =Services.ApiDb.GetSqlFile(item.RESOURCE, payload.PAYLOAD_TABLE);

                    var inParams = new List<SqlParameter>();
                    foreach (PARAM param in item.PARAM_LIST)
                    {
                        inParams.Add(new SqlParameter(param.TEXT, param.VALUE));
                    }
                    obj.iPARAMS = inParams.ToArray();

                    _sqlParam.Add(obj);
                }


                if (uwa_body.METHOD == AppKeys_v1.RETURN_TYPE_GET)
                {
                    return Execute_v2.ExecuteQuery(payload.DB_CONNECTION, _sqlParam);
                }
                //else if (uwa_body.METHOD == AppKeys_v1.RETURN_TYPE_POST)
                //{
                //    SQL_PLIST_v1 obj = new SQL_PLIST_v1();
                //    obj.SQL = sql;
                //    obj.iPARAMS = inParams.ToArray();
                //    EQResult_v1 dbObj = Execute_v1.ExecuteSF(payload.DB_CONNECTION, new List<SQL_PLIST_v1> { obj });
                //    return new EQResultTable_v1() { Result = dbObj };
                //}
                else
                {
                    return new EQResultSet_v2();
                }
            }

            return new EQResultSet_v2();
        }
    }
}
