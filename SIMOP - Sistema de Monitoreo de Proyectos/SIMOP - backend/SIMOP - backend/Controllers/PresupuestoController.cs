using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMOP___backend.Context;
using SIMOP___backend.Modelos;

namespace SIMOP___backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestoController : ControllerBase
    {
        private readonly SIMOPContext _context;

        public PresupuestoController(SIMOPContext context)
        {
            _context = context;
        }

        // GET: api/Presupuesto
        [HttpGet]
        public async Task<IActionResult> GetPresupuestos()
        {
            var lista = await _context.ppre_proyectoPresupuestos.OrderByDescending(p => p.ppre_codigo).ToListAsync();

            return Ok(new
            {
                success = true,
                data = lista,
                mensaje = "Registros de presupuesto obtenidos exitosamente"
            });
        }

        // GET: api/Presupuesto/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPresupuesto(int id)
        {
            var item = await _context.ppre_proyectoPresupuestos.FindAsync(id);

            if (item == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Registro de presupuesto con ID {id} no encontrado"
                });
            }

            return Ok(new
            {
                success = true,
                data = item,
                mensaje = "Registro obtenido exitosamente"
            });
        }


        // GET: api/Presupuesto/proyecto/5           //buscar por proyecto
        [HttpGet("proyecto/{proyectoId:int}")]
        public async Task<IActionResult> GetPresupuestosPorProyecto(int proyectoId)
        {
            var movimientos = await _context.ppre_proyectoPresupuestos.Where(p => p.ppre_codproyecto == proyectoId).OrderByDescending(p => p.ppre_fecha).ToListAsync();

            return Ok(new
            {
                success = true,
                data = movimientos,
                mensaje = "Movimientos de presupuesto obtenidos exitosamente"
            });
        }







        // POST: api/Presupuesto
        [HttpPost]
        public async Task<IActionResult> PostPresupuesto(ppre_proyectoPresupuesto presupuesto)
        {
            try
            {
                _context.ppre_proyectoPresupuestos.Add(presupuesto);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = presupuesto,
                    mensaje = "Registro de presupuesto creado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al crear el registro de presupuesto",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Presupuesto/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutPresupuesto(int id, ppre_proyectoPresupuesto presupuesto)
        {
            if (id != presupuesto.ppre_codigo)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "El ID de la URL no coincide con el del cuerpo"
                });
            }

            _context.Entry(presupuesto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    data = presupuesto,
                    mensaje = "Registro actualizado exitosamente"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresupuestoExists(id))
                {
                    return NotFound(new
                    {
                        success = false,
                        mensaje = $"Registro con ID {id} no encontrado"
                    });
                }

                return StatusCode(500, new
                {
                    success = false,
                    mensaje = "Error de concurrencia al actualizar el registro"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar el registro de presupuesto",
                    error = ex.Message
                });
            }
        }

        // PATCH: api/Presupuesto/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchPresupuesto(int id, ppre_proyectoPresupuesto actualizado)
        {
            var item = await _context.ppre_proyectoPresupuestos.FindAsync(id);

            if (item == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Registro de presupuesto con ID {id} no encontrado"
                });
            }

            // Actualizar solo campos no nulos
            if (actualizado.ppre_monto != 0)
                item.ppre_monto = actualizado.ppre_monto;

            if (!string.IsNullOrWhiteSpace(actualizado.ppre_descripcion))
                item.ppre_descripcion = actualizado.ppre_descripcion;

            if (actualizado.ppre_fecha.HasValue)
                item.ppre_fecha = actualizado.ppre_fecha;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    data = item,
                    mensaje = "Registro de presupuesto actualizado parcialmente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Presupuesto/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePresupuesto(int id)
        {
            var item = await _context.ppre_proyectoPresupuestos.FindAsync(id);

            if (item == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Registro de presupuesto con ID {id} no encontrado"
                });
            }

            try
            {
                _context.ppre_proyectoPresupuestos.Remove(item);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    mensaje = "Registro de presupuesto eliminado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al eliminar el registro de presupuesto",
                    error = ex.Message
                });
            }
        }

        private bool PresupuestoExists(int id)
        {
            return _context.ppre_proyectoPresupuestos.Any(e => e.ppre_codigo == id);
        }
    }
}
