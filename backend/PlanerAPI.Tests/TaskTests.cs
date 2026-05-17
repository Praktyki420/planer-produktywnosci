using Microsoft.EntityFrameworkCore;
using PlanerAPI.Controllers;
using PlanerAPI.Data;
using PlanerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace PlanerAPI.Tests;

public class TaskTests
{
    // Pomocnicza metoda tworząca bazę danych w pamięci (nie na dysku)
    private AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // Test 1 — czy można dodać zadanie
    [Fact]
    public async Task DodanieZadania_ZwracaCreated()
    {
        var db = CreateDb();
        var controller = new TasksController(db);

        var task = new TaskItem
        {
            Title    = "Testowe zadanie",
            Status   = "Nowe",
            Priority = "Średni",
            UserId   = 1
        };

        var result = await controller.Create(task);

        Assert.IsType<CreatedAtActionResult>(result);
    }

    // Test 2 — czy zadanie bez tytułu jest odrzucane
    [Fact]
    public async Task DodanieZadaniaBezTytulu_ZwracaBadRequest()
    {
        var db = CreateDb();
        var controller = new TasksController(db);

        var task = new TaskItem { Title = "", UserId = 1 };

        var result = await controller.Create(task);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    // Test 3 — czy pobieranie zadań działa
    [Fact]
    public async Task PobieranieZadan_ZwracaListe()
    {
        var db = CreateDb();
        db.Tasks.Add(new TaskItem { Title = "Zadanie 1", UserId = 1 });
        db.Tasks.Add(new TaskItem { Title = "Zadanie 2", UserId = 1 });
        await db.SaveChangesAsync();

        var controller = new TasksController(db);
        var result = await controller.GetAll(1, null, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var lista = Assert.IsType<List<TaskItem>>(ok.Value);
        Assert.Equal(2, lista.Count);
    }

    // Test 4 — czy usuwanie zadania działa
    [Fact]
    public async Task UsuwanieZadania_ZwracaNoContent()
    {
        var db = CreateDb();
        db.Tasks.Add(new TaskItem { Id = 1, Title = "Do usunięcia", UserId = 1 });
        await db.SaveChangesAsync();

        var controller = new TasksController(db);
        var result = await controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    // Test 5 — czy usuwanie nieistniejącego zadania zwraca NotFound
    [Fact]
    public async Task UsuwanieNieistniejacego_ZwracaNotFound()
    {
        var db = CreateDb();
        var controller = new TasksController(db);

        var result = await controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}