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

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器中
builder.Services.AddControllersWithViews();

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

// 注册应用程序的服务（例如用户服务和存储库）
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Group>, Repository<Group>>();
builder.Services.AddScoped<IRepository<GroupUser>, Repository<GroupUser>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();




var app = builder.Build();

// 配置HTTP请求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // 强制使用HTTPS，生产环境推荐开启HSTS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 启用身份认证和授权中间件
app.UseAuthentication(); // 必须在UseAuthorization之前调用
app.UseAuthorization();

// 配置控制器路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
