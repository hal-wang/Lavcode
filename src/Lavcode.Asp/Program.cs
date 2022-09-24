using IdentityModel;
using Lavcode.Asp.Entities;
using Lavcode.Asp.Filters;
using Lavcode.Asp.Services;
using LogDashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
builder.Configuration.AddJsonFile("appsettings.local.json", true, true);

builder.Services.AddHttpContextAccessor();

// controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateFilter>();
    options.Filters.Add<ErrorFilter>();
    options.Filters.Add<ExceptionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Lavcode", Version = "v1" });
    options.IncludeXmlComments("./doc.xml");
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    options.AddSecurityDefinition("Bearer", securitySchema);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securitySchema, new[] { "Bearer" }
        }
    });
});

// jwt
var jwtSecret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["SecretKey"]));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = JwtClaimTypes.Name,
        RoleClaimType = JwtClaimTypes.Role,
        ValidIssuer = "hal-wang",
        ValidateIssuer = true,
        ValidAudience = "hal-wang",
        ValidateAudience = true,
        IssuerSigningKey = jwtSecret,
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256, SecurityAlgorithms.RsaSha256, SecurityAlgorithms.Aes128CbcHmacSha256 },
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RequireSignedTokens = true,
        RequireExpirationTime = true,
    };
    o.Events = new JwtBearerEvents()
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            await context.Response.WriteAsJsonAsync(new
            {
                message = "请注销并重新登录"
            });
        }
    };
});
builder.Services.AddSingleton(sp => new SigningCredentials(jwtSecret, SecurityAlgorithms.HmacSha256Signature));
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
builder.Services.AddScoped<AuthTokenService>();

// EF
builder.Services.AddDbContext<DatabaseContext>(
       options => options.UseSqlServer("name=ConnectionStrings:MSSQL"));

// logger
string logOutputTemplate = "{Timestamp:HH:mm:ss.fff zzz} || {Level} || {SourceContext:l} || {Message} || {Exception} ||end {NewLine}";
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .MinimumLevel.Override("Default", LogEventLevel.Information)
  .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
  .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
  .Enrich.FromLogContext()
  .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.SystemConsoleTheme.None)
  .WriteTo.File($"{AppContext.BaseDirectory}logs.log", rollingInterval: RollingInterval.Day, outputTemplate: logOutputTemplate)
  .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddLogDashboard((opt) =>
{
    opt.PathMatch = "/log";
});

var app = builder.Build();

// create db
bool dbCreated = false;
app.Use(async (ctx, next) =>
{
    if (!dbCreated)
    {
        dbCreated = true;
        var dbCotnext = ctx.RequestServices.GetService<DatabaseContext>();
        if (dbCotnext != null)
        {
            await dbCotnext.Database.EnsureCreatedAsync();
        }
    }
    await next();
});

// add Bearer
app.Use(async (ctx, next) =>
{
    var token = ctx.Request.Headers["Authorization"].FirstOrDefault();
    if (!string.IsNullOrEmpty(token) && !token.StartsWith("Bearer "))
    {
        ctx.Request.Headers["Authorization"] = new string[] { "Bearer " + token };
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseLogDashboard("/log");
}

// swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "";
    options.SwaggerEndpoint("swagger/v1/swagger.json", "swagger");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
