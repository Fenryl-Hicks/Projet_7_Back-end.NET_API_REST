using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using P7CreateRestApi.Data;
using P7CreateRestApi.Entities;
using P7CreateRestApi.Mapping;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;
using Serilog;



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting up");

    var builder = WebApplication.CreateBuilder(args);

    // Crée le dossier logs 
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
               buffered: false,        
               shared: true)            
    );

    // Services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "P7CreateRestApi", Version = "v1" });

        // Définir le schéma de sécurité
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,    // <-- ICI : Http au lieu de ApiKey
            Scheme = "Bearer",                // <-- Respecter la casse
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Entrez : Bearer {votre_token}"
        });

        // Exiger le schéma pour toutes les opérations
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});


    builder.Services.AddScoped<BidService>();
    builder.Services.AddScoped<CurvePointService>();
    builder.Services.AddScoped<RatingService>();
    builder.Services.AddScoped<RuleNameService>();
    builder.Services.AddScoped<TradeService>();

    // Repositories (interface -> implémentation)
    builder.Services.AddScoped<IBidRepository, BidRepository>();
    builder.Services.AddScoped<ICurvePointRepository, CurvePointRepository>();
    builder.Services.AddScoped<IRatingRepository, RatingRepository>();
    builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
    builder.Services.AddScoped<ITradeRepository, TradeRepository>();

    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<ApiMappingProfile>();
    }, typeof(ApiMappingProfile).Assembly);


    // DbContext
    builder.Services.AddDbContext<LocalDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Identity
    builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<LocalDbContext>()
        .AddDefaultTokenProviders();

   
    var jwtSection = builder.Configuration.GetSection("Jwt");
    var jwtKey = jwtSection["Key"];         
    var jwtIssuer = jwtSection["Issuer"];   
    var jwtAudience = jwtSection["Audience"];

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer ( options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                
                ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
                ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey))
                     
            };
        });


    
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

    
    app.UseSerilogRequestLogging();

    
    app.UseHttpLogging();

    app.UseRouting();

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
