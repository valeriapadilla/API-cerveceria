using System.Data;
using Npgsql;

namespace Sistema_buses.DbContexts
{
    public class PgsqlDbContext
    {
        private readonly string cadenaConexion;
        public PgsqlDbContext(IConfiguration unaConfiguracion)
        {
            cadenaConexion = unaConfiguracion.GetConnectionString("PgSql")!;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(cadenaConexion);
        }
    }
}
