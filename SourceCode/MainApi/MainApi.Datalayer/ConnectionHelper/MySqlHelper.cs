using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace MainApi.DataLayer
{
    public static class MySqlHelper
    {

        public static int ExecuteNonQuery(MySqlConnection conn, string cmdText, Dictionary<string, object> cmdParms)
        {
            MySqlCommand cmd = conn.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public static int ExecuteNonQueryTransaction(MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            MySqlCommand cmd = conn.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);

                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public static int ExecuteNonQuery(MySqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            MySqlCommand cmd = conn.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public static IDataReader ExecuteReader(MySqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            MySqlCommand cmd = conn.CreateCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DataSet ExecuteReaderToDataSet(MySqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            MySqlCommand cmd = conn.CreateCommand();
            try
            {
                DataSet dtSet = new DataSet();

                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);

                MySqlDataAdapter adapter = new MySqlDataAdapter();

                adapter.SelectCommand = cmd;
                adapter.Fill(dtSet);

                return dtSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static object ExecuteScalar(MySqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            MySqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (var param in cmdParms)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = param.Key;
                    if (param.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                    else
                    {
                        parameter.Value = param.Value;
                    }

                    cmd.Parameters.Add(parameter);
                }

            }
        }

        public static void PrepareCommand(this MySqlCommand cmd, Dictionary<string, object> cmdParms, MySqlTransaction trans = null)
        {
            if (cmdParms != null)
            {
                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                foreach (var param in cmdParms)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = param.Key;
                    if (param.Value == null)
                    {
                        parameter.Value = DBNull.Value;
                    }
                    else
                    {
                        parameter.Value = param.Value;
                    }

                    cmd.Parameters.Add(parameter);
                }
            }
        }

        public static MySqlCommand TransactionInitStoreProcedureCommand(MySqlConnection conn, MySqlTransaction trans, string sqlCmd, Dictionary<string, object> parms)
        {
            // Command Objects for the transaction
            MySqlCommand cmdCreateRoot = new MySqlCommand(sqlCmd, conn);
            cmdCreateRoot.CommandType = CommandType.StoredProcedure;
            cmdCreateRoot.PrepareCommand(parms, trans);

            return cmdCreateRoot;
        }
    }
}
