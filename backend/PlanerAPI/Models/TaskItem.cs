namespace PlanerAPI.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Nowe"; // Nowe / W trakcie / Wykonane
    public string Priority { get; set; } = "Średni"; // Niski / Średni / Wysoki
    public string Category { get; set; } = "";
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Powiązanie z użytkownikiem
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}