using Sistema_buses.Models;

namespace Sistema_buses.Interfaces
{
    public interface ICargadorRepository
    {
        public Task<IEnumerable<Cargador>> GetAllAsync();
        public Task<Cargador_utilizado> GetByIdAsync(int cargador_id);
        public Task<bool> CreateAsync(Cargador unCargador);
        public Task<bool> UpdateAsync(Cargador unCargador);
        public Task<bool> DeleteAsync(Cargador unCargador);
    }
}
