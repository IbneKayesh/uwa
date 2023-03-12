using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using UniversalWebApi.Models;

namespace UniversalWebApi.Controllers
{
    public class UWAController : ApiController
    {
        [HttpPost]
        [HttpOptions]
        [Route("api/v1/ApplyCommit")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 SelectDistributor(SF_STYLER sf_styler)
        {
            if (ModelState.IsValid)
            {
                string baseLink = sf_styler.SF_NAME.Split('-')[0];
                string sqlLink = sf_styler.SF_NAME.Split('-')[1];
                // get DB
                string dbStr = ValidateRequestToken(baseLink);
                SF_BASE_LINK sf_base_link = JsonConvert.DeserializeObject<SF_BASE_LINK>(dbStr);
                // get SQL
                string sqlStr = GetSqlFile(sf_base_link.FILE_LOCATION, sqlLink);
                SF_SQL_LINK sf_sql_link = JsonConvert.DeserializeObject<SF_SQL_LINK>(sqlStr);
                //Execute Stored Procedure In Target Database
                var inParams = new List<SqlParameter>();
                foreach (PARAM param in sf_styler.PARAM_LIST)
                {
                    inParams.Add(new SqlParameter(param.TEXT, param.VALUE));
                }

                //Return type Yes
                if (sf_styler.SF_RETN == AppKeys_v1.RETURN_TYPE_GET)
                {
                    return Execute_v1.ExecuteQuery(sf_base_link.DB_CONNECTION, sf_sql_link.SQL, inParams.ToArray());
                }
                else if (sf_styler.SF_RETN == AppKeys_v1.RETURN_TYPE_POST)
                {
                    SQL_PLIST_v1 obj = new SQL_PLIST_v1();
                    obj.SQL = sf_sql_link.SQL;
                    obj.iPARAMS = inParams.ToArray();
                    EQResult_v1 dbObj = Execute_v1.ExecuteSF(sf_base_link.DB_CONNECTION, new List<SQL_PLIST_v1> { obj });
                    return new EQResultTable_v1() { Result = dbObj };
                }
                else
                {
                    return new EQResultTable_v1();
                }
            }
            return new EQResultTable_v1();
        }



        private string ValidateRequestToken(string tokenNo)
        {
            var file = HttpContext.Current.Server.MapPath(AppKeys_v1.DB_PATH + tokenNo + ".txt");

            if (System.IO.File.Exists(file))
            {
                return System.IO.File.ReadAllText(file);
            }
            else
            {
                return AppKeys_v1.INVALID_BASE_TOKEN;
            }
        }

        private string GetSqlFile(string sqlFilePath, string tokenNo)
        {
            var file = HttpContext.Current.Server.MapPath(sqlFilePath + tokenNo + ".txt");

            if (System.IO.File.Exists(file))
            {
                return System.IO.File.ReadAllText(file);
            }
            else
            {
                return AppKeys_v1.INVALID_TARGET_TOKEN;
            }
        }

    }
}
