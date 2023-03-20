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
        [HttpPost]
        [HttpOptions]
        [Route("api/v1/Execute")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Execute(UWA_BODY uwa_body)
        {
            if (ModelState.IsValid && uwa_body != null)
            {
                //header
                UWA_HEADER header =Services.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);

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
                    return new EQResultTable_v1() { Result = { SUCCESS = false, ROWS = 0, MESSAGES = "Login expired" } };
                }

                UWA_PAYLOAD payload = Services.ApiDb.getDbConnectionString(header.PAYLOAD_TOKEN);

                //body
                string sql = Services.ApiDb.GetSqlFile(uwa_body.RESOURCE, payload.PAYLOAD_TABLE);
                var inParams = new List<SqlParameter>();
                foreach (PARAM param in uwa_body.PARAM_LIST)
                {
                    inParams.Add(new SqlParameter(param.TEXT, param.VALUE));
                }

                if (uwa_body.METHOD == AppKeys_v1.RETURN_TYPE_GET)
                {
                    return Execute_v1.ExecuteQuery(payload.DB_CONNECTION, sql, inParams.ToArray());
                }
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
                    return new EQResultTable_v1();
                }
            }

            return new EQResultTable_v1();
        }

     

   

      



        public static void WriteConnection(UWA_CONNECTION obj, bool isSingle = true)
        {
            var file = HttpContext.Current.Server.MapPath(AppKeys_v1.DB_PATH);
            SQL_PLIST_v1 objSql = new SQL_PLIST_v1();
            objSql.SQL = "INSERT INTO UWA_CONNECTION(PAYLOAD_ID,BRANCH_ID,BRANCH_TOKEN,CREATE_TIME,END_TIME)VALUES(@PAYLOAD_ID,@BRANCH_ID,@BRANCH_TOKEN,@CREATE_TIME,@END_TIME)";
            var inParams = new List<SQLiteParameter>();
            inParams.Add(new SQLiteParameter("@PAYLOAD_ID", obj.PAYLOAD_ID));
            inParams.Add(new SQLiteParameter("@BRANCH_ID", obj.BRANCH_ID));
            inParams.Add(new SQLiteParameter("@BRANCH_TOKEN", obj.BRANCH_TOKEN));
            inParams.Add(new SQLiteParameter("@CREATE_TIME", obj.CREATE_TIME));
            inParams.Add(new SQLiteParameter("@END_TIME", obj.END_TIME));
            objSql.iPARAMS = inParams.ToArray();
            Execute_v1SqLite.ExecuteSF(file, new List<SQL_PLIST_v1> { objSql });
        }
     

        //Login authorization

        [HttpPost]
        [HttpOptions]
        [Route("api/v1/Login")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Login()
        {
            //header
            UWA_HEADER header = Services.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);

            string branchName = header.BRANCH_TOKEN;
            string branchToken = Guid.NewGuid().ToString();
            UWA_BRANCH Branch_Auth = Services.ApiDb.getBranch(header);

            UWA_CONNECTION con = new UWA_CONNECTION();
            con.PAYLOAD_ID = header.PAYLOAD_TOKEN;
            con.BRANCH_ID = branchName;
            con.BRANCH_TOKEN = branchToken;
            con.CREATE_TIME = DateTime.Now;
            con.END_TIME = DateTime.Now.AddDays(1);

            if (Branch_Auth.BRANCH_STATUS == 1)
            {
                WriteConnection(con);
            }
            else if (Branch_Auth.BRANCH_STATUS == 2)
            {
                WriteConnection(con, false);
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
        [HttpOptions]
        [Route("api/v1/Parameters")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Parameters(UWA_BODY uwa_body)
        {
            if (ModelState.IsValid && uwa_body != null)
            {
                //header
                UWA_HEADER header = Services.RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);

                UWA_PAYLOAD payload = Services.ApiDb.getDbConnectionString(header.PAYLOAD_TOKEN);

                //body
                string sql = Services.ApiDb.GetSqlFile(uwa_body.RESOURCE, payload.PAYLOAD_TABLE);


                if (uwa_body.METHOD == AppKeys_v1.RETURN_TYPE_GET)
                {
                    EQResultTable_v1 returnTable = new EQResultTable_v1();
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
                    return new EQResultTable_v1();
                }
            }

            return new EQResultTable_v1();
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
