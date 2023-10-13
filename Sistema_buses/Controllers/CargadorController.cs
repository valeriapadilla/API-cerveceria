using Microsoft.AspNetCore.Mvc;
using Sistema_buses.Helpers;
using Sistema_buses.Models;
using Sistema_buses.Services;

namespace Sistema_buses.Controllers{



    [Route("api/[controller]")]
    [ApiController]
    public class CargadorController : Controller
    {
        private readonly CargadorServices _cargadorService;

        public CargadorController(CargadorServices cargadorServices)
        {
            _cargadorService = cargadorServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var loscargadores = await _cargadorService.GetAllAsync();

            return Ok(loscargadores);
        }

        [HttpGet("{cargador_id:int}")]
        public async Task<IActionResult> GetDetailsByIdAsync(int cargador_id)
        {
            try
            {
                var uncargador = await _cargadorService.GetByIdAsync(cargador_id);
                return Ok(uncargador);
            }
            catch (AppValidationException error)
            {
                return NotFound(error.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(Cargador uncargador)
        {
            try
            {
                var cargadorcreado = await _cargadorService.CreateAsync(uncargador);

                return Ok(cargadorcreado);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpPut("{cargador_id:int}")]
        public async Task<IActionResult> UpdateAsync(int cargador_id, Cargador uncargador)
        {
            try
            {
                var cargadoractualizado = await _cargadorService.UpdateAsync(cargador_id, uncargador);

                return Ok(cargadoractualizado);

            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }

        [HttpDelete("{cargador_id:int}")]
        public async Task<IActionResult> DeleteAsync(int cargador_id)
        {
            try
            {
                await _cargadorService.DeleteAsync(cargador_id);

                return Ok($"Cargador {cargador_id} fue eliminado");

            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }
    }
}

