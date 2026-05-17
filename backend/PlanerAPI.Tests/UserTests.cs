using Microsoft.EntityFrameworkCore;
using PlanerAPI.Controllers;
using PlanerAPI.Data;
using PlanerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace PlanerAPI.Tests;

public class UserTests
{
    private AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // Test 1 — czy rejestracja działa
    [Fact]
    public async Task Rejestracja_ZwracaOk()
    {
        var db = CreateDb();
        var controller = new UsersController(db);

        var dto = new RegisterDto("Hutek", "hutek@test.pl", "haslo123");
        var result = await controller.Register(dto);

        Assert.IsType<OkObjectResult>(result);
    }

    // Test 2 — czy duplikat emaila jest odrzucany
    [Fact]
    public async Task RejestranjaDuplikatuEmaila_ZwracaBadRequest()
    {
        var db = CreateDb();
        var controller = new UsersController(db);

        var dto = new RegisterDto("Hutek", "hutek@test.pl", "haslo123");
        await controller.Register(dto);

        // Próba rejestracji z tym samym emailem
        var result = await controller.Register(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    // Test 3 — czy logowanie działa
    [Fact]
    public async Task Logowanie_PoprawneDane_ZwracaOk()
    {
        var db = CreateDb();
        var controller = new UsersController(db);

        await controller.Register(new RegisterDto("Hutek", "hutek@test.pl", "haslo123"));

        var result = await controller.Login(new LoginDto("hutek@test.pl", "haslo123"));

        Assert.IsType<OkObjectResult>(result);
    }

    // Test 4 — czy złe hasło jest odrzucane
    [Fact]
    public async Task Logowanie_ZleHaslo_ZwracaUnauthorized()
    {
        var db = CreateDb();
        var controller = new UsersController(db);

        await controller.Register(new RegisterDto("Hutek", "hutek@test.pl", "haslo123"));

        var result = await controller.Login(new LoginDto("hutek@test.pl", "zlehaslo"));

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}