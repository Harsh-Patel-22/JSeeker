using System.Text;
using System.Text.Json.Serialization;
using Backend.Data;
using Backend.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});


builder.Services.AddResponseCompression(options => {
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

builder.Services.AddHttpClients();


builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters() {
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        
        ValidAudience = builder.Configuration.GetValue<string>("jwt:audience"),
        ValidIssuer = builder.Configuration.GetValue<string>("jwt:issuer"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("jwt:key") ?? string.Empty))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddQueryServices();
builder.Services.AddServices();
builder.Services.AddAuthModule();
builder.Services.AddJobModule();
builder.Services.AddUserModule();
builder.Services.AddMetricsModule();

var myAllowedServices = "_myAllowedServices";
builder.Services.AddCors(options => {
    options.AddPolicy(name:myAllowedServices,builder => {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    
}

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

int maxRetries = 30;
int delaySeconds = 15;
for (int i = 1; i <= maxRetries; i++)
{
    try
    {
        Console.WriteLine($"[Startup] Attempting database migration/connection (attempt {i}/{maxRetries})...");
        await db.Database.MigrateAsync();
        Console.WriteLine("[Startup] Database is ready and migrations are up to date.");
        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Startup] Database connection/migration failed: {ex.Message}");
        if (i == maxRetries)
        {
            Console.WriteLine("[Startup] Maximum retries reached. Database is still unreachable. Exiting.");
            throw;
        }
        Console.WriteLine($"[Startup] Waiting {delaySeconds} seconds before retrying...");
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
    }
}
// await DbSeeder.SeedAsync(db);

app.UseResponseCompression();
app.UseHttpsRedirection();
app.MapControllers();

app.UseCors(myAllowedServices);

app.UseAuthentication();
app.UseAuthorization();

app.Run();
