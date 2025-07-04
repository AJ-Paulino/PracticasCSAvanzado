using ApplicationLayer.Services.TareaServices;
using DomainLayer.Models;
using InfraestructureLayer.Context;
using InfraestructureLayer.Repositorio.Commons;
using InfraestructureLayer.Repositorio.TareaRepositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PracticasCSAvanzado;
using PracticasCSAvanzado.ClientesConectados;
using PracticasCSAvanzado.Custom;
using PracticasCSAvanzado.Hubs;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Authorization y Authentication
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
    };
});

// Add services to the container.

builder.Services.AddSingleton<ManejadorTareasSecuencial>();
builder.Services.AddSingleton<ITareaFactory, Factory>();
builder.Services.AddSingleton<PracticasCSAvanzado.Custom.Utility>();
//builder.Services.AddSingleton<NotificationHub>();

builder.Services.AddControllers();

//Conexi�n a la base de datos
builder.Services.AddDbContext<PracticasCSAvanzadoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PracticasCSADB"))
);

builder.Services.AddScoped<ICommonsProcess<Tarea>, TareaRepositorio>();
builder.Services.AddScoped<TareaService>();
//builder.Services.AddScoped<NotificationHub>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSignalR();

var app = builder.Build();

//Migraciones de la base de datos

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<PracticasCSAvanzadoContext>();
//    context.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "PEC");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/recibirNotificacion");
//var clienteSignalR = new CCAplicacion1("https://localhost:7009/recibirNotificacion");
//await clienteSignalR.ConectarAsync();

app.Lifetime.ApplicationStarted.Register(() =>
{
    Task.Run(async () =>
    {
        var clienteSignalR = new CCAplicacion1("https://localhost:7009/recibirNotificacion");
        await clienteSignalR.ConectarAsync();
    });
});

app.Run();
