using getQuote.DAO;
using getQuote.Framework;

namespace getQuote.Models
{
    public static class DatabaseInitialize
    {
        public static WebApplication InitializeDB(this WebApplication app) {
            using (var scope = app.Services.CreateScope()) {
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                try
                {
                    context.Database.EnsureCreated();

                    var login = context.Login.FirstOrDefault();
                    if (login == null) {
                        Cryptography cryptography = new();
                        context.Login.AddRange(
                            new LoginModel {Username = "admin", Password = cryptography.GetHash(Environment.GetEnvironmentVariable("USER_PASSWORD"))}
                        );

                        context.SaveChanges();
                    }
                }
                catch (Exception) {
                    throw;
                }

                return app;
            }
        }   
    }
}