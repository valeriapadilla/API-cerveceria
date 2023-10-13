using Sistema_buses.Models;

namespace Sistema_buses.Interfaces
{
    public interface ICargadorRepository
    {
        public Task<IEnumerable<Cargador>> GetAllAsync();
        public Task<Cargador_utilizado> GetByIdCargador_utilizadoAsync(int cargador_id);
        public Task<Cargador> GetByNameAsync(string string_nombre);
        public Task<Cargador> GetByIdAsync(int cargador_id);
        public Task<int> GetAssociatedBusAsync(int cargador_id);

        public Task<bool> CreateAsync(Cargador unCargador);
        public Task<bool> UpdateAsync(Cargador unCargador);
        public Task<bool> DeleteAsync(Cargador unCargador);
    }
}
