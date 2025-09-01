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
    
    Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "logs"));

    builder.Host.UseSerilog((ctx, services, cfg) =>
    cfg
       .Enrich.FromLogContext()
       .MinimumLevel.Debug()
       .WriteTo.Console()
       .WriteTo.File(
           Path.Combine(Directory.GetCurrentDirectory(), "logs", "log-.txt"),
           rollingInterval: RollingInterval.Day,
           retainedFileCountLimit: 7,
           buffered: false,
           shared: true)
);


    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "P7CreateRestApi", Version = "v1" });

        // Définition de la sécurité pour Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Entrez **Bearer** + espace + votre token JWT.\n\nExemple :\nBearer eyJhbGciOiJIUzI1NiIs...",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

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


    builder.Services.AddScoped<IBidRepository, BidRepository>();
    builder.Services.AddScoped<ICurvePointRepository, CurvePointRepository>();
    builder.Services.AddScoped<IRatingRepository, RatingRepository>();
    builder.Services.AddScoped<IRuleNameRepository, RuleNameRepository>();
    builder.Services.AddScoped<ITradeRepository, TradeRepository>();

    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.AddProfile<ApiMappingProfile>();
    }, typeof(ApiMappingProfile).Assembly);


    builder.Services.AddDbContext<LocalDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero, // <-- Évite les erreurs de délai sur les tokens
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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
