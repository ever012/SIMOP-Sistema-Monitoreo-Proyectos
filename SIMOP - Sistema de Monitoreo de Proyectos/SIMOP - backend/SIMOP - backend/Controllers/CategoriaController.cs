using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMOP___backend.Context;
using SIMOP___backend.Modelos;

namespace SIMOP___backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly SIMOPContext _context;

        public CategoriasController(SIMOPContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _context.cat_Categorias.OrderByDescending(c => c.cat_codigo).ToListAsync();

            return Ok(new
            {
                success = true,
                data = categorias,
                mensaje = "Categorías obtenidas exitosamente"
            });
        }

        // GET: api/Categorias/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoria(int id)
        {
            var categoria = await _context.cat_Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Categoría con ID {id} no encontrada"
                });
            }

            return Ok(new
            {
                success = true,
                data = categoria,
                mensaje = "Categoría obtenida exitosamente"
            });
        }

        // POST: api/Categorias
        [HttpPost]
        public async Task<IActionResult> PostCategoria(cat_Categoria categoria)
        {
            try
            {
                _context.cat_Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = categoria,
                    mensaje = "Categoría creada exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al crear la categoría",
                    error = ex.Message
                });
            }
        }

        // PUT: api/Categorias/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutCategoria(int id, cat_Categoria categoria)
        {
            if (id != categoria.cat_codigo)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "El ID de la URL no coincide con el del cuerpo"
                });
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = categoria,
                    mensaje = "Categoría actualizada exitosamente"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound(new
                    {
                        success = false,
                        mensaje = $"Categoría con ID {id} no encontrada"
                    });
                }

                return StatusCode(500, new
                {
                    success = false,
                    mensaje = "Error de concurrencia al actualizar"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar la categoría",
                    error = ex.Message
                });
            }
        }

        // PATCH: api/Categorias/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchCategoria(int id, cat_Categoria categoriaActualizada)
        {
            var categoria = await _context.cat_Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Categoría con ID {id} no encontrada"
                });
            }

            // Actualizar solo los campos que no sean nulos
            if (!string.IsNullOrEmpty(categoriaActualizada.cat_nombre))
                categoria.cat_nombre = categoriaActualizada.cat_nombre;


            try
            {
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = categoria,
                    mensaje = "Categoría actualizada parcialmente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error al actualizar la categoría",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.cat_Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound(new
                {
                    success = false,
                    mensaje = $"Categoría con ID {id} no encontrada"
                });
            }

            var proyectosAsociados = await _context.pro_Proyectos.Where(p => p.pro_codCategoria == id).Select(p => new { p.pro_codigo, p.pro_nombre }).ToListAsync();

            if (proyectosAsociados.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "No se puede eliminar la categoría porque está siendo utilizada por uno o más proyectos.",
                    proyectos = proyectosAsociados
                });
            }

            try
            {
                // Eliminar si no tiene dependencias
                _context.cat_Categorias.Remove(categoria);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    mensaje = "Categoría eliminada exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "Error inesperado al eliminar la categoría",
                    error = ex.Message
                });
            }
        }



        private bool CategoriaExists(int id)
        {
            return _context.cat_Categorias.Any(e => e.cat_codigo == id);
        }
    }
}