using Newtonsoft.Json;
using PlanerDesktop.Models;
using System.Text;

namespace PlanerDesktop.Services;

public class ApiService
{
    private readonly HttpClient _http = new();
    private const string BaseUrl = "http://localhost:5000/api";

    // ── Użytkownicy ──────────────────────────────────────────
    public async Task<User?> LoginAsync(string email, string password)
    {
        var body = JsonConvert.SerializeObject(new { email, password });
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"{BaseUrl}/users/login", content);
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<User>(json);
    }

    // ── Zadania ───────────────────────────────────────────────
    public async Task<List<TaskItem>> GetTasksAsync(int userId,
        string? status = null, string? category = null)
    {
        var url = $"{BaseUrl}/tasks?userId={userId}";
        if (!string.IsNullOrEmpty(status)) url += $"&status={status}";
        if (!string.IsNullOrEmpty(category)) url += $"&category={category}";
        var json = await _http.GetStringAsync(url);
        return JsonConvert.DeserializeObject<List<TaskItem>>(json) ?? new();
    }

    public async Task<TaskItem?> CreateTaskAsync(TaskItem task)
    {
        var body = JsonConvert.SerializeObject(task);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync($"{BaseUrl}/tasks", content);
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TaskItem>(json);
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskItem task)
    {
        var body = JsonConvert.SerializeObject(task);
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _http.PutAsync($"{BaseUrl}/tasks/{id}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var response = await _http.DeleteAsync($"{BaseUrl}/tasks/{id}");
        return response.IsSuccessStatusCode;
    }
}