using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMOP___backend.Context;
using SIMOP___backend.Modelos;

namespace SIMOP___backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HitosController : ControllerBase
    {
        private readonly SIMOPContext _context;

        public HitosController(SIMOPContext context)
        {
            _context = context;
        }

        // GET: api/Hitos
        [HttpGet]
        public async Task<IActionResult> GetHitos()
        {
            var hitos = await _context.phi_proyectoHitos.OrderByDescending(h => h.phi_codigo).ToListAsync();

            return Ok(new
            {
                success = true,
                data = hitos,
                mensaje = "Hitos obtenidos exitosamente"
            });
        }

        // GET: api/Hitos/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHito(int id)
        {
            var hito = await _context.phi_proyectoHitos.FindAsync(id);

            if (hito == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Hito con ID {id} no encontrado"
                });
            }

            return Ok(new
            {
                success = true,
                data = hito,
                mensaje = "Hito obtenido exitosamente"
            });
        }

        // POST: api/Hitos
        [HttpPost]
        public async Task<IActionResult> PostHito(phi_proyectoHito hito)
        {
            try
            {
                _context.phi_proyectoHitos.Add(hito);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = hito,
                    mensaje = "Hito creado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al crear el hito",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Hitos/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutHito(int id, phi_proyectoHito hito)
        {
            if (id != hito.phi_codigo)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "El ID de la URL no coincide con el del cuerpo"
                });
            }

            var hitoDb = await _context.phi_proyectoHitos.FindAsync(id);

            if (hitoDb == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Hito con ID {id} no encontrado"
                });
            }

            hitoDb.phi_nombre = hito.phi_nombre;
            hitoDb.phi_fechaInicio = hito.phi_fechaInicio;
            hitoDb.phi_fechaFin = hito.phi_fechaFin;
            hitoDb.phi_codproyecto = hito.phi_codproyecto;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = hitoDb,
                    mensaje = "Hito actualizado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    mensaje = "Error al actualizar el hito",
                    error = ex.Message
                });
            }
        }


        // PATCH: api/Hitos/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchHito(int id, phi_proyectoHito hitoActualizado)
        {
            var hito = await _context.phi_proyectoHitos.FindAsync(id);

            if (hito == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Hito con ID {id} no encontrado"
                });
            }

            if (!string.IsNullOrWhiteSpace(hitoActualizado.phi_nombre))
                hito.phi_nombre = hitoActualizado.phi_nombre;

            if (hitoActualizado.phi_fechaInicio.HasValue)
                hito.phi_fechaInicio = hitoActualizado.phi_fechaInicio;

            if (hitoActualizado.phi_fechaFin.HasValue)
                hito.phi_fechaFin = hitoActualizado.phi_fechaFin;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = hito,
                    mensaje = "Hito actualizado parcialmente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar el hito",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Hitos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHito(int id)
        {
            var hito = await _context.phi_proyectoHitos.FindAsync(id);

            if (hito == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Hito con ID {id} no encontrado"
                });
            }

            // Verificar dependencias en tareas
            var tareasAsociadas = await _context.pta_proyectoTareas.Where(t => t.pta_codHito == id).Select(t => new { t.pta_codigo, t.pta_Descripcion }).ToListAsync();

            if (tareasAsociadas.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    data = new
                    {
                        dependencias = tareasAsociadas
                    },
                    mensaje = "No se puede eliminar el hito porque contiene tareas asociadas"
                });
            }

            try
            {
                _context.phi_proyectoHitos.Remove(hito);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    mensaje = "Hito eliminado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al eliminar el hito",
                    error = ex.Message
                });
            }
        }


        //private bool HitoExists(int id)
        //{
        //    return _context.phi_proyectoHitos.Any(e => e.phi_codigo == id);
        //}
    }
}
