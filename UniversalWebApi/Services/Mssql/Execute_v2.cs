using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UniversalWebApi.Models;
using UniversalWebApi.ModelsV2;

namespace UniversalWebApi.Services.Mssql
{
    public class Execute_v2
    {
        public static EQResultSet_v2 ExecuteQuery(string _conStr, List<SQL_PLIST_v2> _sf)
        {
            List<EQResultSet_v02> DataTableList = new List<EQResultSet_v02>();
            EQResultSet_v02 DataTable = new EQResultSet_v02();


            EQResult_v02 dResult = new EQResult_v02();
            EQResult_v1 result = new EQResult_v1();

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection con = new SqlConnection(_conStr);
            try
            {
                int tabId = 1;
                foreach (SQL_PLIST_v2 item in _sf)
                {
                    //new dataSet
                    ds = new DataSet();
                    //
                    SqlCommand cmd = new SqlCommand(item.SQL, con);
                    if (item.iPARAMS != null)
                    {
                        cmd.Parameters.AddRange(item.iPARAMS);
                    }
                    cmd.CommandTimeout = int.MaxValue;
                    cmd.CommandType = CommandType.Text;
                    da.SelectCommand = cmd;

                    string tabName = string.IsNullOrWhiteSpace(item.TABLE) ? "Table" + tabId : item.TABLE;
                    ds.Tables.Add(tabName);

                    da.Fill(ds.Tables[tabName]);

                    tabId++;

                    result = new EQResult_v1();
                    result.ROWS = ds.Tables.Count == 0 ? 0 : ds.Tables[tabName].Rows.Count;
                    result.MESSAGES = ds.Tables.Count == 0 ? AppKeys_v1.NO_ROWS_FOUND : AppKeys_v1.SUCCESS_MESSAGES;

                    DataTable = new EQResultSet_v02();
                    DataTable.Name = tabName;
                    DataTable.Table = ds.Tables[tabName];
                    DataTable.Result = result;
                    DataTableList.Add(DataTable);
                }
            }
            catch (SqlException ex)
            {
                dResult.SUCCESS = false;
                dResult.MESSAGES = ex.Message;
                dResult.TABLES = 0;
                return new EQResultSet_v2() { DataSet = new List<EQResultSet_v02>(), DataResult = dResult };
            }
            finally
            {
                con.Close();
            }
            dResult.TABLES = DataTableList.Count;
            return new EQResultSet_v2() { DataSet = DataTableList, DataResult = dResult };
        }
    }
}