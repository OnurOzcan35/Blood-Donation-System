using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace BloodDonation.Requirements
{
    public class DataLayer
    {
        private string _connstring;
        private OleDbConnection _conn;
        private OleDbTransaction _transaction;
        public bool _IsActiveTransaction;

        public DataLayer(string strDbServer, string strUserName, string strPassword, string strDbName)
        {
            _connstring = "Provider = SQLOLEDB.1;Data Source=" + strDbServer + ";Initial Catalog=" + strDbName + ";User Id =" + strUserName + ";Password =" + strPassword + ";Auto Translate=False";
            _conn = new OleDbConnection(_connstring);
        }

        public DataLayer(string ConnString)
        {
            _connstring = ConnString;
            _conn = new OleDbConnection(_connstring);
        }
        public DataTable GetDataFromTable(string strQuery, DataTable parameters)
        {
            try
            {
                DataTable dtResult = new DataTable();
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    da.SelectCommand = new OleDbCommand();
                    da.SelectCommand.Connection = _conn;
                    da.SelectCommand.CommandType = CommandType.Text;
                    da.SelectCommand.CommandText = strQuery;
                    da.SelectCommand.CommandTimeout = 0;
                    if (parameters != null)
                    {
                        foreach (DataRow drParam in parameters.Rows)
                        {
                            OleDbParameter param = new OleDbParameter(drParam["ParameterName"].ToString(), (OleDbType)drParam["ParameterType"]);
                            param.Value = drParam["ParameterValue"];
                            da.SelectCommand.Parameters.Add(param);
                            param = null;
                        }
                    }
                    da.Fill(dtResult);
                }
                return dtResult;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        public DataTable GetDataFromStoredProcedure(string strProcName, DataTable parameters)
        {
            try
            {
                DataTable dtResult = new DataTable();
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    da.SelectCommand = new OleDbCommand();
                    da.SelectCommand.Connection = _conn;
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandText = strProcName;
                    da.SelectCommand.CommandTimeout = 0;
                    if (parameters != null)
                    {
                        foreach (DataRow drParam in parameters.Rows)
                        {
                            OleDbParameter param = new OleDbParameter(drParam["ParameterName"].ToString(), (OleDbType)drParam["ParameterType"]);
                            param.Value = drParam["ParameterValue"];
                            da.SelectCommand.Parameters.Add(param);
                            param = null;
                        }
                    }
                    da.Fill(dtResult);
                }
                return dtResult;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        public int ExecQuery(string strQuery, DataTable parameters)
        {

            try
            {

                int intRecordsAffected;
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQuery;
                cmd.CommandTimeout = 0;
                if (parameters != null)
                {
                    foreach (DataRow drParam in parameters.Rows)
                    {
                        OleDbParameter param = new OleDbParameter(drParam["ParameterName"].ToString(), (OleDbType)drParam["ParameterType"]);
                        param.Value = drParam["ParameterValue"];
                        cmd.Parameters.Add(param);
                        param = null;

                    }
                   ;
                }
                if ((_conn.State == ConnectionState.Closed) || (_conn.State == ConnectionState.Broken))
                {
                    _conn.Open();
                }

                intRecordsAffected = cmd.ExecuteNonQuery();
                return intRecordsAffected;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }

        }

        public object ExecQueryScalar(string strQuery, DataTable parameters)
        {
            try
            {


                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQuery;
                cmd.CommandTimeout = 0;
                if (parameters != null)
                {
                    foreach (DataRow drParam in parameters.Rows)
                    {
                        OleDbParameter param = new OleDbParameter(drParam["ParameterName"].ToString(), (OleDbType)drParam["ParameterType"]);
                        param.Value = drParam["ParameterValue"];
                        cmd.Parameters.Add(param);
                        param = null;
                    }
                }
                if ((_conn.State == ConnectionState.Closed) || (_conn.State == ConnectionState.Broken))
                {
                    _conn.Open();
                }

                return cmd.ExecuteScalar();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }

        }

        public void BeginTran()
        {
            if ((_conn.State == ConnectionState.Closed) || (_conn.State == ConnectionState.Broken))
            {
                _conn.Open();
                _transaction = _conn.BeginTransaction();
                _IsActiveTransaction = true;
            }
        }

        public void CommitTran()
        {
            _transaction.Commit();
            _IsActiveTransaction = false;
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }

        public void RollbackTran()
        {
            _transaction.Rollback();
            _IsActiveTransaction = false;
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }

        public object ExecQueryWithTransaction(string strQuery, DataTable parameters)
        {
            try
            {


                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQuery;
                cmd.Transaction = _transaction;
                if (parameters != null)
                {
                    foreach (DataRow drParam in parameters.Rows)
                    {
                        OleDbParameter param = new OleDbParameter(drParam["ParameterName"].ToString(), drParam["ParameterValue"]);
                        cmd.Parameters.Add(param);
                        param = null;
                    }
                }
                if ((_conn.State == ConnectionState.Closed) || (_conn.State == ConnectionState.Broken))
                {
                    _conn.Open();
                }

                return cmd.ExecuteScalar();

            }
            catch (Exception exc)
            {
                throw exc;
            }


        }


        public DataTable GetDataFromTableWithTransaction(string strQuery, DataTable parameters)
        {
            try
            {
                DataTable dtResult = new DataTable();
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    da.SelectCommand = new OleDbCommand();
                    da.SelectCommand.Connection = _conn;
                    da.SelectCommand.CommandType = CommandType.Text;
                    da.SelectCommand.CommandText = strQuery;
                    da.SelectCommand.CommandTimeout = 0;
                    da.SelectCommand.Transaction = _transaction;
                    if (parameters != null)
                    {
                        foreach (DataRow drParam in parameters.Rows)
                        {
                            OleDbParameter param = new OleDbParameter(drParam["ParameterName"].ToString(), (OleDbType)drParam["ParameterType"]);
                            param.Value = drParam["ParameterValue"];
                            da.SelectCommand.Parameters.Add(param);
                            param = null;
                        }
                    }
                    da.Fill(dtResult);
                }
                return dtResult;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        public DataTable GetDataFromStoredProcedureWithTransaction(string strProcName, DataTable parameters)
        {
            try
            {
                DataTable dtResult = new DataTable();
                using (OleDbDataAdapter da = new OleDbDataAdapter())
                {
                    da.SelectCommand = new OleDbCommand();
                    da.SelectCommand.Connection = _conn;
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandText = strProcName;
                    da.SelectCommand.CommandTimeout = 0;
                    da.SelectCommand.Transaction = _transaction;
                    if (parameters != null)
                    {
                        foreach (DataRow drParam in parameters.Rows)
                        {
                            OleDbParameter param = new OleDbParameter(drParam["ParameterName"].ToString(), (OleDbType)drParam["ParameterType"]);
                            param.Value = drParam["ParameterValue"];
                            da.SelectCommand.Parameters.Add(param);
                            param = null;
                        }
                    }
                    da.Fill(dtResult);
                }
                return dtResult;
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

    }
}
