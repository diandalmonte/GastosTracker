using Aplicacion.DTOs;
using Infraestructura.Persistencia.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
namespace Presentación
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<GastosTrackerContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"));
            });

            builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("Jwt"));

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var settings = builder.Configuration.GetSection("Jwt").Get<TokenSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key))
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();
            

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
