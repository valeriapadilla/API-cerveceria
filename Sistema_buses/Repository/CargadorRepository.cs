using Sistema_buses.Interfaces;
using Sistema_buses.DbContexts;
using Sistema_buses.Helpers;
using Sistema_buses.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace Sistema_buses.Repository;

public class CargadorRepository : ICargadorRepository
{
    private readonly PgsqlDbContext contextoDB;

    public CargadorRepository(PgsqlDbContext unContexto)
    {
        contextoDB = unContexto;
    }
    
    public async Task<IEnumerable<Cargador>> GetAllAsync()
    {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL =   "SELECT id, marca " +
                                        "FROM cargadores " +
                                        "ORDER BY id DESC ";

                var resultadoEnvasados = await conexion.QueryAsync<Cargador>(sentenciaSQL,
                    new DynamicParameters());

                return resultadoEnvasados;
            }
        
    }

    public async Task<Cargador> GetByIdAsync(int cargador_id)
    {
        Cargador unCargador = new Cargador();

        using (var conexion = contextoDB.CreateConnection())
        {
            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@cargador_id", cargador_id,
                DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, marca " +
                                  "FROM cargadores " +
                                  "WHERE id = @cargador_id ";

            var resultado = await conexion.QueryAsync<Cargador>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Count() > 0)
                unCargador = resultado.First();
        }

        return unCargador;
    }

    public async Task<bool> CreateAsync(Cargador unCargador)
    {
        bool resultadoAccion = false;
        try
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "INSERT INTO cargadores (marca) " +
                                      "VALUES (@marca)";

                int filasAfectadas = await conexion.ExecuteAsync(sentenciaSQL,unCargador);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);           
        }

        return resultadoAccion;
        
    }

    public async Task<bool> UpdateAsync(Cargador unCargador)
    {
        bool resultadoAccion = false;

        try
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "UPDATE cargadores " +
                                      "SET marca = @Marca " +
                                      "WHERE id = @Id";

                int filasAfectadas = await conexion.ExecuteAsync(sentenciaSQL, unCargador);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }

        return resultadoAccion;
    }

    public async Task<bool> DeleteAsync(Cargador unCargador)
    {
        bool resultadoAccion = false;

        try
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string sentenciaSQL = "DELETE FROM cargadores WHERE id = @Id";

                var filasAfectadas = await conexion.ExecuteAsync(sentenciaSQL, unCargador);

                if (filasAfectadas > 0)
                    resultadoAccion = true;
            }
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }

        return resultadoAccion;
    }
}