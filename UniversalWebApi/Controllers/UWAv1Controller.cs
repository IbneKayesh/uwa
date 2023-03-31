using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using UniversalWebApi.Models;

namespace UniversalWebApi.Controllers
{
    public class UWAv1Controller : ApiController
    {
        [Cors]
        [HttpPost]
        //[HttpOptions]
        [Route("api/v1/Execute")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Execute(UWA_BODY uwa_body)
        {
            EQResultTable_v1 retObj = new EQResultTable_v1();

            if (ModelState.IsValid && uwa_body != null)
            {
                //find header information
                UWA_HEADER header = Services.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);
                //validate header
                if (!header.IS_VALID)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Header token was not provided" };
                    return retObj;
                }
                //find branch/user information
                UWA_BRANCH Branch_Auth = Services.ApiDb.getBranch(header);
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
                    var ConnectList = Services.ApiDb.getActiveConnection(header.BRANCH_TOKEN, header.PAYLOAD_TOKEN);
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
                UWA_PAYLOAD payload = Services.ApiDb.getDbConnectionString(header.PAYLOAD_TOKEN);
                //validate branch information
                if (payload.DB_DESC == AppKeys_v1.INVALID_PAYLOAD_TOKEN)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = payload.DB_DESC };
                    return retObj;
                }

                //find SQL query
                string sql = Services.ApiDb.GetSqlFile(uwa_body.RESOURCE, payload.PAYLOAD_TABLE);
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
                    return Execute_v1.ExecuteQuery(payload.DB_CONNECTION, sql, inParams.ToArray());
                }
                //POST
                else if (uwa_body.METHOD == AppKeys_v1.RETURN_TYPE_POST)
                {
                    SQL_PLIST_v1 obj = new SQL_PLIST_v1();
                    obj.SQL = sql;
                    obj.iPARAMS = inParams.ToArray();
                    EQResult_v1 dbObj = Execute_v1.ExecuteSF(payload.DB_CONNECTION, new List<SQL_PLIST_v1> { obj });
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



        //Login authorization

        [HttpPost]
        //[HttpOptions]
        [Route("api/v1/Login")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Login()
        {
            EQResultTable_v1 retObj = new EQResultTable_v1();
            //find header information
            UWA_HEADER header = Services.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);
            //validate header
            if (!header.IS_VALID)
            {
                retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Header token was not provided" };
                return retObj;
            }


            string branchName = header.BRANCH_TOKEN;
            string branchToken = Guid.NewGuid().ToString();

            //find branch/user information
            UWA_BRANCH Branch_Auth = Services.ApiDb.getBranch(header);
            //validate branch information
            if (Branch_Auth.BRANCH_DESC == AppKeys_v1.INVALID_BRANCH_TOKEN)
            {
                retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Invalid payload or branch token" };
                return retObj;
            }

            //write branch session
            UWA_CONNECTION con = new UWA_CONNECTION();
            con.PAYLOAD_ID = header.PAYLOAD_TOKEN;
            con.BRANCH_ID = branchName;
            con.BRANCH_TOKEN = branchToken;
            con.CREATE_TIME = DateTime.Now;
            con.END_TIME = DateTime.Now.AddDays(1);

            if (Branch_Auth.BRANCH_STATUS == 1)
            {
                Services.ApiDb.WriteConnection(con);
            }
            else if (Branch_Auth.BRANCH_STATUS == 2)
            {
                Services.ApiDb.WriteConnection(con, false);
            }



            EQResultTable_v1 returnTable = new EQResultTable_v1();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("BRANCH");
            dataTable.Columns.Add("TOKEN");
            DataRow row = dataTable.NewRow();
            row["BRANCH"] = branchName;
            row["TOKEN"] = branchToken;
            dataTable.Rows.Add(row);

            returnTable.Table = dataTable;
            return returnTable;
        }











        //Help Endpoint

        [HttpPost]
        //[HttpOptions]
        [Route("api/v1/Parameters")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Parameters(UWA_BODY uwa_body)
        {
            EQResultTable_v1 retObj = new EQResultTable_v1();

            if (ModelState.IsValid && uwa_body != null)
            {
                //find header information
                UWA_HEADER header = Services.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);
                //validate header
                if (!header.IS_VALID)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = "Header token was not provided" };
                    return retObj;
                }
                //find payload or database connections
                UWA_PAYLOAD payload = Services.ApiDb.getDbConnectionString(header.PAYLOAD_TOKEN);
                //validate branch information
                if (payload.DB_DESC == AppKeys_v1.INVALID_PAYLOAD_TOKEN)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = payload.DB_DESC };
                    return retObj;
                }

                //find SQL query
                string sql = Services.ApiDb.GetSqlFile(uwa_body.RESOURCE, payload.PAYLOAD_TABLE);
                //validate sql query
                if (sql == AppKeys_v1.INVALID_RESOURCE_TOKEN)
                {
                    retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = sql };
                    return retObj;
                }

                //GET
                if (uwa_body.METHOD == AppKeys_v1.RETURN_TYPE_GET)
                {
                    EQResultTable_v1 returnTable = new EQResultTable_v1();
                    int part = uwa_body.RESOURCE.Split('.').Count();
                    if (part != 3)
                    {
                        //table . sql id . parameter
                        retObj.Result = new EQResult_v1() { SUCCESS = false, MESSAGES = AppKeys_v1.INVALID_RESOURCE_TOKEN };
                        return retObj;
                    }
                    List<string> paramList = FindSqlParameters(sql, uwa_body.RESOURCE.Split('.')[2]);
                    int i = 0;
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("TEXT");
                    dataTable.Columns.Add("VALUE");
                    foreach (string item in paramList)
                    {
                        DataRow row = dataTable.NewRow();
                        row["TEXT"] = item;
                        row["VALUE"] = "Value" + i;
                        dataTable.Rows.Add(row);
                        i++;
                    }
                    if (paramList.Count < 1)
                    {
                        DataRow row = dataTable.NewRow();
                        row["TEXT"] = "NULL";
                        row["VALUE"] = "NULL";
                        dataTable.Rows.Add(row);
                    }
                    returnTable.Table = dataTable;

                    return returnTable;
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
        public static List<string> FindSqlParameters(string sql, string separator)
        {
            List<string> parameters = new List<string>();
            Regex regex = new Regex($@"{separator}\w+");
            MatchCollection matches = regex.Matches(sql);
            foreach (Match match in matches)
            {
                parameters.Add(match.Value);
            }
            return parameters;
        }



    }
}
