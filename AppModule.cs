using Beaver.Data;
using Beaver.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Beaver
{
    /// <summary>
    /// 单层应用模块：MVC + 应用服务 + 原生 EF Core。
    /// 与重构前的多层结构相比，这里不再依赖任何 ABP 的 EF Core / Identity / 权限管理组件，
    /// 数据访问、实体映射与数据库迁移全部由 EF Core 承担；
    /// 数据库提供程序（SQLite / MySQL）由配置项 DatabaseType 切换，见 Data/DatabaseOptionsExtensions。
    /// </summary>
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpDddApplicationModule)
    )]
    public class AppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = services.GetConfiguration();

            var connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("缺少连接字符串配置 ConnectionStrings:Default。");

            // 数据访问：原生 DbContext，提供程序由 appsettings.json 的 DatabaseType 决定（Sqlite / MySql）
            var databaseTypeText = configuration["DatabaseType"];
            var databaseProvider = DatabaseProvider.Sqlite;
            if (!string.IsNullOrWhiteSpace(databaseTypeText)
                && !Enum.TryParse(databaseTypeText, ignoreCase: true, out databaseProvider))
            {
                throw new InvalidOperationException(
                    $"不支持的数据库类型 DatabaseType：{databaseTypeText}，可选值：{string.Join("、", Enum.GetNames<DatabaseProvider>())}。");
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                if (databaseProvider == DatabaseProvider.MySql)
                {
                    var mySqlServerVersion = ServerVersion.AutoDetect(connectionString);
                    options.UseMySql(connectionString, mySqlServerVersion);
                }
                else
                {
                    options.UseSqlite(connectionString);
                }
            });

            // 口令哈希：官方 Microsoft.AspNetCore.Identity 的 PBKDF2 实现，不保存明文
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            // 登录会话改用 ASP.NET Core Cookie 认证（替代原来的 ABP Identity 登录管理器）
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "Beaver.Auth";
                    options.Cookie.HttpOnly = true;
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    // 已登录但不是管理员时访问后台，落到这里给出提示（默认路径没有视图会变成 404）
                    options.AccessDeniedPath = "/Home/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromDays(14);
                    options.SlidingExpiration = true;
                    options.ReturnUrlParameter = "returnUrl";
                });

            // MovieService / BingWallpaperService 依赖这两项
            services.AddDistributedMemoryCache();
            services.AddHttpClient();

            // OpenAPI 文档与 Scalar 交互式界面（/scalar/v1）
            services.AddOpenApi(options =>
            {
                options.AddScalarTransformers();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseConfiguredEndpoints(endpoints =>
            {
                endpoints.MapOpenApi();
                // scalar/v1
                endpoints.MapScalarApiReference(options =>
                {
                    options.Title = "BeaverVideos API";
                    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });

                endpoints.MapControllerRoute(
                    name: "defaultArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // 数据库首次初始化在这里完成，Program.cs 只保留托管代码。
        // Post 阶段仍在 app.RunAsync() 之前，服务对外可用时表结构与种子数据一定已就绪。
        public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var dbContext = context.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}
