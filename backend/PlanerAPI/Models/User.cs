namespace PlanerAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Powiązanie: użytkownik ma wiele zadań
    public List<TaskItem> Tasks { get; set; } = new();
}