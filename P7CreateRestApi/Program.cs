using System.Text;
using Microsoft.AspNetCore.HttpLogging; // Optional: built-in HTTP logging
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Data;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Services;
using Serilog;
using P7CreateRestApi.Mapping;

// 1) Bootstrap logger (très tôt, avant CreateBuilder)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up");

    var builder = WebApplication.CreateBuilder(args);

    // Crée le dossier logs si besoin (évite les erreurs de création de fichier)
    Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "logs"));

    // 2) Remplacer le logger par défaut par Serilog
    builder.Host.UseSerilog((ctx, services, cfg) =>
        cfg.ReadFrom.Configuration(ctx.Configuration)
           .ReadFrom.Services(services)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File(
               Path.Combine(AppContext.BaseDirectory, "logs", "log-.txt"),
               rollingInterval: RollingInterval.Day,
               retainedFileCountLimit: 7,
               buffered: false,         // écriture immédiate
               shared: true)            // évite certains verrous
    );

    // Services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<BidService>();
    builder.Services.AddScoped<CurvePointService>();
    builder.Services.AddScoped<RatingService>();
    builder.Services.AddScoped<RuleNameService>();
    builder.Services.AddScoped<TradeService>();

    builder.Services.AddAutoMapper(typeof(ApiMappingProfile));


    // DbContext
    builder.Services.AddDbContext<LocalDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Identity
    builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<LocalDbContext>()
        .AddDefaultTokenProviders();

    // ===== JWT depuis appsettings.json =====
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var jwtKey = jwtSection["Key"];         // ex: "SUPER-SECRET-KEY..."
    var jwtIssuer = jwtSection["Issuer"];   // ex: "P7CreateRestApi"
    var jwtAudience = jwtSection["Audience"]; // ex: "P7CreateRestApiClients"

    builder.Services.AddAuthentication("JwtBearer")
        .AddJwtBearer("JwtBearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Active ValidateIssuer/ValidateAudience si fournis
                ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
                ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(string.IsNullOrWhiteSpace(jwtKey)
                        ? "CHANGE-ME-IN-USER-SECRETS-OR-ENV" // fallback dev
                        : jwtKey))
            };
        });
    // ======================================

    // (Optionnel) HTTP Logging intégré ASP.NET Core — attention aux doublons avec Serilog RequestLogging
    builder.Services.AddHttpLogging(o =>
    {
        o.LoggingFields =
            HttpLoggingFields.RequestProperties |
            HttpLoggingFields.RequestQuery |
            HttpLoggingFields.ResponseStatusCode;

        o.RequestHeaders.Add("User-Agent");
        o.ResponseHeaders.Add("Content-Type");
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Serilog request logging
    app.UseSerilogRequestLogging();

    // (Optionnel) Active aussi le HTTP Logging intégré
    // Si ça fait trop de logs, commente la ligne ci-dessous.
    app.UseHttpLogging();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
