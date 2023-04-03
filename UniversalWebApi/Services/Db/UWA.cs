using System.Collections.Generic;
using System.Data.SQLite;
using System.Web;
using UniversalWebApi.Areas.Manager.Models;
using UniversalWebApi.Models;

namespace UniversalWebApi.Services.Db
{
    public class UWA
    {
        static string sql = "";
        public static EQResultTable_v1 GetAllBranch()
        {
            var file = AppKeys_v1.DB_PATH;
            sql = "SELECT * FROM UWA_BRANCH";
            return Execute_v1SqLite.ExecuteQuery(file, sql);
        }
        public static EQResultTable_v1 GetByIdBranch(string id, string db)
        {
            var file = AppKeys_v1.DB_PATH;
            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@BRANCH_ID", id));
            parameters.Add(new SQLiteParameter("@PAYLOAD_ID", db));
            sql = "SELECT * FROM UWA_BRANCH WHERE BRANCH_ID=@BRANCH_ID AND PAYLOAD_ID=@PAYLOAD_ID";
            return Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
        }
        public static EQResult_v1 UpdateBranch(UWA_BRANCH obj, bool insert = true)
        {
            var file = AppKeys_v1.DB_PATH;

            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            if (insert)
            {
                parameters.Add(new SQLiteParameter("@PAYLOAD_ID", obj.PAYLOAD_ID));
                parameters.Add(new SQLiteParameter("@BRANCH_ID", obj.BRANCH_ID));
                parameters.Add(new SQLiteParameter("@BRANCH_DESC", obj.BRANCH_DESC));
                parameters.Add(new SQLiteParameter("@BRANCH_STATUS", obj.BRANCH_STATUS));
                sql = $@"INSERT INTO  UWA_BRANCH (PAYLOAD_ID,BRANCH_ID,BRANCH_DESC,BRANCH_STATUS)VALUES(@PAYLOAD_ID,@BRANCH_ID,@BRANCH_DESC,@BRANCH_STATUS)";
                sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            }
            else
            {
                parameters.Add(new SQLiteParameter("@PAYLOAD_ID", obj.PAYLOAD_ID));
                parameters.Add(new SQLiteParameter("@BRANCH_DESC", obj.BRANCH_DESC));
                parameters.Add(new SQLiteParameter("@BRANCH_STATUS", obj.BRANCH_STATUS));
                parameters.Add(new SQLiteParameter("@BRANCH_ID", obj.BRANCH_ID));
                sql = $@"UPDATE UWA_BRANCH SET PAYLOAD_ID=@PAYLOAD_ID,BRANCH_DESC=@BRANCH_DESC,BRANCH_STATUS=@BRANCH_STATUS WHERE BRANCH_ID=@BRANCH_ID";
                sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            }
            return Execute_v1SqLite.ExecuteSF(file, sqlList);
        }
        public static EQResult_v1 DeleteBranch(string id, string db)
        {
            var file = AppKeys_v1.DB_PATH;

            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@BRANCH_ID", id));
            parameters.Add(new SQLiteParameter("@PAYLOAD_ID", db));
            sql = $@"DELETE FROM  UWA_BRANCH WHERE BRANCH_ID=@BRANCH_ID AND PAYLOAD_ID=@PAYLOAD_ID";
            sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            return Execute_v1SqLite.ExecuteSF(file, sqlList);
        }






        public static EQResultTable_v1 GetAllPayload()
        {
            var file = AppKeys_v1.DB_PATH;
            sql = "SELECT * FROM UWA_PAYLOAD";
            return Execute_v1SqLite.ExecuteQuery(file, sql);
        }

