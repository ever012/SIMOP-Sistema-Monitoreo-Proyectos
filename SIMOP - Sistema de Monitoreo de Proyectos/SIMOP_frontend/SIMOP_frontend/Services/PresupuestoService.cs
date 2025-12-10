using System.Net.Http.Json;
using SIMOP_frontend.Models;

namespace SIMOP_frontend.Services
{
    public class PresupuestoService : IPresupuestoService
    {
        private readonly HttpClient _http;

        public PresupuestoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PresupuestoDto>> GetMovimientos()
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<PresupuestoDto>>>("api/Presupuesto");
            return response?.data ?? new List<PresupuestoDto>();
        }

        public async Task<List<PresupuestoDto>> GetMovimientosPorProyecto(int proyectoId)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<PresupuestoDto>>>($"api/Presupuesto/proyecto/{proyectoId}");
            return response?.data ?? new List<PresupuestoDto>();
        }

        public async Task<(bool Exito, string Mensaje)> CreateMovimiento(PresupuestoDto dto)
        {
            var result = await _http.PostAsJsonAsync("api/Presupuesto", dto);
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<PresupuestoDto>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }

        public async Task<(bool Exito, string Mensaje)> DeleteMovimiento(int id)
        {
            var result = await _http.DeleteAsync($"api/Presupuesto/{id}");
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<object>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }
    }

    public interface IPresupuestoService
    {
        Task<List<PresupuestoDto>> GetMovimientos();
        Task<List<PresupuestoDto>> GetMovimientosPorProyecto(int proyectoId);
        Task<(bool Exito, string Mensaje)> CreateMovimiento(PresupuestoDto dto);
        Task<(bool Exito, string Mensaje)> DeleteMovimiento(int id);
    }
}
