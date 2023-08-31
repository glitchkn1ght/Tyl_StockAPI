using CommonModels;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using TradesProcessor.Interfaces;
using TradesProcessor.Models;

namespace Trades_Receiver.DAL.Repositories
{
    public class TradesRepository : ITradesRepository
    {
        private readonly IPollyConnectionFactory ConnectionFactory;
        private readonly TradesRepositorySettings TradesRepositorySettings;

        public TradesRepository(IPollyConnectionFactory connectionFactory, IOptions<TradesRepositorySettings> tradesRepositorySettings)
        {
            ConnectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            TradesRepositorySettings = tradesRepositorySettings.Value;
        }

        public async Task<int> InsertTrade(Trade trade)
        {
            using (IDbConnection connection = ConnectionFactory.CreateOpenConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@BrokerId", trade.BrokerId);
                parameters.Add("@retVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await connection.ExecuteAsync(this.TradesRepositorySettings.InsertTradeProc, parameters, commandType: CommandType.StoredProcedure);

                var result = parameters.Get<int>("@retVal");

                return result;
            }
        }
    }
}
