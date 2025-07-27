using BeaverVideos.Data;
using BeaverVideos.Services.Implementations;
using BeaverVideos.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IBingWallpaperService, BingWallpaperService>();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

//配置Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 配置Identity的主Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Identity/Account/Login");
    options.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
    options.ExpireTimeSpan = TimeSpan.FromDays(14); // 其他可选配置
}).AddAuthentication()
.AddCookie();

var autoMapperLicenseKey = builder.Configuration.GetValue<string>("AutoMapperLicenseKey");
builder.Services.AddAutoMapper(option =>
{
    option.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
    // 可在此处添加额外配置
    option.AllowNullCollections = true;
    option.AllowNullDestinationValues = true;
    option.LicenseKey = autoMapperLicenseKey;
});
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "defaultArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
