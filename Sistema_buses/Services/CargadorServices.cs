namespace Sistema_buses.Services;
using Sistema_buses.Helpers;
using Sistema_buses.Interfaces;
using Sistema_buses.Models;

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
}