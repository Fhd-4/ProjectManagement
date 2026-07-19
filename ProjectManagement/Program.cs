using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Models;
using System.Text;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // 1. إعداد نص الاتصال بقاعدة البيانات
        builder.Services.AddDbContext<ProjectManagement.Data.ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // 2. إضافة خدمات ASP.NET Core Identity لإدارة المستخدمين والأدوار
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // هنا تقدر تعدل شروط الباسورد مستقبلاً لو حبيت تسهلها
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<ProjectManagement.Data.ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // 3. إضافة إعدادات التحقق الأمنية باستخدام الـ JWT Bearer Token
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "ProjectManagementSecretIssuer",
                ValidAudience = "ProjectManagementSecretAudience",
                // ملاحظة: هذا مفتاح سري طويل ومشفر لتوقيع الـ Tokens ومنع تزويرها
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FahdProjectManagementSecretKey2026_JWT_Secure!"))
            };
        });

        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "ProjectManagement API",
                Version = "v1"
            });

            // إضافة زر Authorize لإرسال الـ JWT Token في Swagger UI
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
            {
                Description = "الرجاء كتابة الكلمة Bearer متبوعة بمكان فارغ ثم التوكن. مثال: 'Bearer eyJhbGci...' ",
                Name = "Authorization",
                In = Microsoft.OpenApi.ParameterLocation.Header,
                Type = Microsoft.OpenApi.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement((doc) => new Microsoft.OpenApi.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // ⚠️ الترتيب هنا حساس جداً: التوثيق أولاً ثم الصلاحيات
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}