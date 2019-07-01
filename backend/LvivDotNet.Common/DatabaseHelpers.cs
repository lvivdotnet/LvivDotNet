using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace LvivDotNet.Common
{
    /// <summary>
    /// Database helpers.
    /// </summary>
    public static class DatabaseHelpers
    {
        /// <summary>
        /// Gets last identity generated in scope.
        /// </summary>
        /// <param name="connection"> Database connection. </param>
        /// <param name="transaction"> Database transaction. </param>
        /// <returns> Returns the last identity generated in scope. </returns>
        public static Task<int> GetLastIdentity(IDbConnection connection, IDbTransaction transaction) => connection.QuerySingleAsync<int>("select @@identity as 'identity'", transaction: transaction);
    }
}