using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanerAPI.Data;
using PlanerAPI.Models;

namespace PlanerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;
    public TasksController(AppDbContext db) { _db = db; }

    // GET /api/tasks  — pobierz wszystkie zadania
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int userId,
        [FromQuery] string? status, [FromQuery] string? category)
    {
        var query = _db.Tasks.Where(t => t.UserId == userId);
        if (!string.IsNullOrEmpty(status))   query = query.Where(t => t.Status == status);
        if (!string.IsNullOrEmpty(category)) query = query.Where(t => t.Category == category);
        return Ok(await query.ToListAsync());
    }

    // GET /api/tasks/5  — pobierz jedno zadanie
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        return task == null ? NotFound() : Ok(task);
    }

    // POST /api/tasks  — dodaj zadanie
    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title))
            return BadRequest("Tytuł jest wymagany");
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = task.Id }, task);
    }

    // PUT /api/tasks/5  — edytuj zadanie
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskItem updated)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        task.Title       = updated.Title;
        task.Description = updated.Description;
        task.Status      = updated.Status;
        task.Priority    = updated.Priority;
        task.Category    = updated.Category;
        task.DueDate     = updated.DueDate;
        await _db.SaveChangesAsync();
        return Ok(task);
    }

    // DELETE /api/tasks/5  — usuń zadanie
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}