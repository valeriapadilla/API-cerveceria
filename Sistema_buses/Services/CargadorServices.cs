using Sistema_buses.Helpers;
using Sistema_buses.Interfaces;
using Sistema_buses.Models;

namespace Sistema_buses.Services
{
    public class CargadorServices
    {
        private readonly ICargadorRepository _cargadorRepository;

        public CargadorServices(ICargadorRepository cargadorRepository)
        {
            _cargadorRepository = cargadorRepository;
        }

        public async Task<IEnumerable<Cargador>> GetAllAsync()
        {
            return await _cargadorRepository.GetAllAsync();
        }
        public async Task<Cargador> GetByIdAsync(int cargador_id)
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

            var cargadorExistente = await _cargadorRepository.GetByNameAsync(unCargador.Marca);

            if (cargadorExistente.Id != 0)
                throw new AppValidationException($"Ya existe un cargador con esa marca {unCargador.Marca}");
            try
            {
                bool resultadoAccion = await _cargadorRepository.CreateAsync(unCargador);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cargadorExistente = await _cargadorRepository.GetByNameAsync(unCargador.Marca!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return cargadorExistente;
        }

        public async Task<Cargador> UpdateAsync(int cargador_id, Cargador uncargador)
        {
            //Validamos que los parametros sean consistentes
            if (cargador_id != uncargador.Id)
                throw new AppValidationException($"Inconsistencia en el Id del cargador a actualizar. Verifica argumentos");

            //Validamos que el cargador tenga su marca
            if (uncargador.Marca.Length == 0)
                throw new AppValidationException($"No se puede actualizar el cargador {uncargador.Id} por tener nombre nulo");

            //Validamos que el nuevo cargador no exista previamente con otro Id
            var cargadorExistente = await _cargadorRepository.GetByNameAsync(uncargador.Marca);

            if (cargadorExistente.Id != 0)
                throw new AppValidationException($"Ya existe un cargador con la marca {uncargador.Marca}");

            // validamos que el cargador a actualizar si exista con ese Id
            cargadorExistente = await _cargadorRepository.GetByIdAsync(uncargador.Id);

            if (cargadorExistente.Id == 0)
                throw new AppValidationException($"No existe un cargador con el Id {uncargador.Id} que se pueda actualizar");

            try
            {
                bool resultadoAccion = await _cargadorRepository.UpdateAsync(uncargador);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                cargadorExistente = await _cargadorRepository.GetByNameAsync(uncargador.Marca!);
            }
            catch (DbOperationException error)
            {
                throw error;
            }

            return cargadorExistente;
        }

        public async Task DeleteAsync(int cargador_id)
        {
            // validamos que el cargador a eliminar si exista con ese Id
            var cargadorexistente = await _cargadorRepository.GetByIdAsync(cargador_id);

            if (cargadorexistente.Id == 0)
                throw new AppValidationException($"No existe un cargador con el Id {cargador_id} que se pueda eliminar");

            // Validamos que el cargador no este cargando un bus.
            var busasociado = await _cargadorRepository.GetAssociatedBusAsync(cargador_id);

            if (busasociado > 0)
                throw new AppValidationException($"Un bus esta utilizando el cargador {cargadorexistente.Id}. No se puede eliminar");

            //Si existe y no tiene cervezas asociadas, se puede eliminar
            try
            {
                bool resultadoAccion = await _cargadorRepository.DeleteAsync(cargadorexistente);

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