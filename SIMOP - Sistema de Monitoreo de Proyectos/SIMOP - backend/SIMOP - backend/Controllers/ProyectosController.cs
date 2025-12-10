using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMOP___backend.Context;
using SIMOP___backend.Modelos;

namespace SIMOP___backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : ControllerBase
    {
        private readonly SIMOPContext _context;

        public ProyectosController(SIMOPContext context)
        {
            _context = context;
        }

        // GET: api/Proyectos
        [HttpGet]
        public async Task<IActionResult> GetProyectos()
        {
            var proyectos = await _context.pro_Proyectos.OrderByDescending(p => p.pro_codigo).ToListAsync();

            return Ok(new
            {
                success = true,
                data = proyectos,
                mensaje = "Proyectos obtenidos exitosamente"
            });
        }

        // GET: api/Proyectos/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProyecto(int id)
        {
            var proyecto = await _context.pro_Proyectos.FindAsync(id);

            if (proyecto == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Proyecto con ID {id} no encontrado"
                });
            }

            return Ok(new
            {
                success = true,
                data = proyecto,
                mensaje = "Proyecto obtenido exitosamente"
            });
        }

        // POST: api/Proyectos
        [HttpPost]
        public async Task<IActionResult> PostProyecto(pro_Proyecto proyecto)
        {
            try
            {
                var categoria = await _context.cat_Categorias.FindAsync(proyecto.pro_codCategoria);
                if (categoria == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        mensaje = "La categoría asociada no existe"
                    });
                }

                proyecto.pro_fechaCreacion = DateTime.Now;
                proyecto.pro_fechaActualizacion = DateTime.Now;

                _context.pro_Proyectos.Add(proyecto);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = proyecto,
                    mensaje = "Proyecto creado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al crear el proyecto",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Proyectos/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutProyecto(int id, pro_Proyecto proyecto)
        {
            // Validar que el ID coincida
            if (id != proyecto.pro_codigo)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "El ID de la URL no coincide con el del cuerpo"
                });
            }

            var proyectoDb = await _context.pro_Proyectos.FindAsync(id);
            if (proyectoDb == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Proyecto con ID {id} no encontrado"
                });
            }

            proyectoDb.pro_nombre = proyecto.pro_nombre;
            proyectoDb.pro_descripcion = proyecto.pro_descripcion;
            proyectoDb.pro_codCategoria = proyecto.pro_codCategoria;
            proyectoDb.pro_ubicacion = proyecto.pro_ubicacion;
            proyectoDb.pro_presupuestoTotal = proyecto.pro_presupuestoTotal;
            proyectoDb.pro_fechaInicio = proyecto.pro_fechaInicio;
            proyectoDb.pro_fechaFinEstimada = proyecto.pro_fechaFinEstimada;
            proyectoDb.pro_fechaActualizacion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = proyectoDb,
                    mensaje = "Proyecto actualizado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar el proyecto",
                    error = ex.Message
                });
            }
        }


        // PATCH: api/Proyectos/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchProyecto(int id, pro_Proyecto actualizado)
        {
            var proyecto = await _context.pro_Proyectos.FindAsync(id);

            if (proyecto == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Proyecto con ID {id} no encontrado"
                });
            }

            // Actualizar solo campos no nulos
            if (!string.IsNullOrWhiteSpace(actualizado.pro_nombre))
                proyecto.pro_nombre = actualizado.pro_nombre;

            if (!string.IsNullOrWhiteSpace(actualizado.pro_descripcion))
                proyecto.pro_descripcion = actualizado.pro_descripcion;

            if (actualizado.pro_codCategoria != 0)
                proyecto.pro_codCategoria = actualizado.pro_codCategoria;

            if (!string.IsNullOrWhiteSpace(actualizado.pro_ubicacion))
                proyecto.pro_ubicacion = actualizado.pro_ubicacion;

            if (actualizado.pro_presupuestoTotal != 0)
                proyecto.pro_presupuestoTotal = actualizado.pro_presupuestoTotal;

            if (actualizado.pro_fechaInicio.HasValue)
                proyecto.pro_fechaInicio = actualizado.pro_fechaInicio;

            if (actualizado.pro_fechaFinEstimada.HasValue)
                proyecto.pro_fechaFinEstimada = actualizado.pro_fechaFinEstimada;

            proyecto.pro_fechaActualizacion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    success = true,
                    data = proyecto,
                    mensaje = "Proyecto actualizado parcialmente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar el proyecto",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Proyectos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            var proyecto = await _context.pro_Proyectos.FindAsync(id);

            if (proyecto == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Proyecto con ID {id} no encontrado"
                });
            }

            // Validar dependencias: tareas, hitos, presupuesto
            var dependencias = new List<string>();

            if (await _context.pta_proyectoTareas.AnyAsync(t => t.pta_codproyecto == id))
                dependencias.Add("Tareas");

            if (await _context.phi_proyectoHitos.AnyAsync(h => h.phi_codproyecto == id))
                dependencias.Add("Hitos");

            if (await _context.ppre_proyectoPresupuestos.AnyAsync(p => p.ppre_codproyecto == id))
                dependencias.Add("Presupuesto");

            if (dependencias.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "No se puede eliminar el proyecto porque tiene dependencias.",
                    detalles = dependencias
                });
            }

            try
            {
                _context.pro_Proyectos.Remove(proyecto);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    mensaje = "Proyecto eliminado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al eliminar el proyecto",
                    error = ex.Message
                });
            }
        }

        //private bool ProyectoExists(int id)
        //{
        //    return _context.pro_Proyectos.Any(e => e.pro_codigo == id);
        //}
    }
}
