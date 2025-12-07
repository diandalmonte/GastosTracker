using Infraestructura.Persistencia.Contexto;
namespace Presentación
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSqlServer<GastosTrackerContext>(builder.Configuration.GetConnectionString("data source=PHND128;Database=EFCoreStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true"));

            var app = builder.Build();
            

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
