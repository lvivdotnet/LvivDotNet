using System.Data;

namespace LvivDotNet.Application.Interfaces
{
    /// <summary>
    /// Database connection factory.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Gets new database connection.
        /// </summary>
        /// <returns> Returns database connection. </returns>
        IDbConnection Connection { get; }
    }
}
