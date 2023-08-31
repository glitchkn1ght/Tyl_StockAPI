using Microsoft.Extensions.Options;
using System.Data;
using TradesProcessor.Interfaces;
using TradesProcessor.Models.Configuration;

namespace TradesProcessor.DAL.Polly
{
    public class PollySqlConnectionFactory : IPollyConnectionFactory
    {
        private SqlConnectionSettings ConnectionSettings { get; set; }
        
        private IPollyRetryPolicy RetryPolicy { get; set; }

        public PollySqlConnectionFactory(IOptions<SqlConnectionSettings> connectionSettings, IPollyRetryPolicy retryPolicy)
        {
            this.RetryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));

            this.ConnectionSettings = connectionSettings.Value ?? throw new ArgumentNullException(nameof(connectionSettings));

            if (string.IsNullOrWhiteSpace(this.ConnectionSettings.ConnectionString))
            {
                throw new ArgumentException("SqlConnectionString is null or whitespace");
            }
        }

        public IDbConnection CreateOpenConnection()
        {
            var conn = new PollySqlDbConnection(this.ConnectionSettings.ConnectionString, this.RetryPolicy);
            conn.Open();

            return conn;
        }
    }
}
