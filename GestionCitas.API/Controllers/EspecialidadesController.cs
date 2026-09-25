using GestionCitas.API.Dto;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly IEspecialidadRepository _especialidadRepository;

        public EspecialidadesController(IEspecialidadRepository especialidadRepository)
        {
            _especialidadRepository = especialidadRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(EspecialidadDto especialidadDto)
        {
            await _especialidadRepository.CreateAsync(new Especialidad()
            {
                Nombre = especialidadDto.Nombre,
                Descripcion = especialidadDto.Descripcion
            });

            return Ok(especialidadDto);
        }

        [HttpGet("{skip}/{take}")]
        public async Task<IActionResult> Get(int skip, int take)
        {
            var especialidades = await _especialidadRepository.GetAsync(skip, take);
            return Ok(especialidades);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var especialidad = await _especialidadRepository.GetById(id);

            if (especialidad == null)
            {
                return NotFound();
            }

            return Ok(especialidad);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _especialidadRepository.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EspecialidadDto especialidadDto)
        {
            var existingEspecialidad = await _especialidadRepository.GetById(id);
            if (existingEspecialidad == null)
            {
                return NotFound();
            }

            existingEspecialidad.Nombre = especialidadDto.Nombre;
            existingEspecialidad.Descripcion = especialidadDto.Descripcion;

            await _especialidadRepository.Update(existingEspecialidad);
            return Ok(existingEspecialidad);
        }
    }
}