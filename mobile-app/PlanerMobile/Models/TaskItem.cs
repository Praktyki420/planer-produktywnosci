namespace PlanerMobile.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Nowe";
    public string Priority { get; set; } = "Średni";
    public string Category { get; set; } = "";
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
}