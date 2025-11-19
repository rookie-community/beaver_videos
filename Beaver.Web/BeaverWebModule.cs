using Beaver.Localization;
using Beaver.MultiTenancy;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;

namespace Beaver
{
    [DependsOn(
        typeof(BeaverApplicationModule),
        typeof(BeaverEntityFrameworkCoreModule),
        typeof(BeaverHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpQuartzModule),
        typeof(AbpSwashbuckleModule)
    )]
    public class BeaverWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(BeaverResource),
                    typeof(BeaverDomainModule).Assembly,
                    typeof(BeaverDomainSharedModule).Assembly,
                    typeof(BeaverApplicationModule).Assembly,
                    typeof(BeaverApplicationContractsModule).Assembly,
                    typeof(BeaverWebModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureAuthentication(context);
            ConfigureUrls(configuration);
            ConfigureAutoMapper();
            ConfigureAutoApiControllers();
            ConfigureSwaggerServices(context.Services);

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
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context)
        {
            //context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
            context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
            {
                options.IsDynamicClaimsEnabled = true;
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureAutoMapper()
        {
            //配置 AutoMapper
            Configure<AbpAutoMapperOptions>(options =>
            {
                // 添加包含 Profile 的程序集
                // ABP 会自动扫描这个程序集中所有的 AutoMapper Profile
                options.AddMaps<BeaverWebModule>();
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(BeaverApplicationModule).Assembly);
            });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddAbpSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BeaverVideos API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
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

            app.UseCorrelationId();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            // 3. 添加 ABP 的身份认证和授权中间件
            app.UseAuthentication();

            if (MultiTenancyConsts.IsEnabled)
            {
                //app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseDynamicClaims();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API");
            });

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
