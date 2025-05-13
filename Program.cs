using Amigos.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;


namespace Amigos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //Para inyeccion de dependencias
            builder.Services.AddSingleton<IInc, IncImpl>();  // Mantiene el valor
                                                             //builder.Services.AddScoped<IInc, IncImpl>();  // Se reinicia por solicitud HTTP
                                                             //builder.Services.AddTransient<IInc, IncImpl>(); // Se reinicia en cada llamada

            // Para añadir la base de datos como dependencia
            builder.Services.AddDbContext<AmigoDBContext>(options =>
                         options.UseSqlite("Data Source=Amigos.db"));


            // Agregar SignalR a los servicios
            builder.Services.AddSignalR();  // Añadir SignalR aquí


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            // Mapeo de rutas de SignalR
            app.MapHub<SignalRNotification>("/notificaciones");


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "Prueba",
                 pattern: "{controller}/{action}/{valor}/{veces}");

            app.Run();
        }
    }
}