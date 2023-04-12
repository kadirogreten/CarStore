using CarStore.Core;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Options;
using CarStore.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using CarStore.Business.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build();


var logger = new LoggerConfiguration()
    // Read from appsettings.json
    .ReadFrom.Configuration(configuration)
     .WriteTo.File(new JsonFormatter(),
        "Logs/important-logs-.json", rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Debug)

    // Add a log file that will be replaced by a new log file each day
    .WriteTo.File("Logs/all-daily-.logs",
        rollingInterval: RollingInterval.Day)
    .WriteTo.File("Logs/serilog-file-.txt", rollingInterval: RollingInterval.Day)
    // Set default minimum log level
    .MinimumLevel.Debug()
    // Create the actual logger
    .CreateLogger();

try
{
    logger.Information("Starting web application");
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);


    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<CarStoreDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


    builder.Services.AddIdentity<Customer, IdentityRole>(opt =>
    {
        opt.Password.RequiredLength = 5;
        opt.Password.RequireDigit = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Lockout.MaxFailedAccessAttempts = 3;
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        opt.User.RequireUniqueEmail = true;
        opt.SignIn.RequireConfirmedEmail = true;

    })
     .AddEntityFrameworkStores<CarStoreDbContext>()
     .AddDefaultTokenProviders();


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
           .AddJwtBearer(options =>
           {
               options.SaveToken = true;
               options.RequireHttpsMetadata = false;

               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidAudience = "https://localhost:5001",
                   ValidIssuer = "https://localhost:5001",

                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("da758005-8edf-43e0-976b-55880eb27e71"))
               };
           });


    builder.Services.AddSwaggerGen(c =>
    {

        // add a custom operation filter which sets default values
        //c.OperationFilter<SwaggerDefaultValues>();



        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Api anahtarýnýzý giriniz.",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

    });

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<CarStoreDbContext>();
        context.Database.Migrate();
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    logger.Fatal(ex.ToString(), "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
