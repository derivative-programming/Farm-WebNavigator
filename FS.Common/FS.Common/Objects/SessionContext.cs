using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FS.Common.Authentication;
using Npgsql;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace FS.Common.Objects
{
    public class SessionContext
    {
        public Guid ObjDataSetCode { get; set; }
        public Guid UserID { get; set; }
        public Guid CustomerCode { get; set; }
        public string CustomerRoleCSVList { get; set; }
        public int UTCOffsetInMinutes { get; set; }
        public string UserName { get; set; }
        public bool IsImpersonatingUser { get; set; }
        public List<string> ImpersonationChainUserID { get; set; }
        public List<string> ImpersonationChainUserName { get; set; }
        public string AppName { get; set; }
        public string RequestCode { get; set; }
        public string SessionCode { get; set; }
        public bool LoggingBroadcastForced { get; set; }
        public bool LoggingBroadcastForcedSet { get; set; }
        public int LoggingVerboseLevelForced { get; set; }
        public bool LoggingVerboseLevelForcedSet { get; set; }
        public System.Collections.Specialized.NameValueCollection SessionCache { get; set; }
        public System.Collections.Specialized.NameValueCollection RequestCache { get; set; }
        public string CreatedDateTime { get; set; }
        public List<ConnectionInstance> DBConnections { get; set; }
        public bool UseTransactions { get; set; }
        public bool TransactionStarted { get; set; }
        public DateTime TransactionStartedUTCDateTime { get; set; }
        public bool TransactionEnded { get; set; }
        public bool TransactionFailed { get; set; }
        public bool CacheNoneForced { get; set; }
        public bool CacheAllForced { get; set; }
        public bool CacheIndividualForced { get; set; }

        public Dictionary<string, string> LoggingDimensions {get;set;}

        public List<Task> Tasks { get; set; }


        public Semaphore Bouncer { get; set; }

        public SessionContext(bool useTransactions)
        {
            this.ObjDataSetCode = Guid.Empty;
            this.UserID = Guid.NewGuid();
            this.CustomerCode = Guid.NewGuid();
            this.CustomerRoleCSVList = string.Empty;
            this.UTCOffsetInMinutes = 0;
            this.UserName = string.Empty;
            this.UseTransactions = useTransactions;
            this.DBConnections = new List<ConnectionInstance>();
            this.Tasks = new List<Task>();
            this.Bouncer = new Semaphore(1, 1);
            this.IsImpersonatingUser = false;
            this.ImpersonationChainUserID = new List<string>();
            this.ImpersonationChainUserName = new List<string>();
            this.AppName = string.Empty;
            this.RequestCode = string.Empty;
            this.SessionCode = Guid.NewGuid().ToString();
            this.LoggingBroadcastForced = false;
            this.LoggingBroadcastForcedSet = false;
            this.LoggingVerboseLevelForced = 0;
            this.LoggingVerboseLevelForcedSet = false;
            this.CreatedDateTime = DateTime.UtcNow.ToString();
            this.TransactionStarted = false;
            this.TransactionStartedUTCDateTime = DateTime.UtcNow; 
            this.TransactionEnded = false;
            this.TransactionFailed = false;
            this.LoggingDimensions = new Dictionary<string, string>();
        }

        public SessionContext(bool useTransactions, AuthenticationToken authenticationToken)
        {
            this.ObjDataSetCode = Guid.Parse(authenticationToken.ObjDataSetCode);
            this.UserID = Guid.Parse(authenticationToken.UserID);
            this.CustomerCode = Guid.Parse(authenticationToken.CustomerCode);
            this.CustomerRoleCSVList = authenticationToken.CustomerRoleListCSV; 
            this.UserName = authenticationToken.UserName;
            this.UseTransactions = useTransactions;
            this.DBConnections = new List<ConnectionInstance>();
            this.Tasks = new List<Task>();
            this.Bouncer = new Semaphore(1, 1);
            this.IsImpersonatingUser = false;
            this.UTCOffsetInMinutes = 0;
            this.ImpersonationChainUserID = new List<string>();
            this.ImpersonationChainUserName = new List<string>();
            this.AppName = string.Empty;
            this.RequestCode = string.Empty;
            this.SessionCode = Guid.NewGuid().ToString();
            this.LoggingBroadcastForced = false;
            this.LoggingBroadcastForcedSet = false;
            this.LoggingVerboseLevelForced = 0;
            this.LoggingVerboseLevelForcedSet = false;
            this.CreatedDateTime = DateTime.UtcNow.ToString();
            this.TransactionStarted = false;
            this.TransactionStartedUTCDateTime = DateTime.UtcNow;
            this.TransactionEnded = false;
            this.TransactionFailed = false;
            this.LoggingDimensions = new Dictionary<string, string>();
        }

        public void SetLoggingDimension(string dimensionName, string dimensionValue)
        {
            if(this.LoggingDimensions.Keys.Contains(dimensionName))
            {
                this.LoggingDimensions.Add(dimensionName, dimensionValue);
            }
            else
            {
                this.LoggingDimensions[dimensionName] = dimensionValue;
            }
        }



        public void LockSession()
        {
            if(this.UseTransactions)
                this.Bouncer.WaitOne();
        }

        public void ReleaseSession()
        {
            if(this.UseTransactions)
                this.Bouncer.Release(1);
        }

        public async Task WaitOnAllTasksAsync()
        { 
            for(int i = 0;i < this.Tasks.Count;i++)
            {
                await this.Tasks[i]; 
            }
            this.Tasks.Clear();
        }

        public void RollBackTransactions()
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsSqlServer)
                    continue;
                System.Data.SqlClient.SqlTransaction sqlTransaction = this.DBConnections[i].SqlTransaction;
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                    this.DBConnections[i].SqlTransaction = null;

                    if (DateTime.UtcNow.Subtract(TransactionStartedUTCDateTime).TotalSeconds > 2)
                    {
                        FS.Common.Diagnostics.Loggers.Manager.LogMessage(this, Diagnostics.Loggers.ApplicationLogEntrySeverities.Warning,
                            "Rollback of Transaction. Long Running Transaction. Duration in seconds:" + DateTime.UtcNow.Subtract(TransactionStartedUTCDateTime).TotalSeconds.ToString() + ", username: " + this.UserName + ", sessionCode:" + this.SessionCode.ToString());
                    }
                }


                System.Data.SqlClient.SqlConnection connection = this.DBConnections[i].SqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsPostgres)
                    continue;
                NpgsqlTransaction transaction = this.DBConnections[i].NpgsqlTransaction;
                if (transaction != null)
                {
                    transaction.Rollback();
                    this.DBConnections[i].NpgsqlTransaction = null;
                }

                NpgsqlConnection connection = this.DBConnections[i].NpgsqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsMySql)
                    continue;
                MySqlTransaction transaction = this.DBConnections[i].MySqlTransaction;
                if (transaction != null)
                {
                    transaction.Rollback();
                    this.DBConnections[i].MySqlTransaction = null;
                }

                MySqlConnection connection = this.DBConnections[i].MySqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            TransactionEnded = true;
            TransactionFailed = true;
            this.DBConnections.Clear();
        }


        public async Task RollBackTransactionsAsync()
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsSqlServer)
                    continue;
                System.Data.SqlClient.SqlTransaction sqlTransaction = this.DBConnections[i].SqlTransaction;
                if (sqlTransaction != null)
                {
                    await sqlTransaction.RollbackAsync();
                    this.DBConnections[i].SqlTransaction = null;

                    if (DateTime.UtcNow.Subtract(TransactionStartedUTCDateTime).TotalSeconds > 2)
                    {
                        await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(this, Diagnostics.Loggers.ApplicationLogEntrySeverities.Warning,
                            "Rollback of Transaction. Long Running Transaction. Duration in seconds:" + DateTime.UtcNow.Subtract(TransactionStartedUTCDateTime).TotalSeconds.ToString() + ", username: " + this.UserName + ", sessionCode:" + this.SessionCode.ToString());
                    }
                }


                System.Data.SqlClient.SqlConnection connection = this.DBConnections[i].SqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }

            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsPostgres)
                    continue;
                NpgsqlTransaction transaction = this.DBConnections[i].NpgsqlTransaction;
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                    this.DBConnections[i].NpgsqlTransaction = null;
                }

                NpgsqlConnection connection = this.DBConnections[i].NpgsqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsMySql)
                    continue;
                MySqlTransaction transaction = this.DBConnections[i].MySqlTransaction;
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                    this.DBConnections[i].MySqlTransaction = null;
                }

                MySqlConnection connection = this.DBConnections[i].MySqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }
            TransactionEnded = true;
            TransactionFailed = true;
            this.DBConnections.Clear();
        }

        public void CommitTransactions()
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsSqlServer)
                    continue;
                System.Data.SqlClient.SqlTransaction sqlTransaction = this.DBConnections[i].SqlTransaction;
                if (sqlTransaction != null)
                {
                    sqlTransaction.Commit();
                    this.DBConnections[i].SqlTransaction = null;

                    if (DateTime.UtcNow.Subtract(TransactionStartedUTCDateTime).TotalSeconds > 2)
                    {
                        FS.Common.Diagnostics.Loggers.Manager.LogMessage(this, Diagnostics.Loggers.ApplicationLogEntrySeverities.Warning,
                            "Long Running Transaction. Duration in seconds:" + DateTime.UtcNow.Subtract(TransactionStartedUTCDateTime).TotalSeconds.ToString() + ", username: " + this.UserName + ", sessionCode:" + this.SessionCode.ToString());
                    }
                }

                System.Data.SqlClient.SqlConnection connection = this.DBConnections[i].SqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsPostgres)
                    continue;
                NpgsqlTransaction transaction = this.DBConnections[i].NpgsqlTransaction;
                if (transaction != null)
                {
                    transaction.Commit();
                    this.DBConnections[i].NpgsqlTransaction = null;
                }

                NpgsqlConnection connection = this.DBConnections[i].NpgsqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsMySql)
                    continue;
                MySqlTransaction transaction = this.DBConnections[i].MySqlTransaction;
                if (transaction != null)
                {
                    transaction.Commit();
                    this.DBConnections[i].MySqlTransaction = null;
                }

                MySqlConnection connection = this.DBConnections[i].MySqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            TransactionEnded = true;
            TransactionFailed = false;
            this.DBConnections.Clear();
        }


        public async Task CommitTransactionsAsync()
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsSqlServer)
                    continue;
                System.Data.SqlClient.SqlTransaction sqlTransaction = this.DBConnections[i].SqlTransaction;
                if (sqlTransaction != null)
                {
                    await sqlTransaction.CommitAsync();
                    this.DBConnections[i].SqlTransaction = null;

                    if (DateTime.UtcNow.Subtract(TransactionStartedUTCDateTime).TotalSeconds > 2)
                    {
                        await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(this, Diagnostics.Loggers.ApplicationLogEntrySeverities.Warning,
                            "Long Running Transaction. Duration in seconds:" + DateTime.UtcNow.Subtract(TransactionStartedUTCDateTime).TotalSeconds.ToString() + ", username: " + this.UserName + ", sessionCode:" + this.SessionCode.ToString());
                    }
                }

                System.Data.SqlClient.SqlConnection connection = this.DBConnections[i].SqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsPostgres)
                    continue;
                NpgsqlTransaction transaction = this.DBConnections[i].NpgsqlTransaction;
                if (transaction != null)
                {
                    await transaction.CommitAsync();
                    this.DBConnections[i].NpgsqlTransaction = null;
                }

                NpgsqlConnection connection = this.DBConnections[i].NpgsqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }

            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsMySql)
                    continue;
                MySqlTransaction transaction = this.DBConnections[i].MySqlTransaction;
                if (transaction != null)
                {
                    await transaction.CommitAsync();
                    this.DBConnections[i].MySqlTransaction = null;
                }

                MySqlConnection connection = this.DBConnections[i].MySqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }
            TransactionEnded = true;
            TransactionFailed = false;
            this.DBConnections.Clear();
        }


        public void CloseTransactions()
        {
            if (UseTransactions)
                return;

            for (int i = 0; i < DBConnections.Count; i++)
            {  
                if(!this.DBConnections[i].IsSqlServer)
                    continue;
                System.Data.SqlClient.SqlConnection connection = this.DBConnections[i].SqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsPostgres)
                    continue;
                NpgsqlConnection connection = this.DBConnections[i].NpgsqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsMySql)
                    continue;
                MySqlConnection connection = this.DBConnections[i].MySqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
            this.DBConnections.Clear();
        }


        public async Task CloseTransactionsAsync()
        {
            if (UseTransactions)
                return;

            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsSqlServer)
                    continue;
                System.Data.SqlClient.SqlConnection connection = this.DBConnections[i].SqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsPostgres)
                    continue;
                NpgsqlConnection connection = this.DBConnections[i].NpgsqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (!this.DBConnections[i].IsMySql)
                    continue;
                MySqlConnection connection = this.DBConnections[i].MySqlConnection;
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    await connection.CloseAsync();
            }
            this.DBConnections.Clear();
        }

        public System.Data.SqlClient.SqlConnection GetSqlConnection(string connectionString)
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (this.DBConnections[i].ConnectionString.ToLower().Trim() == connectionString.ToLower().Trim() &&
                    this.DBConnections[i].IsSqlServer == true)
                    return this.DBConnections[i].SqlConnection; 
            }
            return null;
        }


        public NpgsqlConnection GetNpgsqlConnection(string connectionString)
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (this.DBConnections[i].ConnectionString.ToLower().Trim() == connectionString.ToLower().Trim() &&
                    this.DBConnections[i].IsPostgres == true)
                    return this.DBConnections[i].NpgsqlConnection;
            }
            return null;
        }
        public MySqlConnection GetMySqlConnection(string connectionString)
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (this.DBConnections[i].ConnectionString.ToLower().Trim() == connectionString.ToLower().Trim() &&
                    this.DBConnections[i].IsMySql == true)
                    return this.DBConnections[i].MySqlConnection;
            }
            return null;
        }

        public bool SqlConnectionExists(string connectionString)
        {
            return (GetSqlConnection(connectionString) != null);
        }
        public bool NpgsqlConnectionExists(string connectionString)
        {
            return (GetNpgsqlConnection(connectionString) != null);
        }
        public bool MySqlConnectionExists(string connectionString)
        {
            return (GetMySqlConnection(connectionString) != null);
        }

        public System.Data.SqlClient.SqlTransaction GetSqlTransaction(string connectionString)
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (this.DBConnections[i].ConnectionString.ToLower().Trim() == connectionString.ToLower().Trim() && this.DBConnections[i].IsSqlServer == true)
                    return this.DBConnections[i].SqlTransaction;
            }
            return null;
        }
        public NpgsqlTransaction GetNpgsqlTransaction(string connectionString)
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (this.DBConnections[i].ConnectionString.ToLower().Trim() == connectionString.ToLower().Trim() && this.DBConnections[i].IsPostgres == true)
                    return this.DBConnections[i].NpgsqlTransaction;
            }
            return null;
        }
        public MySqlTransaction GetMySqlTransaction(string connectionString)
        {
            for (int i = 0; i < DBConnections.Count; i++)
            {
                if (this.DBConnections[i].ConnectionString.ToLower().Trim() == connectionString.ToLower().Trim() && this.DBConnections[i].IsMySql == true)
                    return this.DBConnections[i].MySqlTransaction;
            }
            return null;
        }

        public bool SqlTransactionExists(string connectionString)
        {
            return (GetSqlTransaction(connectionString) != null);
        }
        public bool NpgsqlTransactionExists(string connectionString)
        {
            return (GetNpgsqlTransaction(connectionString) != null);
        }
        public bool MySqlTransactionExists(string connectionString)
        {
            return (GetMySqlTransaction(connectionString) != null);
        }
        public void AddConnection(string connectionString, System.Data.SqlClient.SqlConnection sqlConnection, System.Data.SqlClient.SqlTransaction sqlTransaction)
        {
            ConnectionInstance connectionInstance = new ConnectionInstance();
            connectionInstance.ConnectionString = connectionString.ToLower().Trim();
            connectionInstance.SqlConnection = sqlConnection;
            connectionInstance.SqlTransaction = sqlTransaction;
            connectionInstance.IsSqlServer = true;
            this.DBConnections.Add(connectionInstance);
            TransactionStarted = true;
            TransactionStartedUTCDateTime = DateTime.UtcNow;
        }
        public void AddConnection(string connectionString, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            ConnectionInstance connectionInstance = new ConnectionInstance();
            connectionInstance.ConnectionString = connectionString.ToLower().Trim();
            connectionInstance.NpgsqlConnection = connection;
            connectionInstance.NpgsqlTransaction = transaction;
            connectionInstance.IsPostgres = true;
            this.DBConnections.Add(connectionInstance);
            TransactionStarted = true;
        }

        public void AddConnection(string connectionString, MySqlConnection connection, MySqlTransaction transaction)
        {
            ConnectionInstance connectionInstance = new ConnectionInstance();
            connectionInstance.ConnectionString = connectionString.ToLower().Trim();
            connectionInstance.MySqlConnection = connection;
            connectionInstance.MySqlTransaction = transaction;
            connectionInstance.IsMySql = true;
            this.DBConnections.Add(connectionInstance);
            TransactionStarted = true;
        }

        public class ConnectionInstance
        {
            public System.Data.SqlClient.SqlTransaction SqlTransaction = null;
            public System.Data.SqlClient.SqlConnection SqlConnection = null;
            public NpgsqlTransaction NpgsqlTransaction = null;
            public NpgsqlConnection NpgsqlConnection = null;
            public MySqlTransaction MySqlTransaction = null;
            public MySqlConnection MySqlConnection = null;
            public Boolean IsSqlServer = false;
            public Boolean IsPostgres = false;
            public Boolean IsMySql = false;
            public string ConnectionString = null;
        }
    }
}