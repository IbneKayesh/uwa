using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web;
using UniversalWebApi.Models;

namespace UniversalWebApi.Services
{
    public class ApiDb
    {
        public static UWA_BRANCH getBranch(UWA_HEADER header)
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
        public static UWA_PAYLOAD getDbConnectionString(string payloadId)
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
        public static string GetSqlFile(string resource, string dbPath)
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
        public static void WriteConnection(UWA_CONNECTION obj, bool isSingle = true)
        {
            var file = HttpContext.Current.Server.MapPath(AppKeys_v1.DB_PATH);
            SQL_PLIST_v1 objSql = new SQL_PLIST_v1();
            List<SQLiteParameter> inParams = new List<SQLiteParameter>();
            if (isSingle)
            {
                objSql.SQL = "DELETE FROM UWA_CONNECTION WHERE PAYLOAD_ID=@PAYLOAD_ID AND BRANCH_ID=@BRANCH_ID";
                inParams = new List<SQLiteParameter>();
                inParams.Add(new SQLiteParameter("@PAYLOAD_ID", obj.PAYLOAD_ID));
                inParams.Add(new SQLiteParameter("@BRANCH_ID", obj.BRANCH_ID));
                objSql.iPARAMS = inParams.ToArray();
                Execute_v1SqLite.ExecuteSF(file, new List<SQL_PLIST_v1> { objSql });
            }

            objSql = new SQL_PLIST_v1();
            objSql.SQL = "INSERT INTO UWA_CONNECTION(PAYLOAD_ID,BRANCH_ID,BRANCH_TOKEN,CREATE_TIME,END_TIME)VALUES(@PAYLOAD_ID,@BRANCH_ID,@BRANCH_TOKEN,@CREATE_TIME,@END_TIME)";
            inParams = new List<SQLiteParameter>();
            inParams.Add(new SQLiteParameter("@PAYLOAD_ID", obj.PAYLOAD_ID));
            inParams.Add(new SQLiteParameter("@BRANCH_ID", obj.BRANCH_ID));
            inParams.Add(new SQLiteParameter("@BRANCH_TOKEN", obj.BRANCH_TOKEN));
            inParams.Add(new SQLiteParameter("@CREATE_TIME", obj.CREATE_TIME));
            inParams.Add(new SQLiteParameter("@END_TIME", obj.END_TIME));
            objSql.iPARAMS = inParams.ToArray();
            Execute_v1SqLite.ExecuteSF(file, new List<SQL_PLIST_v1> { objSql });
        }
    }
}