using Sistema_buses.Interfaces;
using Sistema_buses.DbContexts;
using Sistema_buses.Helpers;
using Sistema_buses.Models;
using Dapper;
using Npgsql;
using System.Data;

namespace Sistema_buses.Repositories;

public class BusRepository : IBusRepository
{
    private readonly PgsqlDbContext contextoDB;

    public BusRepository(PgsqlDbContext unContexto)
    {
        contextoDB = unContexto;
    }
    public async Task<IEnumerable<Autobus>> GetAllAsync()
    {
        using (var conexion = contextoDB.CreateConnection())
        {
            string sentenciaSQL =   "SELECT id, ruta " +
                                    "FROM buses " +
                                    "ORDER BY id DESC ";

            var resultadoEnvasados = await conexion.QueryAsync<Autobus>(sentenciaSQL,
                new DynamicParameters());

            return resultadoEnvasados;
        }
    }

    public async Task<Autobus> GetByIdAsync(int Autobus_id)
    {
        Autobus unAutobus = new Autobus();
        using (var conexion = contextoDB.CreateConnection())
        {
            DynamicParameters parametrosSentencia = new DynamicParameters();
            parametrosSentencia.Add("@Autobus_id", Autobus_id,
                DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT * " +
                                  "FROM buses " +
                                  "WHERE id = @Autobus_id ";

            var resultado = await conexion.QueryAsync<Autobus>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Count() > 0)
                unAutobus = resultado.First();
        }
        return unAutobus;
    }

    public async Task<bool> CreateAsync(Autobus unAutobus)
    {
        bool resultadoAccion = false;
        try
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string procedimiento = "core.p_inserta_bus";
                var parametros = new
                {
                    p_ruta = unAutobus.Placa
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

    public async Task<bool> UpdateAsync(Autobus unAutobus)
    {
        bool resultadoAccion = false;

        try
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string procedimiento = "core.p_actualiza_bus";
                var parametros = new
                {
                    p_id = unAutobus.Id,
                    p_nombre = unAutobus.Placa
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

    public async Task<bool> DeleteAsync(Autobus unAutobus)
    {
        bool resultadoAccion = false;

        try
        {
            using (var conexion = contextoDB.CreateConnection())
            {
                string procedimiento = "core.p_elimina_bus";
                var parametros = new 
                { 
                    p_id = unAutobus.Id
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

    public async Task<bool> CreateAsync(Cargador_utilizado UnCargador)
    {
            
    }

    public async Task<bool> UpdateAsync(Cargador_utilizado UnCargador)
    {
        
    }
}