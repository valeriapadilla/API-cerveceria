using Sistema_buses.Models;

namespace Sistema_buses.Interfaces
{
    public interface IBusRepository
    {
        public Task<IEnumerable<Autobus>> GetAllAsync();
        public Task<Autobus> GetByIdAsync(int Autobus_id);
        public Task<bool> CreateAsync(Autobus unAutobus);
        public Task<bool> UpdateAsync(Autobus unAutobus);
        public Task<bool> DeleteAsync(Autobus unAutobus);
        public Task<bool> CreateAsync(Cargador_utilizado UnCargador);
        public Task<bool> UpdateAsync(Cargador_utilizado UnCargador);
        public Task<bool> DeleteAsync(Cargador_utilizado UnCargador);
        public Task<bool> CreateAsync(Operacion unaOperacion);
        public Task<bool> UpdateAsync(Operacion unaOperacion);
        public Task<bool> DeleteAsync(Operacion unaOperacion);
    }
}
