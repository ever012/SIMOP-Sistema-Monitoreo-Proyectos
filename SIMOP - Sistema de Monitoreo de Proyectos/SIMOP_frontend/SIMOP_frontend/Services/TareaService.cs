using System.Net.Http.Json;
using SIMOP_frontend.Models;

namespace SIMOP_frontend.Services
{
    public class TareaService : ITareaService
    {
        private readonly HttpClient _http;

        public TareaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<TareaDto>> GetTareas()
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<TareaDto>>>("api/Tareas");
            return response?.data ?? new List<TareaDto>();
        }

        public async Task<TareaDto> GetTarea(int id)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<TareaDto>>($"api/Tareas/{id}");
            return response?.data;
        }

        public async Task<(bool Exito, string Mensaje)> CreateTarea(TareaDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Tareas", dto);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadFromJsonAsync<ApiResponse<TareaDto>>();
                    return (true, json?.mensaje ?? "Tarea creada exitosamente");
                }
                else
                {
                    // Intentar leer el error como JSON
                    try
                    {
                        var errorJson = await response.Content.ReadFromJsonAsync<ApiResponse<TareaDto>>();
                        return (false, errorJson?.mensaje ?? "Error desconocido");
                    }
                    catch
                    {
                        // Si no es JSON, leer como texto plano
                        var errorText = await response.Content.ReadAsStringAsync();
                        return (false, $"Error del servidor: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        public async Task<(bool Exito, string Mensaje)> UpdateTarea(int id, TareaDto dto)
        {
            var result = await _http.PutAsJsonAsync($"api/Tareas/{id}", dto);
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<TareaDto>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }

        public async Task<(bool Exito, string Mensaje)> DeleteTarea(int id)
        {
            var result = await _http.DeleteAsync($"api/Tareas/{id}");
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<object>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }
    }

    public interface ITareaService
    {
        Task<List<TareaDto>> GetTareas();
        Task<TareaDto> GetTarea(int id);
        Task<(bool Exito, string Mensaje)> CreateTarea(TareaDto dto);
        Task<(bool Exito, string Mensaje)> UpdateTarea(int id, TareaDto dto);
        Task<(bool Exito, string Mensaje)> DeleteTarea(int id);
    }
}
