using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql;

namespace Beaver.Data
{
    /// <summary>
    /// 设计时工厂：让 `dotnet ef` 命令无需启动整个 ABP 宿主即可构造上下文。
    /// 注意：本项目建库走 EnsureCreated + HasData（见 AppModule），仓库里没有 EF Core 迁移，
    /// 该工厂仅为将来可能重新引入 EF Core 工具链而保留，正常运行流程不依赖它。
    /// 配置加载与运行时保持一致：按当前环境依次叠加
    /// appsettings.json → appsettings.{环境}.json → 环境变量，
    /// 环境取自 ASPNETCORE_ENVIRONMENT / DOTNET_ENVIRONMENT，缺省为 Production。
    /// 因此默认走正式环境的 MySQL；设 ASPNETCORE_ENVIRONMENT=Development 即切到测试环境的 SQLite。
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? Environments.Production;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                // 环境变量优先级最高，便于命令行 / CI 临时切换 DatabaseType 或连接串
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("缺少连接字符串配置 ConnectionStrings:Default。");

            // 与运行时保持同一套分派逻辑：DatabaseType 决定用 Sqlite 还是 MySql
            var databaseTypeText = configuration["DatabaseType"];
            var databaseProvider = DatabaseProvider.Sqlite;
            if (!string.IsNullOrWhiteSpace(databaseTypeText)
                && !Enum.TryParse(databaseTypeText, ignoreCase: true, out databaseProvider))
            {
                throw new InvalidOperationException(
                    $"不支持的数据库类型 DatabaseType：{databaseTypeText}，可选值：{string.Join("、", Enum.GetNames<DatabaseProvider>())}。");
            }

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            if (databaseProvider == DatabaseProvider.MySql)
            {
                // MySQL 服务器版本：优先用连接串自动探测（运行时/可达环境）；
                // 探测失败（如本机无法连接数据库服务器）时，回退到 8.0 默认版本，
                // 保证 `dotnet ef migrations add` 在设计时也能离线生成 MySQL 迁移。
                ServerVersion serverVersion;
                try
                {
                    serverVersion = ServerVersion.AutoDetect(connectionString);
                }
                catch
                {
                    serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
                }

                builder.UseMySql(connectionString, serverVersion);
            }
            else
            {
                builder.UseSqlite(connectionString);
            }

            return new AppDbContext(builder.Options);
        }
    }
}
