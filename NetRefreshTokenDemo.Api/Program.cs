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
//builder.Services.AddEndpointsApiExplorer();

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
builder.Services.AddAuthentication(options => // لتفعيل المصادقة
{
    //هنا بدي احكي للتطبيق انه المصادقة من jwt وكل ركوست مبعوت معه توكن اتاكد انه jwt
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //لنظام الرئيسي للمصادقة سيكون JWT
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => //إعدادات التحقق من صحة التوكين JWT
{
    options.SaveToken = true; //خزن التوكين داخل HttpContext، بحيث تقدر توصله لاحقًا إن احتجته
    options.RequireHttpsMetadata = false; //يسمح باستخدام HTTP في التطوير
    options.TokenValidationParameters = new TokenValidationParameters //التحقق من صحة JWT Token.
    {
        ValidateIssuer = true, //التوكين صادق وجاي من جهة مصدّقة
        ValidateAudience = true, //تحقق من أن التوكين موجه لهذا الـ API (الجمهور).
        ValidAudience = builder.Configuration["JWT:ValidAudience"], //الجهة المصدّرة المقبولة للتوكين
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"], //الجمهور المسموح له باستخدام التوكين
        ClockSkew = TimeSpan.Zero, //إلغاء أي تأخير زمني في وقت انتهاء التوكين (افتراضيًا .NET يسمح بـ 5 دقائق تأخير)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["JWT:secret"]!)) //المفتاح السري المستخدم للتحقق من التوقيع الخاص بالتوكين
    };
});

builder.Services.AddAuthorization();


//register services
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await DbSeeder.SeedData(app);

app.Run();
