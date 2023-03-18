using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        [Route("api/v1/Execute")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Execute(UWA_BODY uwa_body)
        {
            if (ModelState.IsValid && uwa_body != null)
            {
                //header
                UWA_HEADER header = RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);

                bool IsValidAuth = true;
                UWA_BRANCH Branch_Auth = getBranch(header);
                List<UWA_CONNECTION> ConnectList = getActiveConnection(header.BRANCH_TOKEN, header.PAYLOAD_TOKEN);
               
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

                UWA_PAYLOAD payload = getDbConnectionString(header.PAYLOAD_TOKEN);

                //body
                string sql = GetSqlFile(uwa_body.RESOURCE, payload.PAYLOAD_TABLE);
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

        private UWA_PAYLOAD getDbConnectionString(string payloadId)
        {
            var file = HttpContext.Current.Server.MapPath(AppKeys_v1.DB_PATH);
            string sql = "SELECT * FROM UWA_PAYLOAD WHERE PAYLOAD_ID=@PAYLOAD_ID";
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@PAYLOAD_ID", payloadId));
            EQResultTable_v1 dbObj = Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS == 1)
            {
                UWA_PAYLOAD obj = new UWA_PAYLOAD();
                obj.PAYLOAD_ID = dbObj.Table.Rows[0]["PAYLOAD_ID"].ToString();
                obj.DB_CONNECTION = dbObj.Table.Rows[0]["DB_CONNECTION"].ToString();
                obj.DB_DESC = dbObj.Table.Rows[0]["DB_DESC"].ToString();
                obj.PAYLOAD_TABLE = dbObj.Table.Rows[0]["PAYLOAD_TABLE"].ToString();
                return obj;
            }
            else
            {
                return new UWA_PAYLOAD() { DB_DESC = AppKeys_v1.INVALID_PAYLOAD_TOKEN };
            }
        }

        private string GetSqlFile(string resource, string dbPath)
        {
            var file = HttpContext.Current.Server.MapPath(AppKeys_v1.DB_PATH);

            string sql_file_name = resource.Split('.')[0];
            string sql_file_id = resource.Split('.')[1];

            string sql = $"SELECT * FROM {dbPath}_{sql_file_name} WHERE ID=@ID";
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@ID", sql_file_id));
            EQResultTable_v1 dbObj = Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS == 1)
            {
                return dbObj.Table.Rows[0]["SQL"].ToString();
            }
            else
            {
                return AppKeys_v1.INVALID_RESOURCE_TOKEN;
            }
        }

        private UWA_BRANCH getBranch(UWA_HEADER header)
        {
            var file = HttpContext.Current.Server.MapPath(AppKeys_v1.DB_PATH);
            string sql = "SELECT * FROM UWA_BRANCH B WHERE B.BRANCH_ID=@BRANCH_ID AND PAYLOAD_ID=@PAYLOAD_ID";
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@BRANCH_ID", header.BRANCH_TOKEN));
            parameters.Add(new SQLiteParameter("@PAYLOAD_ID", header.PAYLOAD_TOKEN));
            EQResultTable_v1 dbObj = Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS == 1)
            {
                UWA_BRANCH obj = new UWA_BRANCH();
                obj.PAYLOAD_ID = dbObj.Table.Rows[0]["PAYLOAD_ID"].ToString();
                obj.BRANCH_ID = dbObj.Table.Rows[0]["BRANCH_ID"].ToString();
                obj.BRANCH_DESC = dbObj.Table.Rows[0]["BRANCH_DESC"].ToString();
                obj.BRANCH_STATUS = Convert.ToInt32(dbObj.Table.Rows[0]["BRANCH_STATUS"]);
                return obj;
            }
            else
            {
                return new UWA_BRANCH() { BRANCH_DESC = AppKeys_v1.INVALID_BRANCH_TOKEN };
            }
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
        public static List<UWA_CONNECTION> getActiveConnection(string branchId, string payloadId)
        {
            var file = HttpContext.Current.Server.MapPath(AppKeys_v1.DB_PATH);

            List<UWA_CONNECTION> objects = new List<UWA_CONNECTION>();

            string sql = "SELECT * FROM UWA_CONNECTION WHERE PAYLOAD_ID=@PAYLOAD_ID AND BRANCH_ID=@BRANCH_ID";
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@PAYLOAD_ID", payloadId));
            parameters.Add(new SQLiteParameter("@BRANCH_ID", branchId));
            EQResultTable_v1 dbObj = Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
            {
                foreach (DataRow row in dbObj.Table.Rows)
                {
                    UWA_CONNECTION obj = new UWA_CONNECTION
                    {
                        BRANCH_ID = row["BRANCH_ID"].ToString(),
                        BRANCH_TOKEN = row["BRANCH_TOKEN"].ToString(),
                        CREATE_TIME = Convert.ToDateTime(row["CREATE_TIME"]),
                        END_TIME = Convert.ToDateTime(row["END_TIME"])
                    };
                    objects.Add(obj);
                }
            }
            return objects;
        }








        //Login authorization

        [HttpPost]
        [HttpOptions]
        [Route("api/v1/Login")]
        [ResponseType(typeof(EQResultTable_v1))]
        public EQResultTable_v1 v1_Login()
        {
            //header
            UWA_HEADER header = RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);

            string branchName = header.BRANCH_TOKEN;
            string branchToken = Guid.NewGuid().ToString();
            UWA_BRANCH Branch_Auth = getBranch(header);

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
                UWA_HEADER header = RequestHeaders.FromHttpRequestHeaders(this.Request.Headers);

                UWA_PAYLOAD payload = getDbConnectionString(header.PAYLOAD_TOKEN);

                //body
                string sql = GetSqlFile(uwa_body.RESOURCE, payload.PAYLOAD_TABLE);


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
