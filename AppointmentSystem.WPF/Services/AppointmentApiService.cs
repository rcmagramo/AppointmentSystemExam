using AppointmentSystem.WPF.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace AppointmentSystem.WPF.Services;

public class AppointmentApiService : IAppointmentApiService
{
    private readonly HttpClient _httpClient;

    public AppointmentApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResult<AppointmentDto>> GetAppointmentsAsync(
        string? searchTerm = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>
        {
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            queryParams.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");
        }

        var queryString = string.Join("&", queryParams);
        var response = await _httpClient.GetAsync($"/api/v1/appointments?{queryString}", cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedResult<AppointmentDto>>(json);
        return result ?? new PagedResult<AppointmentDto>();
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/v1/appointments/{id}", cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AppointmentDto>(json);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(
        CreateAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/v1/appointments", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<AppointmentDto>(responseJson);
        return result ?? throw new Exception("Failed to deserialize response");
    }

    public async Task<AppointmentDto> UpdateAppointmentAsync(
        int id,
        UpdateAppointmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"/api/v1/appointments/{id}", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<AppointmentDto>(responseJson);
        return result ?? throw new Exception("Failed to deserialize response");
    }

    public async Task DeleteAppointmentAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"/api/v1/appointments/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}