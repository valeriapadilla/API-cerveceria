using Sistema_buses.DbContexts;
using Sistema_buses.Helpers;
using Sistema_buses.Interfaces;
using Sistema_buses.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace Sistema_buses.Repositories
{
    public class HoraRepository : IHoraRepository
    {
        private readonly PgsqlDbContext contextoDB;

        public HoraRepository(PgsqlDbContext unContexto)
        {
            contextoDB = unContexto;
        }
        
        public async Task<IEnumerable<Hora>> GetAllAsync()
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "SELECT id, eshorapico " +
                                      "FROM horas " +
                                      "ORDER BY id DESC";

                var resultadoEstilos = await conexion.QueryAsync<Hora>(sentenciaSQL,
                    new DynamicParameters());

                return resultadoEstilos;
            }
        }
        public async Task<Hora> GetByIdAsync(int hora_id)
        {
            Hora unahora = new Hora();

            using (var conexion = contextoDB.CreateConnection())
            {
                DynamicParameters parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@hora_id", hora_id,
                    DbType.Int32, ParameterDirection.Input);

                string sentenciaSQL = "SELECT id, eshorapico " +
                                      "FROM horas " +
                                      "WHERE id = @hora_id ";

                var resultado = await conexion.QueryAsync<Hora>(sentenciaSQL,
                    parametrosSentencia);

                if (resultado.Count() > 0)
                    unahora = resultado.First();
            }
            return unahora;
        }
    }
}