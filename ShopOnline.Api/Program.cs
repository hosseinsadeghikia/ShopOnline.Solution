using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using ShopOnline.Core.Extensions;
using ShopOnline.Core.IRepository;
using ShopOnline.Core.Repository;
using ShopOnline.Core.Services;
using ShopOnline.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection"),
        sqlOption =>
            sqlOption.MigrationsAssembly("ShopOnline.Data"));
});

//Cache Config
builder.Services.ConfigureHttpCacheHeader();

//Rate Limit Config
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimiting();
builder.Services.AddHttpContextAccessor();

//Identity Config
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddCors(co =>
{
    co.AddPolicy("AllowAll", cpb => cpb.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.ConfigureAutoMapper();

//IOC Container
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDoc();

builder.Services.AddControllers(config =>
{
    config.CacheProfiles.Add("120SecondDuration", new CacheProfile
    {
        Duration = 120
    });
}).AddNewtonsoftJson(st => st.SerializerSettings.
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

builder.Services.ConfigureVersioning();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs\\log-.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => { options.SerializeAsV2 = false; options.RouteTemplate = "swagger/{documentName}/docs.json"; });
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        foreach (var description in
                 app.Services.GetRequiredService<IApiVersionDescriptionProvider>()
                     .ApiVersionDescriptions)
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/docs.json",
                description.GroupName.ToUpperInvariant());
    });
}

app.UseCors(policy =>
{
    policy.WithOrigins("https://localhost:7269", "http://localhost:7269")
        .AllowAnyMethod()
        .WithHeaders(HeaderNames.ContentType);
});

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseResponseCaching();
app.UseHttpCacheHeaders();
app.UseIpRateLimiting();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Application Is Starting");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application Failed to Start");
}
finally
{
    Log.CloseAndFlush();
}
