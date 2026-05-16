using Newtonsoft.Json;
using PlanerMobile.Models;
using System.Text;

namespace PlanerMobile.Services;

public class ApiService
{
    private readonly HttpClient _http = new();
    private const string BaseUrl = "http://localhost:5000/api";

    public async Task<User?> LoginAsync(string email, string password)
    {
        var body = JsonConvert.SerializeObject(new { email, password });
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"{BaseUrl}/users/login", content);
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<User>(json);
    }

    public async Task<List<TaskItem>> GetTasksAsync(int userId, string? status = null)
    {
        var url = $"{BaseUrl}/tasks?userId={userId}";
        if (!string.IsNullOrEmpty(status)) url += $"&status={status}";
        var json = await _http.GetStringAsync(url);
        return JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new();
    }

    public async Task<bool> CreateTaskAsync(TaskItem task)
    {
        var body = JsonConvert.SerializeObject(task);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"{BaseUrl}/tasks", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> MarkAsDoneAsync(int id, TaskItem task)
    {
        task.Status = "Wykonane";
        var body = JsonConvert.SerializeObject(task);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _http.PutAsync($"{BaseUrl}/tasks/{id}", content);
        return response.IsSuccessStatusCode;
    }
}