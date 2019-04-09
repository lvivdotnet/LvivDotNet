using System.Data;

namespace Lviv_.NET_Platform.Application.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
