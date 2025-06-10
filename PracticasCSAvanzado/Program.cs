using ApplicationLayer.Services.TareaServices;
using DomainLayer.Models;
using InfraestructureLayer.Context;
using InfraestructureLayer.Repositorio.Commons;
using InfraestructureLayer.Repositorio.TareaRepositorio;
using Microsoft.EntityFrameworkCore;
using PracticasCSAvanzado;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<ManejadorTareasSecuencial>();
builder.Services.AddSingleton<ITareaFactory, Factory>();


builder.Services.AddControllers();

//Conexión a la base de datos
builder.Services.AddDbContext<PracticasCSAvanzadoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PracticasCSADB"))
);

builder.Services.AddScoped<ICommonsProcess<Tarea>, TareaRepositorio>();
builder.Services.AddScoped<TareaService>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
