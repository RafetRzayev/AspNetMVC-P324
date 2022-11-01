using AspNetMVC_P324.DAL;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AspNetMVC_P324
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
           
            builder.Services.AddDbContext<AppDbContext>(
                    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromMinutes(10));

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}