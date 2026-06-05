using CaneOrbis.Api.Data;
using CaneOrbis.Api.Models;
using CaneOrbis.Api.Services;
using CaneOrbit.Api.Models;
using CaneOrbit.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<EosSettings>(
    builder.Configuration.GetSection("Eos"));

builder.Services.Configure<GeminiSettings>(
    builder.Configuration.GetSection("Gemini"));

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__OracleConnection");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(connectionString)
);

builder.Services.AddScoped<EosService>();

builder.Services.AddHttpClient<GeminiService>();

builder.Services.AddScoped<AnaliseAgricolaService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();