using AppDbContext;
using GestorTareas.Web.Controllers;
using System;
using TareasRepositorio;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//configuracion de la conexion 
builder.Services.AddSingleton(new Contexto(builder.Configuration.GetConnectionString("conexion")));

builder.Services.AddHttpClient<TareasController>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7045/api/Tareas"); // URL base de la API
});
//dependencias


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
