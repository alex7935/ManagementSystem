using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClientApp.Models;

namespace ClientApp.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiService()
    {
        _httpClient = new HttpClient();
        // Android-емулятор бачить локальний ПК за адресою 10.0.2.2, а Windows — через localhost
        _baseUrl = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5000"
            : "http://localhost:5000";
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/auth/login", new { Email = email, Password = password });
            if (!response.IsSuccessStatusCode) return false;

            var data = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (data != null && !string.IsNullOrEmpty(data.Token))
            {
                Preferences.Set("JwtToken", data.Token);
                Preferences.Set("TeacherName", data.Name);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private void AttachToken()
    {
        var token = Preferences.Get("JwtToken", string.Empty);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<CourseItem>> GetCoursesAsync()
    {
        AttachToken();
        try
        {
            var courses = await _httpClient.GetFromJsonAsync<List<CourseItem>>($"{_baseUrl}/api/courses");
            return courses ?? new List<CourseItem>();
        }
        catch
        {
            return new List<CourseItem>();
        }
    }

    public async Task<bool> CreateProposalAsync(string courseName, string proposalType)
    {
        AttachToken();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/proposals", new
            {
                CourseName = courseName,
                ProposalType = proposalType
            });
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}