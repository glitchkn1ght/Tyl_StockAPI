using System.Data;

namespace TradesProcessor.Interfaces
{
    public interface IPollyConnectionFactory
    {
        IDbConnection CreateOpenConnection();
    }
}