        public static EQResultTable_v1 GetByIdPayload(string id)
        {
            var file = AppKeys_v1.DB_PATH;
            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@PAYLOAD_ID", id));
            sql = "SELECT * FROM UWA_PAYLOAD WHERE PAYLOAD_ID=@PAYLOAD_ID";
            return Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
        }
        public static EQResult_v1 UpdatePayload(UWA_PAYLOAD obj, bool insert = true)
        {
            var file = AppKeys_v1.DB_PATH;

            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            if (insert)
            {
                parameters.Add(new SQLiteParameter("@PAYLOAD_ID", obj.PAYLOAD_ID));
                parameters.Add(new SQLiteParameter("@DB_TYPE", obj.DB_TYPE));
                parameters.Add(new SQLiteParameter("@DB_CONNECTION", obj.DB_CONNECTION));
                parameters.Add(new SQLiteParameter("@DB_DESC", obj.DB_DESC));
                parameters.Add(new SQLiteParameter("@PAYLOAD_TABLE", obj.PAYLOAD_TABLE));
                sql = $@"INSERT INTO UWA_PAYLOAD(PAYLOAD_ID,DB_TYPE,DB_CONNECTION,DB_DESC,PAYLOAD_TABLE)VALUES(@PAYLOAD_ID,@DB_TYPE,@DB_CONNECTION,@DB_DESC,@PAYLOAD_TABLE)";
                sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            }
            else
            {
                parameters.Add(new SQLiteParameter("@DB_CONNECTION", obj.DB_CONNECTION));
                parameters.Add(new SQLiteParameter("@DB_TYPE", obj.DB_TYPE));
                parameters.Add(new SQLiteParameter("@DB_DESC", obj.DB_DESC));
                parameters.Add(new SQLiteParameter("@PAYLOAD_TABLE", obj.PAYLOAD_TABLE));
                parameters.Add(new SQLiteParameter("@PAYLOAD_ID", obj.PAYLOAD_ID));
                sql = $@"UPDATE UWA_PAYLOAD SET DB_CONNECTION=@DB_CONNECTION,DB_TYPE=@DB_TYPE,DB_DESC=@DB_DESC,PAYLOAD_TABLE=@PAYLOAD_TABLE WHERE PAYLOAD_ID=@PAYLOAD_ID";
                sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            }
            return Execute_v1SqLite.ExecuteSF(file, sqlList);
        }
        public static EQResult_v1 DeletePayload(string id)
        {
            var file = AppKeys_v1.DB_PATH;

            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@PAYLOAD_ID", id));
            sql = $@"DELETE FROM UWA_PAYLOAD WHERE PAYLOAD_ID=@PAYLOAD_ID";
            sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            return Execute_v1SqLite.ExecuteSF(file, sqlList);
        }





        public static EQResultTable_v1 GetDbTable(string name)
        {
            var file = AppKeys_v1.DB_PATH;
            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();

            //show all table
            sql = $@"SELECT '{name}' PAYLOAD_ID,name TABLE_NAME FROM sqlite_master WHERE type = 'table' and name not in ('UWA_PAYLOAD','UWA_BRANCH','UWA_CONNECTION') AND name like '{name}_%'";
            return Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
        }

        public static EQResult_v1 CreateModuleTable(string payloadId, string moduleName)
        {
            string table = $"{payloadId}_{moduleName}";
            sql = $"CREATE TABLE {table} ( `ID` TEXT, `SQL` TEXT, PRIMARY KEY(`ID`) )";
            var file = AppKeys_v1.DB_PATH;

            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            var ret = Execute_v1SqLite.ExecuteSF(file, sqlList);
            CreateSql(new TableSqlModel { TABLE_NAME = table, ID = "item-select-by-id", SQL = "select * from master_item where item_id=@item_id" });

            return ret;
        }


        public static EQResultTable_v1 GetDbTableSql(string name)
        {
            var file = AppKeys_v1.DB_PATH;
            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            //show table data
            sql = $"SELECT '{name}' TABLE_NAME,ID,SQL FROM {name}";
            return Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
        }

        public static EQResult_v1 CreateSql(TableSqlModel _obj)
        {
            sql = $"INSERT INTO {_obj.TABLE_NAME} (ID,SQL)VALUES('{_obj.ID}','{_obj.SQL}')";
            var file = AppKeys_v1.DB_PATH;

            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            return Execute_v1SqLite.ExecuteSF(file, sqlList);
        }
        public static EQResult_v1 DeleteTableSql(string id, string table)
        {
            var file = AppKeys_v1.DB_PATH;

            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@ID", id));
            sql = $@"DELETE FROM {table} WHERE ID=@ID";
            sqlList.Add(new SQL_PLIST_v1 { SQL = sql, iPARAMS = parameters.ToArray() });
            return Execute_v1SqLite.ExecuteSF(file, sqlList);
        }
        public static EQResultTable_v1 GetDbTableSqlById(string table, string id)
        {
            var file = AppKeys_v1.DB_PATH;
            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@ID", id));
            //show table data
            sql = $"SELECT SQL FROM {table} WHERE ID=@ID";
            return Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
        }

        public static string GetSeparator(string tablePrefix)
        {
            var file = AppKeys_v1.DB_PATH;
            List<SQL_PLIST_v1> sqlList = new List<SQL_PLIST_v1>();

            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            parameters.Add(new SQLiteParameter("@PAYLOAD_TABLE", tablePrefix));
            //show table data
            sql = $"SELECT DB_TYPE FROM UWA_PAYLOAD WHERE PAYLOAD_TABLE=@PAYLOAD_TABLE";
            EQResultTable_v1 dbObj = Execute_v1SqLite.ExecuteQuery(file, sql, parameters.ToArray());
            if (dbObj.Result.SUCCESS && dbObj.Result.ROWS > 0)
            {
                string dbtype = dbObj.Table.Rows[0]["DB_TYPE"].ToString();

                if (dbtype == DatabaseType.MSSQL.ToString())
                {
                    //mssql
                    return "@";
                }
                else if (dbtype == DatabaseType.ORACLE.ToString())
                {
                    //oracle
                    return ":";
                }
                else
                {
                    //mssql
                    return "@";
                }
            }
            else
            {
                //mssql
                return "@";
            }
        }
    }
}