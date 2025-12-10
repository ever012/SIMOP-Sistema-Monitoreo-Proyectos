using SIMOP_frontend.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace SIMOP_frontend.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient _http;

        public CategoriaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CategoriaDto>> GetCategorias()
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<CategoriaDto>>>("api/Categorias");
            return response?.data ?? new List<CategoriaDto>();
        }

        public async Task<CategoriaDto> GetCategoria(int id)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<CategoriaDto>>($"api/Categorias/{id}");
            return response.data;
        }

        public async Task<bool> CreateCategoria(CategoriaDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/Categorias", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoria(int id, CategoriaDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/Categorias/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<(bool Exito, string Mensaje)> DeleteCategoria(int id)
        {
            var response = await _http.DeleteAsync($"api/Categorias/{id}");

            var contenido = await response.Content.ReadFromJsonAsync<JsonElement>();

            if (response.IsSuccessStatusCode)
            {
                return (true, contenido.GetProperty("mensaje").GetString());
            }

            // Extraer el mensaje de error
            string mensaje = contenido.GetProperty("mensaje").GetString();

            // Si existen proyectos asociados, concatenarlos
            if (contenido.TryGetProperty("proyectos", out var projs) && projs.ValueKind == JsonValueKind.Array)
            {
                var nombres = projs.EnumerateArray()
                                   .Select(p => p.GetProperty("pro_nombre").GetString())
                                   .ToList();

                mensaje += " Proyectos asociados: " + string.Join(", ", nombres);
            }

            return (false, mensaje);
        }
    }

    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> GetCategorias();
        Task<CategoriaDto> GetCategoria(int id);
        Task<bool> CreateCategoria(CategoriaDto dto);
        Task<bool> UpdateCategoria(int id, CategoriaDto dto);
        Task<(bool Exito, string Mensaje)> DeleteCategoria(int id);
    }
}
