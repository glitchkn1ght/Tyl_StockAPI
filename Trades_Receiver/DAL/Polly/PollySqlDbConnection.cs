using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using TradesProcessor.Interfaces;

namespace TradesProcessor.DAL.Polly
{
    public class PollySqlDbConnection : DbConnection
    {
        private readonly SqlConnection _underlyingConnection;
        private readonly IPollyRetryPolicy _retryPolicy;

        private string _connectionString;

        public PollySqlDbConnection(
            string connectionString,
            IPollyRetryPolicy retryPolicy)
        {
            _connectionString = connectionString;
            _retryPolicy = retryPolicy;
            _underlyingConnection = new SqlConnection(connectionString);
        }

        public override string ConnectionString
        {
            get
            {
                return _connectionString;
            }

            set
            {
                _connectionString = value;
                _underlyingConnection.ConnectionString = value;
            }
        }

        public override string Database => _underlyingConnection.Database;

        public override string DataSource => _underlyingConnection.DataSource;

        public override string ServerVersion => _underlyingConnection.ServerVersion;

        public override ConnectionState State => _underlyingConnection.State;

        public override void ChangeDatabase(string databaseName)
        {
            _underlyingConnection.ChangeDatabase(databaseName);
        }

        public override void Close()
        {
            _underlyingConnection.Close();
        }

        public override void Open()
        {
            _retryPolicy.Execute(() =>
            {
                if (_underlyingConnection.State != ConnectionState.Open)
                {
                    _underlyingConnection.Open();
                }
            });
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return _underlyingConnection.BeginTransaction(isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            return _underlyingConnection.CreateCommand();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_underlyingConnection.State == ConnectionState.Open)
                {
                    _underlyingConnection.Close();
                }

                _underlyingConnection.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}