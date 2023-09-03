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

        public async Task InsertTrade(Trade trade)
        {
            using (IDbConnection connection = ConnectionFactory.CreateOpenConnection())
            {
                var parameters = new DynamicParameters();

                parameters.Add("@TradeId", trade.TradeId);
                parameters.Add("@BrokerId", trade.BrokerId);
                parameters.Add("@TickerSymbol", trade.TickerSymbol);
                parameters.Add("@PriceTotal", trade.PriceTotal);
                parameters.Add("@TradeCurrency", trade.TradeCurrency);
                parameters.Add("@NumberOfShares", trade.NumberOfShares);

                await connection.ExecuteAsync(this.TradesRepositorySettings.InsertProc, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
