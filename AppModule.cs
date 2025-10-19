using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace BeaverVideos
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqliteModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpQuartzModule)
    )]
    public class AppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient();
            context.Services.AddDistributedMemoryCache();
            context.Services.AddAbpIdentity(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            });

            // 配置 ASP.NET Core 的 Cookie 认证
            context.Services.ConfigureApplicationCookie(options =>
            {
                // 设置登录页面路径
                options.LoginPath = new PathString("/Identity/Account/Login");
                // 设置登出页面路径
                options.LogoutPath = "/Account/Logout";
                // 设置访问被拒绝的页面路径
                options.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
                options.ExpireTimeSpan = TimeSpan.FromDays(14); // 其他可选配置
            });

            // 配置 EF Core
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlite();
            });

            //配置 AutoMapper
            Configure<AbpAutoMapperOptions>(options =>
            {
                // 添加包含 Profile 的程序集
                // ABP 会自动扫描这个程序集中所有的 AutoMapper Profile
                options.AddMaps<AppModule>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            // 3. 添加 ABP 的身份认证和授权中间件
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseConfiguredEndpoints(endpoints =>
            {
                // 配置路由
                endpoints.MapControllerRoute(
                    name: "defaultArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
