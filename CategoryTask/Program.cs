using CategoryTask.Api.ApiServices;
using CategoryTask.Interface;
using CategoryTask.Models.Data;
using CategoryTask.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CategoryTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var configuration = builder.Configuration;

            builder.Services.Configure<AppSetting>(configuration);
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppSetting>>().Value);


            builder.Services.AddDbContext<CategoryProductDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("CategoryProductTask")));

            builder.Services.AddScoped<ICategory, CategoryService>();
            builder.Services.AddScoped<IProduct, ProductService>();
            builder.Services.AddScoped<IAppSettingsService, AppSettingsService>();

            builder.Services.AddHttpClient<CategoryApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7247/"); // Replace with your actual API base URL
            });

            builder.Services.AddHttpClient<ProductApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7247/"); // Replace with your actual API base URL
            });

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