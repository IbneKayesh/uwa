using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using UniversalWebApi.Controllers;
using UniversalWebApi.Models;

namespace UniversalWebApi.Areas.Mssql.Controllers
{
    public class Mssql_v1Controller : ApiController
    {
        [CORSDomain]
        [HttpPost]
        //[HttpOptions]
        [Route("api/Mssql/v1/Execute")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Execute(UWA_BODY uwa_body)
        {
            EQResultTable_v1 retObj = new EQResultTable_v1();

            if (ModelState.IsValid && uwa_body != null)
            {
                //find header information
                UWA_HEADER header = Services.Api.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);
                //validate header
                if (!header.IS_VALID)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Header token was not provided" };
                    return retObj;
                }
                //find branch/user information
                UWA_BRANCH Branch_Auth = Services.Db.ApiStore.getBranch(header);
                //validate branch information
                if (Branch_Auth.BRANCH_DESC == AppKeys_v1.INVALID_BRANCH_TOKEN)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Invalid payload or branch token" };
                    return retObj;
                }

                bool IsValidAuth = true;
                // auth single or multiple, 1 or 2
                if (Branch_Auth.BRANCH_STATUS > 0)
                {
                    var ConnectList = Services.Db.ApiStore.getActiveConnection(header.BRANCH_TOKEN, header.PAYLOAD_TOKEN);
                    //single auth
                    if (Branch_Auth.BRANCH_STATUS == 1 && ConnectList.Count == 1)
                    {
                        IsValidAuth = (header.BRANCH_TOKEN == ConnectList.First().BRANCH_TOKEN);
                    }
                    //multiple auth
                    else if (Branch_Auth.BRANCH_STATUS > 1 && ConnectList.Count > 0)
                    {
                        IsValidAuth = (header.BRANCH_TOKEN == ConnectList.First().BRANCH_TOKEN);
                    }
                    else
                    {
                        IsValidAuth = false;
                    }
                }
                //validate auth
                if (!IsValidAuth)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Session is expired, Please Login" };
                    return retObj;
                }
                //find payload or database connections
                UWA_PAYLOAD payload = Services.Db.ApiStore.getDbConnectionString(header.PAYLOAD_TOKEN);
                //validate branch information
                if (payload.DB_DESC == AppKeys_v1.INVALID_PAYLOAD_TOKEN)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = payload.DB_DESC };
                    return retObj;
                }

                //find SQL query
                string sql = Services.Db.ApiStore.GetSqlFile(uwa_body.RESOURCE, payload.PAYLOAD_TABLE);
                //validate sql query
                if (sql == AppKeys_v1.INVALID_RESOURCE_TOKEN)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = sql };
                    return retObj;
                }
                //bind query and parameters
                var inParams = new List<SqlParameter>();
                foreach (PARAM param in uwa_body.PARAM_LIST)
                {
                    inParams.Add(new SqlParameter(param.TEXT, param.VALUE));
                }
                //GET
                if (uwa_body.METHOD == AppKeys_v1.RETURN_TYPE_GET)
                {
                    return Services.Mssql.Execute_v1.ExecuteQuery(payload.DB_CONNECTION, sql, inParams.ToArray());
                }
                //POST
                else if (uwa_body.METHOD == AppKeys_v1.RETURN_TYPE_POST)
                {
                    SQL_PLIST_v1 obj = new SQL_PLIST_v1();
                    obj.SQL = sql;
                    obj.iPARAMS = inParams.ToArray();
                    EQResult_v1 dbObj = Services.Mssql.Execute_v1.ExecuteSF(payload.DB_CONNECTION, new List<SQL_PLIST_v1> { obj });
                    return new EQResultTable_v1() { Result = dbObj };
                }
                else
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Invalid Method" };
                    return retObj;
                }
            }
            retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Invalid body parameters" };
            return retObj;
        }
    }
}
