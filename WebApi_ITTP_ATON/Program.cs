using Microsoft.EntityFrameworkCore;
using WebApi_ITTP_ATON.Models;
using WebApi_ITTP_ATON.Repositories;
using WebApi_ITTP_ATON.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("UsersDB"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Users API", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    if (!await context.Users.AnyAsync(u => u.Login == "Admin"))
    {
        await context.Users.AddAsync(new User
        {
            Guid = Guid.NewGuid(),
            Login = "Admin",
            Password = "admin",
            Name = "Admin",
            Gender = 2,
            Birthday = null,
            Admin = true,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "System",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "System",
            RevokedOn = DateTime.MinValue,
            RevokedBy = string.Empty
        });
        await context.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API V1");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();

app.Run();