using System.Net.Http.Json;
using SIMOP_frontend.Models;

namespace SIMOP_frontend.Services
{
    public class HitoService : IHitoService
    {
        private readonly HttpClient _http;

        public HitoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<HitoDto>> GetHitos()
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<List<HitoDto>>>("api/Hitos");
            return response?.data ?? new List<HitoDto>();
        }

        public async Task<HitoDto> GetHito(int id)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<HitoDto>>($"api/Hitos/{id}");
            return response?.data;
        }

        public async Task<(bool Exito, string Mensaje)> CreateHito(HitoDto dto)
        {
            var result = await _http.PostAsJsonAsync("api/Hitos", dto);
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<HitoDto>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }

        public async Task<(bool Exito, string Mensaje)> UpdateHito(int id, HitoDto dto)
        {
            var result = await _http.PutAsJsonAsync($"api/Hitos/{id}", dto);
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<HitoDto>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }

        public async Task<(bool Exito, string Mensaje)> DeleteHito(int id)
        {
            var result = await _http.DeleteAsync($"api/Hitos/{id}");
            var json = await result.Content.ReadFromJsonAsync<ApiResponse<object>>();

            return (result.IsSuccessStatusCode, json?.mensaje ?? "Error desconocido");
        }
    }

    public interface IHitoService
    {
        Task<List<HitoDto>> GetHitos();
        Task<HitoDto> GetHito(int id);
        Task<(bool Exito, string Mensaje)> CreateHito(HitoDto dto);
        Task<(bool Exito, string Mensaje)> UpdateHito(int id, HitoDto dto);
        Task<(bool Exito, string Mensaje)> DeleteHito(int id);
    }
}
