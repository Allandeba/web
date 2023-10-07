using getQuote.Controllers;
using getQuote.DAO;
using getQuote.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace getQuote;

public class Program
{
    public static void Main(string[] args)
    {
        DotNetEnv.Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services
            .AddControllers()
            .AddJsonOptions(
                x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
            );
        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/" + nameof(LoginController.Login);
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.Cookie.Name = "authCookie";
            });
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        // Add SyncfusionKey
        var syncfusionKey = Environment.GetEnvironmentVariable("SYNC_FUSION_LICENSING");
        if (syncfusionKey.IsNullOrEmpty())
        {
            syncfusionKey = builder.Configuration.GetConnectionString("SYNC_FUSION_LICENSING");
        }
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);

        // Add MySQL connection.
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        if (connectionString.IsNullOrEmpty())
        {
            connectionString = builder.Configuration.GetConnectionString("DB_CONNECTION");
        }
        builder.Services.AddDbContext<ApplicationDBContext>(
            options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        builder.Services.AddScoped<CatalogRepository>();
        builder.Services.AddScoped<CatalogBusiness>();
        builder.Services.AddScoped<ItemRepository>();
        builder.Services.AddScoped<ItemBusiness>();
        builder.Services.AddScoped<PersonRepository>();
        builder.Services.AddScoped<PersonBusiness>();
        builder.Services.AddScoped<ProposalRepository>();
        builder.Services.AddScoped<ProposalBusiness>();
        builder.Services.AddScoped<ProposalHistoryRepository>();
        builder.Services.AddScoped<ProposalHistoryBusiness>();
        builder.Services.AddScoped<CompanyBusiness>();
        builder.Services.AddScoped<CompanyRepository>();
        builder.Services.AddScoped<LoginBusiness>();
        builder.Services.AddScoped<LoginRepository>();

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(name: "default", pattern: "{controller=Login}/{action=Index}/{id?}");

        app.InitializeDB();

        app.Run();
    }
}
