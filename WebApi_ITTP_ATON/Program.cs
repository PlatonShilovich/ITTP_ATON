using WebApi_ITTP_ATON.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Users API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API V1");
        c.RoutePrefix = "swagger";
    });
}

// app.UseHttpsRedirection(); // Закомментировано для избежания предупреждения
app.MapControllers();

app.Run();