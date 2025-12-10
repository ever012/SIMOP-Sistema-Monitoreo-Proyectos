using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMOP___backend.Context;
using SIMOP___backend.Modelos;

namespace SIMOP___backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly SIMOPContext _context;

        public TareasController(SIMOPContext context)
        {
            _context = context;
        }

        // GET: api/Tareas
        [HttpGet]
        public async Task<IActionResult> GetTareas()
        {
            var tareas = await _context.pta_proyectoTareas.OrderByDescending(t => t.pta_codigo).ToListAsync();

            return Ok(new
            {
                success = true,
                data = tareas,
                mensaje = "Tareas obtenidas exitosamente"
            });
        }

        // GET: api/Tareas/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTarea(int id)
        {
            var tarea = await _context.pta_proyectoTareas.FindAsync(id);

            if (tarea == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Tarea con ID {id} no encontrada"
                });
            }

            return Ok(new
            {
                success = true,
                data = tarea,
                mensaje = "Tarea obtenida exitosamente"
            });
        }

        // POST: api/Tareas
        [HttpPost]
        public async Task<IActionResult> PostTarea(pta_proyectoTarea tarea)
        {
            try
            {
                var proyecto = await _context.pro_Proyectos.FindAsync(tarea.pta_codproyecto);
                if (proyecto == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        mensaje = "El proyecto asociado no existe"
                    });
                }

                // Validar hito (si viene)
                if (tarea.pta_codHito.HasValue)
                {
                    var hito = await _context.phi_proyectoHitos.FindAsync(tarea.pta_codHito.Value);
                    if (hito == null)
                    {
                        return BadRequest(new
                        {
                            success = false,
                            mensaje = "El hito asociado no existe"
                        });
                    }
                }

                _context.pta_proyectoTareas.Add(tarea);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        tarea.pta_codigo,
                        tarea.pta_codproyecto,
                        tarea.pta_codHito,
                        tarea.pta_Descripcion,
                        tarea.pta_Estado,
                        tarea.pta_FechaInicio,
                        tarea.pta_FechaFin
                    },
                    mensaje = "Tarea creada exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al crear la tarea",
                    error = ex.Message
                });
            }
        }
        // PUT: api/Tareas/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutTarea(int id, pta_proyectoTarea tarea)
        {
            if (id != tarea.pta_codigo)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "El ID de la URL no coincide con el del cuerpo"
                });
            }

            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = tarea,
                    mensaje = "Tarea actualizada exitosamente"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound(new
                    {
                        success = false,
                        mensaje = $"Tarea con ID {id} no encontrada"
                    });
                }

                return StatusCode(500, new
                {
                    success = false,
                    mensaje = "Error de concurrencia al actualizar la tarea"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar la tarea",
                    error = ex.Message
                });
            }
        }

        // PATCH: api/Tareas/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchTarea(int id, pta_proyectoTarea actualizado)
        {
            var tarea = await _context.pta_proyectoTareas.FindAsync(id);

            if (tarea == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Tarea con ID {id} no encontrada"
                });
            }

            // Actualizar campos no nulos
            if (!string.IsNullOrWhiteSpace(actualizado.pta_Descripcion))
                tarea.pta_Descripcion = actualizado.pta_Descripcion;

            if (!string.IsNullOrWhiteSpace(actualizado.pta_Estado))
                tarea.pta_Estado = actualizado.pta_Estado;

            if (actualizado.pta_FechaInicio.HasValue)
                tarea.pta_FechaInicio = actualizado.pta_FechaInicio;

            if (actualizado.pta_FechaFin.HasValue)
                tarea.pta_FechaFin = actualizado.pta_FechaFin;

            if (actualizado.pta_codHito.HasValue)
                tarea.pta_codHito = actualizado.pta_codHito;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = tarea,
                    mensaje = "Tarea actualizada parcialmente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar la tarea",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Tareas/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var tarea = await _context.pta_proyectoTareas.FindAsync(id);

            if (tarea == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Tarea con ID {id} no encontrada"
                });
            }

            try
            {
                _context.pta_proyectoTareas.Remove(tarea);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    mensaje = "Tarea eliminada exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al eliminar la tarea",
                    error = ex.Message
                });
            }
        }

        private bool TareaExists(int id)
        {
            return _context.pta_proyectoTareas.Any(e => e.pta_codigo == id);
        }
    }
}
