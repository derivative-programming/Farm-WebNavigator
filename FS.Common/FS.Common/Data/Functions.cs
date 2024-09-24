using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FS.Common.Data
{
    public static class Functions
    {
        public static string SetApplicationName(string connString, string appName)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder connBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connString);
            // ---------------------------------------------------------------------
            // ---------------------------------------------------------------------
            // Use machine and process name
            // ---------------------------------------------------------------------
            // ---------------------------------------------------------------------
            connBuilder.ApplicationName = string.Format("{0}/{1}", Environment.MachineName, appName);

            // ---------------------------------------------------------------------
            // ---------------------------------------------------------------------
            // Application name can not be greater then 128 characters. If so, just
            // take the first 128.
            // ---------------------------------------------------------------------
            // ---------------------------------------------------------------------
            if (connBuilder.ApplicationName.Length > 128)
            {
                connBuilder.ApplicationName = connBuilder.ApplicationName.Substring(0, 127);
            }

            return connBuilder.ConnectionString;

        }

        //private static System.Data.SqlClient.SqlConnectionStringBuilder SetConnectionTimeOut(System.Data.SqlClient.SqlConnectionStringBuilder connBuilder)
        //{

        //    //connBuilder.ConnectTimeout = AT2.Base.Configuration.ApplicationSetting.ReadApplicationSettingAs<System.Int32>(DEFAULT_TIMEOUT_KEY, DEFAULT_TIMEOUT_SETTING);

        //    return connBuilder;

        //}

        //public enum SqlConnectionOwnership
        //{
        //    /// <summary>Connection is owned and managed by SqlHelper</summary>
        //    Internal,
        //    /// <summary>Connection is owned and managed by the caller</summary>
        //    External
        //}
        //public static async Task<SqlDataReader> ExecuteReaderAsync(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        //{

        //    //log this
        //    if (connection == null) throw new ArgumentNullException("connection");

        //    bool mustCloseConnection = false;
        //    // Create a command and prepare it for execution
        //    SqlCommand cmd = new SqlCommand();
        //    try
        //    {
        //        PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
        //        cmd.CommandTimeout = 60;
        //        // Create a reader
        //        SqlDataReader dataReader;
        //        Stopwatch timer = new Stopwatch();
        //        timer.Start();

        //        // Call ExecuteReader with the appropriate CommandBehavior
        //        if (connectionOwnership == SqlConnectionOwnership.External)
        //        {
        //            dataReader = await cmd.ExecuteReaderAsync();
        //        }
        //        else
        //        {
        //            dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        //        }

        //        timer.Stop();

        //        if (timer.ElapsedMilliseconds > 500)
        //        {
        //            string message = "Warning...ExecuteReader DB query is taking longer than expected.";
        //            message += Environment.NewLine + "timer.ElapsedMilliseconds: " + timer.ElapsedMilliseconds.ToString();
        //            message += Environment.NewLine + "commandText: " + commandText.ToString();

        //            if (commandParameters != null)
        //            {
        //                for (int i = 0; i < commandParameters.Length; i++)
        //                {
        //                    message += Environment.NewLine + "parameter: " + commandParameters[i].ParameterName.ToString();
        //                    if (commandParameters[i].Value != null)
        //                    {
        //                        message += " value: " + commandParameters[i].Value.ToString();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                message += Environment.NewLine + "(commandParameters was null)";
        //            }

        //        }

        //        // Detach the SqlParameters from the command object, so they can be used again.
        //        // HACK: There is a problem here, the output parameter values are fletched 
        //        // when the reader is closed, so if the parameters are detached from the command
        //        // then the SqlReader can´t set its values. 
        //        // When this happen, the parameters can´t be used again in other command.
        //        bool canClear = true;
        //        foreach (SqlParameter commandParameter in cmd.Parameters)
        //        {
        //            if (commandParameter.Direction != ParameterDirection.Input)
        //                canClear = false;
        //        }

        //        if (canClear)
        //        {
        //            cmd.Parameters.Clear();
        //        }

        //        return dataReader;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (mustCloseConnection)
        //            connection.Close();

        //        throw new Exception("An error occurred, see inner exception.", ex);
        //    }
        //}

        //public static async Task<SqlDataReader> ExecuteReaderAsync(SqlTransaction transaction, CommandType commandType, string commanVRxt, params SqlParameter[] commandParameters)
        //{
        //    if (transaction == null) throw new ArgumentNullException("transaction");
        //    if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        //    // Pass through to private overload, indicating that the connection is owned by the caller
        //    return await ExecuteReaderAsync(transaction.Connection, transaction, commandType, commanVRxt, commandParameters, SqlConnectionOwnership.External);
        //}

        //public static async Task<SqlDataReader> ExecuteReaderAsync(string connectionString, CommandType commandType, string commanVRxt, params SqlParameter[] commandParameters)
        //{
        //    if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        //    SqlConnection connection = null;
        //    try
        //    {
        //        connection = new SqlConnection(connectionString);
        //        connection.Open();

        //        // Call the private overload that takes an internally owned connection in place of the connection string
        //        return await ExecuteReaderAsync(connection, null, commandType, commanVRxt, commandParameters, SqlConnectionOwnership.Internal);
        //    }
        //    catch
        //    {
        //        // If we fail to return the SqlDatReader, we need to close the connection ourselves
        //        if (connection != null) connection.Close();
        //        throw;
        //    }

        //}

        //public static async Task<int> ExecuteNonQueryAsync(SqlConnection connection, CommandType commandType, string commanVRxt, params SqlParameter[] commandParameters)
        //{
        //    //log this
        //    if (connection == null) throw new ArgumentNullException("connection");

        //    // Create a command and prepare it for execution
        //    SqlCommand cmd = new SqlCommand();
        //    bool mustCloseConnection = false;
        //    PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commanVRxt, commandParameters, out mustCloseConnection);

        //    // Finally, execute the command
        //    int starttick = System.Environment.TickCount;
        //    int retval = await cmd.ExecuteNonQueryAsync();
        //    int tickDuration = System.Environment.TickCount - starttick;
        //    if (tickDuration > 500)
        //    {
        //        string message = "Warning...ExecuteNonQuery DB query is taking longer than expected.";
        //        message += Environment.NewLine + "Duration (ticks): " + tickDuration.ToString();
        //        message += Environment.NewLine + "commanText: " + commanVRxt.ToString();
        //        for (int i = 0; i < commandParameters.Length; i++)
        //        {
        //            message += Environment.NewLine + "parameter: " + commandParameters[i].ParameterName.ToString();
        //            if (commandParameters[i].Value != null)
        //            {
        //                message += " value: " + commandParameters[i].Value.ToString();
        //            }
        //        }
        //    }
        //    // Detach the SqlParameters from the command object, so they can be used again
        //    cmd.Parameters.Clear();
        //    if (mustCloseConnection)
        //        connection.Close();
        //    return retval;
        //}

        //public static async Task<int> ExecuteNonQueryAsync(SqlTransaction transaction, CommandType commandType, string commanVRxt, params SqlParameter[] commandParameters)
        //{
        //    if (transaction == null) throw new ArgumentNullException("transaction");
        //    if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

        //    // Create a command and prepare it for execution
        //    SqlCommand cmd = new SqlCommand();
        //    bool mustCloseConnection = false;
        //    PrepareCommand(cmd, transaction.Connection, transaction, commandType, commanVRxt, commandParameters, out mustCloseConnection);

        //    // Finally, execute the command
        //    int retval = await cmd.ExecuteNonQueryAsync();

        //    // Detach the SqlParameters from the command object, so they can be used again
        //    cmd.Parameters.Clear();
        //    return retval;
        //}

        //public static async Task<int> ExecuteNonQueryAsync(string connectionString, CommandType commandType, string commanVRxt, params SqlParameter[] commandParameters)
        //{
        //    if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");

        //    // Create & open a SqlConnection, and dispose of it after we are done
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        // Call the overload that takes a connection in place of the connection string
        //        return await ExecuteNonQueryAsync(connection, commandType, commanVRxt, commandParameters);
        //    }
        //}

        ///// <summary>
        ///// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        ///// to the provided command
        ///// </summary>
        ///// <param name="command">The SqlCommand to be prepared</param>
        ///// <param name="connection">A valid SqlConnection, on which to execute this command</param>
        ///// <param name="transaction">A valid SqlTransaction, or 'null'</param>
        ///// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        ///// <param name="commanVRxt">The stored procedure name or T-SQL command</param>
        ///// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
        ///// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
        //private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commanVRxt, SqlParameter[] commandParameters, out bool mustCloseConnection)
        //{
        //    if (command == null) throw new ArgumentNullException("command");
        //    if (commanVRxt == null || commanVRxt.Length == 0) throw new ArgumentNullException("commanVRxt");

        //    // If the provided connection is not open, we will open it
        //    if (connection.State != ConnectionState.Open)
        //    {
        //        mustCloseConnection = true;
        //        connection.Open();
        //    }
        //    else
        //    {
        //        mustCloseConnection = false;
        //    }

        //    // Associate the connection with the command
        //    command.Connection = connection;

        //    // Set the command text (stored procedure name or SQL statement)
        //    command.CommandText = commanVRxt;

        //    // If we were provided a transaction, assign it
        //    if (transaction != null)
        //    {
        //        if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        //        command.Transaction = transaction;
        //    }

        //    // Set the command type
        //    command.CommandType = commandType;

        //    // Attach the command parameters if they are provided
        //    if (commandParameters != null)
        //    {
        //        AttachParameters(command, commandParameters);
        //    }
        //    return;
        //}
        ///// <summary>
        ///// This method is used to attach array of SqlParameters to a SqlCommand.
        ///// 
        ///// This method will assign a value of DbNull to any parameter with a direction of
        ///// InputOutput and a value of null.  
        ///// 
        ///// This behavior will prevent default values from being used, but
        ///// this will be the less common case than an intended pure output parameter (derived as InputOutput)
        ///// where the user provided no input value.
        ///// </summary>
        ///// <param name="command">The command to which the parameters will be added</param>
        ///// <param name="commandParameters">An array of SqlParameters to be added to command</param>
        //private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        //{
        //    if (command == null) throw new ArgumentNullException("command");
        //    if (commandParameters != null)
        //    {
        //        foreach (SqlParameter p in commandParameters)
        //        {
        //            if (p != null)
        //            {
        //                // Check for derived output value with no value assigned
        //                if ((p.Direction == ParameterDirection.InputOutput ||
        //                    p.Direction == ParameterDirection.Input) &&
        //                    (p.Value == null))
        //                {
        //                    p.Value = DBNull.Value;
        //                }
        //                command.Parameters.Add(p);
        //            }
        //        }
        //    }
        //}
    }
}
