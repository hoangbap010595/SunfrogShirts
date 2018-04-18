using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MainUploadV2.DataHelper
{
    public class ConnectionStoreDB
    {
        public ConnectionStoreDB() { }
        //private string GetDBConnection
        //{
        //    get
        //    {
        //        string datasource = "103.53.231.198";
        //        string database = "AutoUploadShirt";
        //        string username = "autoupload";
        //        string password = "Abc@123";
        //        string connString = @"Data Source=" + datasource + ";Initial Catalog="
        //                    + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;// co the dung lenh nay
        //        return connString;
        //    }
        //}

        private string GetDBConnection
        {
            get
            {
                string datasource = @".\SQLEXPRESS";
                string database = "TimViecNhanh";
                string username = "sa";
                string password = "Hoang911";
                string connString = @"Data Source=" + datasource + ";Initial Catalog="
                            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;// co the dung lenh nay
                return connString;
            }
        }

        private SqlConnection Connection { get; set; }

        private SqlCommand Command { get; set; }

        public List<DbParameter> OutParameters { get; private set; }

        private void Open()
        {
            try
            {
                Connection = new SqlConnection(GetDBConnection);
                Connection.Open();
            }
            catch (Exception ex)
            {
                Close();
            }
        }

        private void Close()
        {
            if (Connection != null)
            {
                Connection.Close();
            }
        }

        public SqlDataReader CreateCommand(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(GetDBConnection))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandTimeout = 15;
                command.CommandType = CommandType.Text;
                command.CommandText = queryString;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Close();
                return reader;
            }
        }
        public bool ExecuteProcedure(string storeString, List<ParametersStore> parameterStore)
        {
            SqlDataReader sqlData = null;
            try
            {
                Open();

                if (Connection != null)
                {
                    if (Connection.State == ConnectionState.Open)
                    {
                        using (SqlCommand command = Connection.CreateCommand())
                        {
                            command.CommandText = storeString;
                            command.CommandType = CommandType.StoredProcedure;
                            foreach (var item in parameterStore)
                            {
                                command.Parameters.Add(new SqlParameter("@" + item.Text, item.Value));
                            }
                            sqlData = command.ExecuteReader();
                        }
                        Close();
                        return true;
                    }
                }
                else
                {
                    Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Close();
                return false;
            }
        }

        public DataTable RunStoredProc(string storeString)
        {
            SqlDataReader rdr = null;
            DataTable table = new DataTable();
            try
            {
                Open();
                if (Connection != null)
                {
                    if (Connection.State == ConnectionState.Open)
                    {

                        // 1. create a command object identifying
                        // the stored procedure
                        SqlCommand cmd = new SqlCommand(storeString, Connection);

                        // 2. set the command object so it knows
                        // to execute a stored procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        // execute the command
                        rdr = cmd.ExecuteReader();

                        table.Load(rdr);
                    }
                }

                Close();
                return table;
                // iterate through results, printing each to console
            }
            catch (Exception ex)
            {

                Close();
                return null;
            }

        }
        public DataTable GetExecuteProcedure(string storeString, List<ParametersStore> parameterStore)
        {
            SqlDataReader sqlData = null;
            DataTable table = new DataTable();
            try
            {
                Open();
                if (Connection != null)
                {
                    if (Connection.State == ConnectionState.Open)
                    {
                        using (SqlCommand command = Connection.CreateCommand())
                        {
                            command.CommandText = storeString;
                            command.CommandType = CommandType.StoredProcedure;
                            foreach (var item in parameterStore)
                            {
                                command.Parameters.Add(new SqlParameter("@" + item.Text, item.Value));
                            }
                            sqlData = command.ExecuteReader();
                            table.Load(sqlData);
                        }
                    }
                }
                Close();
                return table;
            }
            catch (Exception ex)
            {
                Close();
                return table;
            }
        }

        public int ReturnExecuteScalar(string storeString, List<ParametersStore> parameterStore)
        {

            int sqlData = -1;
            try
            {
                Open();
                if (Connection != null)
                {
                    if (Connection.State == ConnectionState.Open)
                    {
                        using (SqlCommand command = Connection.CreateCommand())
                        {
                            command.CommandText = storeString;
                            command.CommandType = CommandType.StoredProcedure;
                            foreach (var item in parameterStore)
                            {
                                command.Parameters.Add(new SqlParameter("@" + item.Text, item.Value));
                            }

                            var sqlA = command.ExecuteScalar();


                        }
                    }
                }
                Close();
                return sqlData;
            }
            catch (Exception ex)
            {
                Close();
                return sqlData;
            }
        }
        public int ReturnExecuteScalar(string storeString)
        {
            int rdr = -1;
            try
            {
                Open();
                if (Connection != null)
                {
                    if (Connection.State == ConnectionState.Open)
                    {
                        // 1. create a command object identifying
                        // the stored procedure
                        SqlCommand cmd = new SqlCommand(storeString, Connection);
                        // 2. set the command object so it knows
                        // to execute a stored procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        // execute the command
                        rdr = (int)cmd.ExecuteScalar();


                    }
                }
                Close();
                return rdr;
                // iterate through results, printing each to console
            }
            catch (Exception ex)
            {

                Close();
                return rdr;
            }
        }

        // executes stored procedure with DB parameteres if they are passed
        public object ExecuteProcedure(string procedureName, ExecuteType executeType, List<DbParameter> parameters)
        {
            object returnObject = null;
            Open();
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Command = new SqlCommand(procedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;

                    // pass stored procedure parameters to command
                    if (parameters != null)
                    {
                        Command.Parameters.Clear();

                        foreach (DbParameter dbParameter in parameters)
                        {
                            SqlParameter parameter = new SqlParameter();
                            parameter.ParameterName = "@" + dbParameter.Name;
                            parameter.Direction = dbParameter.Direction;
                            parameter.Value = dbParameter.Value;
                            Command.Parameters.Add(parameter);
                        }
                    }

                    switch (executeType)
                    {
                        case ExecuteType.ExecuteReader:
                            returnObject = Command.ExecuteReader();
                            break;
                        case ExecuteType.ExecuteNonQuery:
                            returnObject = Command.ExecuteNonQuery();
                            break;
                        case ExecuteType.ExecuteScalar:
                            returnObject = Command.ExecuteScalar();
                            break;
                        default:
                            break;
                    }
                }
            }

            return returnObject;
        }

        // updates output parameters from stored procedure
        private void UpdateOutParameters()
        {
            if (Command.Parameters.Count > 0)
            {
                OutParameters = new List<DbParameter>();
                OutParameters.Clear();

                for (int i = 0; i < Command.Parameters.Count; i++)
                {
                    if (Command.Parameters[i].Direction == ParameterDirection.Output)
                    {
                        OutParameters.Add(new DbParameter(Command.Parameters[i].ParameterName,
                                                          ParameterDirection.Output,
                                                          Command.Parameters[i].Value));
                    }
                }
            }
        }

        // executes scalar query stored procedure without parameters
        public T ExecuteSingle<T>(string procedureName) where T : new()
        {
            return ExecuteSingle<T>(procedureName, null);
        }

        // executes scalar query stored procedure and maps result to single object
        public T ExecuteSingle<T>(string procedureName, List<DbParameter> parameters) where T : new()
        {
            IDataReader reader = (IDataReader)ExecuteProcedure(procedureName, ExecuteType.ExecuteReader, parameters);
            T tempObject = new T();

            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var content = reader.GetValue(i);
                    if (content == System.DBNull.Value)
                    {
                        continue;
                    }
                    else
                    {
                        PropertyInfo propertyInfo = typeof(T).GetProperty(reader.GetName(i));
                        propertyInfo.SetValue(tempObject, content, null);
                    }

                }
            }

            reader.Close();

            UpdateOutParameters();

            Close();

            return tempObject;
        }

        // executes list query stored procedure without parameters
        public List<T> ExecuteList<T>(string procedureName) where T : new()
        {
            return ExecuteList<T>(procedureName, null);
        }

        // executes list query stored procedure and maps result generic list of objects
        public List<T> ExecuteList<T>(string procedureName, List<DbParameter> parameters) where T : new()
        {
            List<T> objects = new List<T>();

            IDataReader reader = (IDataReader)ExecuteProcedure(procedureName, ExecuteType.ExecuteReader, parameters);

            while (reader.Read())
            {
                T tempObject = new T();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetValue(i) != DBNull.Value)
                    {
                        var duLieu = reader.GetValue(i);
                        if (duLieu == null)
                        {
                            continue;
                        }
                        else
                        {
                            PropertyInfo propertyInfo = typeof(T).GetProperty(reader.GetName(i));
                            propertyInfo.SetValue(tempObject, duLieu, null);
                        }

                    }
                }

                objects.Add(tempObject);
            }

            reader.Close();

            UpdateOutParameters();

            Close();

            return objects;
        }

        // executes non query stored procedure with parameters
        public int ExecuteNonQuery(string procedureName, List<DbParameter> parameters)
        {
            int returnValue;

            Open();

            returnValue = (int)ExecuteProcedure(procedureName, ExecuteType.ExecuteNonQuery, parameters);

            UpdateOutParameters();

            Close();

            return returnValue;
        }


        #region code cua công

        public List<Dictionary<string, object>> ConvertDataTableToList(string storeString, List<ParametersStore> parameterStore = null)
        {
            List<Dictionary<string, object>> listData = new List<Dictionary<string, object>>();
            DataTable dt = null;
            if (parameterStore == null)
                dt = RunStoredProc(storeString);
            else
                dt = GetExecuteProcedure(storeString, parameterStore);
            if (dt == null || dt.Rows.Count == 0)
                return listData;
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    rowData.Add(col.ColumnName, dr[col]);
                }
                listData.Add(rowData);
            }
            return listData;
        }
        #endregion

    }
}
