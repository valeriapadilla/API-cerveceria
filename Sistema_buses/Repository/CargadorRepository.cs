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

    public async Task<Cargador_utilizado> GetByIdAsync(int cargador_id)
    {
        Cargador_utilizado unCargador = new Cargador_utilizado();

        using (var conexion = contextoDB.CreateConnection())
        {
            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@cargador_id", cargador_id,
                DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT * " +
                                  "FROM cargadores_utilizados " +
                                  "WHERE id = @cargador_id ";

            var resultado = await conexion.QueryAsync<Cargador_utilizado>(sentenciaSQL,
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
                string procedimiento = "core.p_inserta_cargador";
                var parametros = new
                {
                    p_marca = unCargador.Marca
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
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
                string procedimiento = "core.p_actualiza_cargador";
                var parametros = new
                {
                    p_id = unCargador.Id,
                    p_nombre = unCargador.Marca
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
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
                string procedimiento = "core.p_elimina_cargador";
                var parametros = new 
                { 
                    p_id = unCargador.Id
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
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