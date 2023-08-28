using System.Runtime.Caching;
using UrlShortenerService;
using UrlShortenerService.Analytics;
using UrlShortenerService.Mappings;
using UrlShortenerService.Routing;

namespace IddyBiddyUrl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddMongoDbClient();
            builder.Services.AddMongoDbCollections();

            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<RoutingService>();
            builder.Services.AddScoped<MappingService>();
            builder.Services.AddScoped<AnalyticService>();
            builder.Services.AddHttpClient<MappingService>();


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
        }
    }
}