using DownloadStation.Server.Data;
using DownloadStation.Server.Middleware;
using DownloadStation.Server.Services.Implementations;
using DownloadStation.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. 配置 Serilog 日志
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// 2. 注入数据库上下文 SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=downloadstation.db";
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

// 3. 配置跨域策略，允许 Vue 前端访问
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueClient", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://127.0.0.1:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // 提供对认证信息的支持
    });
});

// 4. 配置 JWT 认证机制
var jwtSecret = builder.Configuration.GetValue<string>("AppSettings:JwtSecret") ?? "default-fallback-jwt-secret-key-at-least-32-chars";
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

// 5. 依赖注入 (DI)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<ISoftwareService, SoftwareService>();
builder.Services.AddScoped<IVersionService, VersionService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITagService, TagService>();


// 托管进程级别服务的注册：异步提取计算防篡改
builder.Services.AddHostedService<DownloadStation.Server.BackgroundServices.HashComputeService>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 107374182400; // 100GB
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 默认情况下 AddControllers 映射了 API，后续添加 Fallback 即可

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// 配置 HTTP 请求管线
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors("AllowVueClient");

var uploadBasePath = builder.Configuration.GetValue<string>("AppSettings:UploadBasePath") ?? "./uploads";
if (!System.IO.Directory.Exists(uploadBasePath))
{
    System.IO.Directory.CreateDirectory(uploadBasePath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        System.IO.Path.Combine(builder.Environment.ContentRootPath, uploadBasePath)),
    RequestPath = "/uploads"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 配置前端静态资源托管 (供正式部署使用)
app.UseStaticFiles(); 

// 自动执行数据库迁移
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "数据库迁移过程中发生错误。");
    }
}

// 处理 Vue 路由回退 (Vue Router History Mode)
app.MapFallbackToFile("index.html");

app.Run();
