using Backend.Data;
using Backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddHttpClient<GithubService>();

var MyAllowedServices = "_myAllowedServices";
builder.Services.AddCors(options => {
    options.AddPolicy(name:MyAllowedServices,builder => {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors(MyAllowedServices);
app.Run();
