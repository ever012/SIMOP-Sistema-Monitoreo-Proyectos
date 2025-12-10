using System.Net.Http.Json;
using SIMOP_frontend.Models;
using System.Text.Json;

namespace SIMOP_frontend.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly HttpClient _http;

        public ProyectoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProyectoDto>> GetProyectos()
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<ProyectoDto>>>("api/Proyectos");
            return response?.data ?? new List<ProyectoDto>();
        }

        public async Task<ProyectoDto> GetProyecto(int id)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<ProyectoDto>>($"api/Proyectos/{id}");
            return response?.data;
        }

        public async Task<(bool Exito, string Mensaje)> CreateProyecto(ProyectoDto dto)
        {
            var result = await _http.PostAsJsonAsync("api/Proyectos", dto);
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<ProyectoDto>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }

        public async Task<(bool Exito, string Mensaje)> UpdateProyecto(int id, ProyectoDto dto)
        {
            var result = await _http.PutAsJsonAsync($"api/Proyectos/{id}", dto);
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<ProyectoDto>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }

        public async Task<(bool Exito, string Mensaje, List<string> Dependencias)> DeleteProyecto(int id)
        {
            var response = await _http.DeleteAsync($"api/Proyectos/{id}");
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            var mensaje = json.GetProperty("mensaje").GetString();
            List<string> deps = new();

            if (json.TryGetProperty("detalles", out var arr) && arr.ValueKind == JsonValueKind.Array)
            {
                deps = arr.EnumerateArray()
                          .Select(d => d.GetString())
                          .ToList();
            }

            return (response.IsSuccessStatusCode, mensaje, deps);
        }
    }

    public interface IProyectoService
    {
        Task<List<ProyectoDto>> GetProyectos();
        Task<ProyectoDto> GetProyecto(int id);
        Task<(bool Exito, string Mensaje)> CreateProyecto(ProyectoDto dto);
        Task<(bool Exito, string Mensaje)> UpdateProyecto(int id, ProyectoDto dto);
        Task<(bool Exito, string Mensaje, List<string> Dependencias)> DeleteProyecto(int id);
    }
}
