using System.Data;

namespace Core.Infrastructure
{
  public interface IDBConnectionFactory
  {
    Task<IDbConnection> CreateConnection();
  }
}