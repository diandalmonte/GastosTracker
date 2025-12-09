using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Aplicacion.DTOs;
using Aplicacion.DTOs.CategoriaEntity;
using Aplicacion.DTOs.GastoEntity;
using Aplicacion.DTOs.MetodoDePagoEntity;
using Aplicacion.DTOs.UsuarioEntity;
using Aplicacion.Interfaces.AplicacionServices;
using Aplicacion.Interfaces.Infraestructura;
using Aplicacion.Servicios;
using Aplicacion.Servicios.Mappers;
using Dominio.Modelos.Entidades;
using Dominio.Servicios;
using Infraestructura.Exportacion;
using Infraestructura.Persistencia.Contexto;
using Infraestructura.Persistencia.Repositorios;
using Infraestructura.Seguridad;

namespace Presentacion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Configuración de Licencia de ExcelPackage (EPPlus)
            ExcelPackage.License.SetNonCommercialPersonal("Dian Delgado");

            // 2. Base de Datos
            builder.Services.AddDbContext<GastosTrackerContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"));
            });

            // 3. Configuración de JWT (TokenSettings)
            builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("Jwt"));

            // 4. Autenticación
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var settings = builder.Configuration.GetSection("Jwt").Get<TokenSettings>();
                    // Es buena práctica validar que settings no sea null
                    if (settings == null) throw new InvalidOperationException("La configuración JWT no se encuentra.");

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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // 6. CORS (Opcional, útil si el frontend corre en puerto distinto al backend en desarrollo)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Repositorios
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IFiltrableRepository<Gasto, GastoFilter>, GastoRepository>();
            builder.Services.AddScoped<IFiltrableRepository<Categoria, string>, CategoriaRepository>();

            //Mappers
            builder.Services.AddScoped<IMapperService<Usuario, UsuarioRequestDTO, UsuarioResponseDTO>, UsuarioMapper>();
            builder.Services.AddScoped<IMapperService<Gasto, GastoCreateDTO, GastoReadDTO>, GastoMapper>();
            builder.Services.AddScoped<IMapperService<Categoria, CategoriaCreateDTO, CategoriaReadDTO>, CategoriaMapper>();
            builder.Services.AddScoped<IMapperService<MetodoDePago, MetodoDePagoCreateDTO, MetodoDePagoReadDTO>, MetodoDePagoMapper>();

            //Servicios Infraestructura
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IGastoImport, GastoExcelImporter>();

            builder.Services.AddScoped<IExporterFactory, ReporteExporterFactory>();

            builder.Services.AddScoped<IReporteExporter, ReporteExcelExporter>();
            builder.Services.AddScoped<IReporteExporter, ReporteTxtExporter>();
            builder.Services.AddScoped<IReporteExporter, ReporteJsonExporter>();

            // Servicios Dominio
            builder.Services.AddScoped<IPresupuestoManager, PresupuestoManager>();

            //Servicios Aplicación
            builder.Services.AddScoped<IAuthService, AuthenticationService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<IMetodoDePagoService, MetodoDePagoService>();
            builder.Services.AddScoped<IGastoService, GastoService>();
            builder.Services.AddScoped<IPresupuestoService, PresupuestoService>();
            builder.Services.AddScoped<IReporteService, ReporteService>();

            var app = builder.Build();

            app.UseHttpsRedirection();


            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //Fallback al index por si acadso
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}