using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetRefreshTokenDemo.Api.Data;
using NetRefreshTokenDemo.Api.Models;
using NetRefreshTokenDemo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 1. Add services to the container

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🟢 2. Connection string
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// 🟢 3. Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🟢 4. Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 🟢 5. Add Authentication
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
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JWT:secret"]!))
    };
});

// 🟢 6. Add Authorization
builder.Services.AddAuthorization();

// 🟢 7. Add custom services (إن وجد)
builder.Services.AddScoped<ITokenService, TokenService>();

// 🔹 8. Build the app
var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 🔹 10. Seed admin user
await DbSeeder.SeedData(app);

app.Run();
