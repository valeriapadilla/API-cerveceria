using Sistema_buses.Helpers;
using Sistema_buses.Interfaces;
using Sistema_buses.Models;

namespace Sistema_buses.Services
{
    public class EstiloService
    {
        private readonly ICargadorRepository _cargadorRepository;

        public EstiloService(ICargadorRepository cargadorRepository)
        {
            _cargadorRepository = cargadorRepository;
        }

        public async Task<IEnumerable<Cargador>> GetAllAsync()
        {
            return await _cargadorRepository.GetAllAsync();
        }
        public async Task<Cargador_utilizado> GetByIdAsync(int cargador_id)
        {
            //Validamos que el cargado exista y este usado
            var uncargadorusado = await _cargadorRepository.GetByIdAsync(cargador_id);

            if (uncargadorusado.Id == 0)
                throw new AppValidationException($"Cargador no usado, con el id {cargador_id}");

            return uncargadorusado;
        }

        public async Task<Cargador> CreateAsync(Cargador unCargador)
        {
            
            if (unCargador.Marca.Length == 0)
                throw new AppValidationException("No se puede insertar un cargador con valor nulo");
            

            if (cargadorExistente.Id != 0)
                throw new AppValidationException($"Ya existe un cargador  con la marca {unCargador.Marca}");

            try
            {
                bool resultadoAccion = await _estiloRepository
                    .CreateAsync(unEstilo);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                estiloExistente = await _estiloRepository
                    .GetByNameAsync(unEstilo.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return estiloExistente;
        }

        public async Task<Estilo> UpdateAsync(int estilo_id, Estilo unEstilo)
        {
            //Validamos que los parametros sean consistentes
            if (estilo_id != unEstilo.Id)
                throw new AppValidationException($"Inconsistencia en el Id del estilo a actualizar. Verifica argumentos");

            //Validamos que el estilo tenga nombre
            if (unEstilo.Nombre.Length == 0)
                throw new AppValidationException($"No se puede actualizar el estilo {unEstilo.Id} para que tenga nombre nulo");

            //Validamos que el nuevo nombre no exista previamente con otro Id
            var estiloExistente = await _estiloRepository
                .GetByNameAsync(unEstilo.Nombre!);

            if (estiloExistente.Id != 0)
                throw new AppValidationException($"Ya existe un estilo con el nombre {unEstilo.Nombre}");

            // validamos que el estilo a actualizar si exista con ese Id
            estiloExistente = await _estiloRepository
                .GetByIdAsync(unEstilo.Id);

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"No existe un estilo con el Id {unEstilo.Id} que se pueda actualizar");

            try
            {
                bool resultadoAccion = await _estiloRepository
                    .UpdateAsync(unEstilo);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                estiloExistente = await _estiloRepository
                    .GetByNameAsync(unEstilo.Nombre!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return estiloExistente;
        }

        public async Task DeleteAsync(int estilo_id)
        {
            // validamos que el estilo a eliminar si exista con ese Id
            var estiloExistente = await _estiloRepository
                .GetByIdAsync(estilo_id);

            if (estiloExistente.Id == 0)
                throw new AppValidationException($"No existe un estilo con el Id {estilo_id} que se pueda eliminar");

            // Validamos que el estilo no tenga asociadas cervezas
            var cantidadCervezasAsociadas = await _estiloRepository
                .GetTotalAssociatedBeersAsync(estiloExistente.Id);

            if (cantidadCervezasAsociadas > 0)
                throw new AppValidationException($"Existen {cantidadCervezasAsociadas} cervezas " +
                    $"asociadas al estilo {estiloExistente.Nombre}. No se puede eliminar");

            //Si existe y no tiene cervezas asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _estiloRepository
                    .DeleteAsync(estiloExistente);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");
            }
            catch (DbOperationException error)
            {
                throw error;
            }
        }
    }
}