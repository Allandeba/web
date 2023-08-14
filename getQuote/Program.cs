using System.Text.Json.Serialization;
using getQuote.DAO;
using Microsoft.EntityFrameworkCore;

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

        // Add SyncfusionKey
        //var syncfusionKey = Environment.GetEnvironmentVariable("SYNC_FUSION_LICENSING");
        var syncfusionKey = builder.Configuration.GetConnectionString("SYNC_FUSION_LICENSING");
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);

        // Add MySQL connection.
        //var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        var connectionString = builder.Configuration.GetConnectionString("DB_CONNECTION");
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

        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
