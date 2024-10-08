using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VenueBookingSystem.Data;
using VenueBookingSystem.Services;
using VenueBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using sports_management.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();  // 添加控制台日志记录

// 定义 CORS 策略名称
var AllowSpecificOrigins = "AllowSpecificOrigins";

// 添加 CORS 服务
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*")
                                .SetIsOriginAllowedToAllowWildcardSubdomains()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// 添加服务到容器中
builder.Services.AddControllersWithViews();

// 配置 ApplicationDbContext 使用 Oracle 数据库
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"))
           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
           .UseLoggerFactory(LoggerFactory.Create(logBuilder => logBuilder.AddConsole()))
           .EnableSensitiveDataLogging(); // 可选：启用敏感数据日志记录
});


// 配置JWT认证服务
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// 注册应用程序的服务和存储库
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserPersonalInfoService, UserPersonalInfoService>();
builder.Services.AddScoped<IUserGroupInfoService, UserGroupInfoService>();
builder.Services.AddScoped<IVenueAnalysisService, VenueAnalysisService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IVenueAvailabilityServices, VenueAvailabilityService>();
builder.Services.AddScoped<IUserReservationService, UserReservationService>();

var app = builder.Build();

// 配置HTTP请求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // 使用指定路径的异常处理程序
    app.UseHsts(); // 在生产环境中启用 HSTS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(AllowSpecificOrigins); // 启用 CORS 中间件

app.UseAuthentication(); // 添加认证中间件
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
