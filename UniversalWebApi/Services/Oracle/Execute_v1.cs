﻿using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using UniversalWebApi.Models;

namespace UniversalWebApi.Services.Oracle
{
    public class Execute_v1
    {
        public static EQResultTable_v1 ExecuteQuery(string _conStr, string _command, object[] _parameters = null)
        {
            EQResult_v1 result = new EQResult_v1();
            result.SUCCESS = true;
            result.MESSAGES = AppKeys_v1.SUCCESS_MESSAGES;
            DataSet ds = new DataSet();
            OracleDataAdapter da = new OracleDataAdapter();
            OracleConnection con = new OracleConnection();
            try
            {
                con = new OracleConnection(_conStr);
                OracleCommand cmd = new OracleCommand(_command, con);
                if (_parameters != null)
                {
                    cmd.Parameters.AddRange(_parameters);
                }
                cmd.CommandTimeout = int.MaxValue;
                cmd.CommandType = CommandType.Text;
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (OracleException ex)
            {
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
                return new EQResultTable_v1() { Table = new DataTable(), Result = result };
            }
            finally
            {
                con.Close();
            }
            result.ROWS = ds.Tables.Count == 0 ? 0 : ds.Tables[0].Rows.Count;
            result.MESSAGES = ds.Tables.Count == 0 ? AppKeys_v1.NO_ROWS_FOUND : AppKeys_v1.SUCCESS_MESSAGES;
            return new EQResultTable_v1() { Table = ds.Tables.Count == 0 ? new DataTable() : ds.Tables[0], Result = result };
        }

        public static EQResult_v1 ExecuteSF(string _conStr, List<SQL_PLIST_v1> _sf)
        {
            EQResult_v1 result = new EQResult_v1();
            OracleConnection con = new OracleConnection(_conStr);
            OracleCommand cmd = new OracleCommand();
            OracleTransaction trn;
            con.Open();
            cmd.Connection = con;
            cmd.CommandTimeout = int.MaxValue;
            trn = con.BeginTransaction();
            cmd.Transaction = trn;
            try
            {
                int r = 0;

                foreach (SQL_PLIST_v1 sf in _sf)
                {
                    cmd.CommandText = sf.SQL;
                    if (sf.iPARAMS != null)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(sf.iPARAMS);
                        r += cmd.ExecuteNonQuery();
                    }
                }

                trn.Commit();

                result.SUCCESS = true;
                result.MESSAGES = AppKeys_v1.SUCCESS_MESSAGES;
                result.ROWS = r;
            }
            catch (OracleException ex)
            {
                trn.Rollback();
                result.SUCCESS = false;
                result.MESSAGES = ex.Message;
                result.ROWS = 0;
            }
            finally
            {
                con.Close();
            }
            return result;
        }

    }
}