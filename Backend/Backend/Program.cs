using System.Text;
using Backend.Data;
using Backend.Extensions;
using Backend.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddHttpClient<GithubService>();
builder.Services.AddHttpClient<AIService>();
builder.Services.AddScoped<ResumeBuilderService>();

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

builder.Services.AddAuthModule();
builder.Services.AddJobModule();
builder.Services.AddUserModule();

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

app.UseHttpsRedirection();
app.MapControllers();

app.UseCors(myAllowedServices);

app.UseAuthentication();
app.UseAuthorization();

app.Run();
