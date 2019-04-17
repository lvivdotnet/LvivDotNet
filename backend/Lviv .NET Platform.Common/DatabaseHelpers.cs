using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace Lviv_.NET_Platform.Common
{
    public class DatabaseHelpers
    {
        public static Task<int> GetLastIdentity(IDbConnection connection, IDbTransaction transaction) => connection.QuerySingleAsync<int>("select @@identity as 'identity'", transaction: transaction);
    }
}