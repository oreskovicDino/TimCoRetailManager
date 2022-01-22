namespace TRMDataManager.Library.Internal.DataAcsess
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    internal class SqlDataAccess : IDisposable
    {
        private IDbConnection _connection;
        private IDbTransaction transaction;
        private bool isTransactionClosed = false;

        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        // Open connection/start tranasction method.
        public void StartTransaction(string connnectionStringName)
        {
            string connectionString = GetConnectionString(connnectionStringName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();

            transaction = _connection.BeginTransaction();
            isTransactionClosed = false;
        }

        // Load using the transaction.
        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction).ToList();
            return rows;
        }

        // Save using the transaction.
        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            _connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
        }
        // Close connection/stop tranasction method.
        public void CommitTransaction()
        {
            transaction?.Commit();
            _connection?.Close();
            isTransactionClosed = true;
        }

        public void RollbackTransaction()
        {
            transaction?.Rollback();
            isTransactionClosed = true;

        }

        // Dispose.
        public void Dispose()
        {
            if (!isTransactionClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    //TODO: log this issue.
                }
            }

            transaction = null;
            _connection = null;
        }
    }
}
