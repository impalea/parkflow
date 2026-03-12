using Microsoft.EntityFrameworkCore;
using ParkFlow.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                     ?? throw new InvalidOperationException("'AllowedOrigins' not found in appsettings.json.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("ParkFlowCors", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("ParkFlowCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
