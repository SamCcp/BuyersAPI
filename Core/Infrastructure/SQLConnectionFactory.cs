using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure
{
  public class SQLConnectionFactory : IDBConnectionFactory
  {
    private readonly string _cadenaConexion;

    public SQLConnectionFactory(string cadenaConexion)
    {
      _cadenaConexion = cadenaConexion;
    }
    public async Task<IDbConnection> CreateConnection()
    {
      var cnx = new SqlConnection(_cadenaConexion);
      await cnx.OpenAsync();
      return cnx;
    }
  }
}
