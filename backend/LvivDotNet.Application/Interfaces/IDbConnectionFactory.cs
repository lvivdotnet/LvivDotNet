using System.Data;

namespace LvivDotNet.Application.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
