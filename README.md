# BeaverVideos（单层架构版）

基于 **ASP.NET Core (net9.0) + ABP 核心** 的影视站，使用**原生 EF Core**，数据库支持 **SQLite / MySQL** 切换。

> 本项目由原多项目（分层）解决方案重构而来：**9 个项目合并为 1 个**，数据访问层不再依赖任何
> `Volo.Abp.EntityFrameworkCore*` / `Volo.Abp.Identity*` / `Volo.Abp.PermissionManagement*` 组件，
> 改为仅使用 `Microsoft.EntityFrameworkCore.*` 实现 DbContext、实体映射与数据库迁移。
> 重构前的完整代码已备份在 `C:\Code\BeaverVideos_backup`。

## 技术栈

| 层次 | 选型 |
| --- | --- |
| Web / MVC | ASP.NET Core MVC（`Microsoft.NET.Sdk.Web`，单项目） |
| 模块与 DI | ABP 核心：`Volo.Abp.AspNetCore.Mvc`、`Volo.Abp.Autofac`、`Volo.Abp.Ddd.Application` |
| 数据访问 | **原生 `DbContext`** + `IEntityTypeConfiguration`；首次建库用 `EnsureCreated`，种子数据写在实体配置类里（`HasData`） |
| 数据库 | **SQLite**（`Microsoft.EntityFrameworkCore.Sqlite`，默认）/ **MySQL**（`Pomelo.EntityFrameworkCore.MySql`），由配置项 `DatabaseType` 切换 |
| 认证 | ASP.NET Core **Cookie 认证**（替代原 ABP Identity 的 `AbpSignInManager`） |
| 口令哈希 | `Microsoft.AspNetCore.Identity.PasswordHasher<T>`（PBKDF2，不存明文） |
| API 文档 | OpenAPI + Scalar（`/scalar/v1`） |

## 目录结构

```
BeaverVideos/
├── BeaverVideos.csproj        # 唯一的项目文件
├── BeaverVideos.slnx          # 解决方案（仅引用上面这一个项目）
├── Program.cs                 # 托管入口（UseAutofac + AddApplicationAsync）
├── AppModule.cs               # ABP 模块：DI 注册、中间件管道、启动时首次建库
├── appsettings.json           # 正式环境配置（DatabaseType=MySql）
├── appsettings.Development.json # 测试环境覆盖项（DatabaseType=Sqlite）
├── Data/                      # 数据访问层
│   ├── AppDbContext.cs        #   原生 DbContext（含创建时间自动填充）
│   ├── DatabaseProvider.cs    #   数据库提供程序枚举（Sqlite / MySql）
│   ├── DbTablePrefix.cs       #   表名前缀（统一前缀 App）
│   ├── AppSeedData.cs         #   内置账号种子常量（含固定盐口令哈希），供配置类 HasData 引用
│   ├── AppDbContextFactory.cs #   设计时工厂（供 dotnet ef 使用）
│   └── Configurations/        #   实体映射（IEntityTypeConfiguration）
├── Entities/                  # 持久化实体（User : ABP CreationAuditedEntity<Guid>，含 IsAdmin 标记）
├── Security/                  # 角色常量（AppRoles.Admin / AppRoles.User）
├── Services/                  # 业务服务层：DTO 与实现放在同一模块内
│   ├── Movies/                #   影视业务：IMovieService / MovieService（360kan 接口）、CatType 等枚举
│   │   └── Dtos/              #     搜索、详情、排行榜、轮播等 DTO 与 ViewModel
│   ├── Wallpapers/            #   Bing 壁纸服务（登录页背景图）
│   │   └── Dtos/              #     壁纸请求 / 响应 DTO
│   └── Account/               #   账号应用服务（登录校验、注册、最近登录、改密）
│       └── Dtos/              #     登录表单、注册表单、修改口令 DTO
├── Controllers/               # Home / Account / Equipment / AccountApi
├── Areas/System/              # 后台系统管理（用户、角色、组织机构、声明类型、安全日志）
├── Areas/Workbench/           # 控制台
├── Menus/                     # 左侧菜单 ViewComponent 与 MenuItemDto
├── Models/                    # 通用模型（LayuiResultDto、ErrorViewModel）
├── Extensions/                # 枚举与 DTO 扩展
├── Views/                     # Razor 视图（Home / Account / Equipment / Shared）
└── wwwroot/                   # 静态资源（Layui、Font Awesome、jQuery、xm-select 等）
```

## 数据库

### 按环境切换：正式 MySQL，测试 SQLite

数据库类型不由代码推断，而是由**配置项 `DatabaseType` 显式指定**（取值与 `Data/DatabaseProvider`
一致，不区分大小写）；不同环境的差异通过 ASP.NET Core 的环境配置文件覆盖实现：

| 环境 | 配置文件 | `DatabaseType` | 提供程序 | 说明 |
| --- | --- | --- | --- | --- |
| 正式（Production） | `appsettings.json` | `MySql` | `Pomelo.EntityFrameworkCore.MySql`（net9.0 → 9.0.0） | 基础配置即正式环境，部署时替换连接串 |
| 测试 / 开发（Development） | `appsettings.Development.json` | `Sqlite` | `Microsoft.EntityFrameworkCore.Sqlite` | 单文件、免安装、删档即重建 |

配置加载顺序（后者覆盖前者）：

```
appsettings.json  →  appsettings.{环境}.json  →  环境变量  →  命令行参数
```

`{环境}` 取自 `ASPNETCORE_ENVIRONMENT`（或 `DOTNET_ENVIRONMENT`），**未设置即为 `Production`**。

```jsonc
// appsettings.json —— 正式环境
{
  "DatabaseType": "MySql",
  "ConnectionStrings": {
    "Default": "server=127.0.0.1;port=3306;database=BeaverVideos;user=root;password=CHANGE_ME;"
  }
}
```

```jsonc
// appsettings.Development.json —— 测试环境，只写与正式环境不同的键
{
  "DatabaseType": "Sqlite",
  "ConnectionStrings": { "Default": "Data Source=BeaverVideos.db;" }
}
```

本地 `dotnet run` / Visual Studio 调试会走 `Properties/launchSettings.json` 里预设的
`ASPNETCORE_ENVIRONMENT=Development`，因此自动使用 SQLite；部署到服务器时不要设置该变量
（或设为 `Production`）即使用 MySQL。

> SQLite 连接串是相对路径，`BeaverVideos.db` 会生成在**应用的当前工作目录**下
> （`dotnet run` 即项目根目录，见启动日志的 `Content root path`）。
> `.gitignore` 已排除 `*.db` / `*.db-shm` / `*.db-wal`，测试库不会入库。

### 建库与结构变更

- **首次建库**：应用启动时在 `AppModule.OnPostApplicationInitializationAsync` 中执行
  `await dbContext.Database.EnsureCreatedAsync()`，按**当前模型**（实体 + `Data/Configurations` 下的映射）
  一次性建表。数据库已存在时该方法直接返回：不比对模型、不补建表、不补种子。
- **种子数据**：声明在实体配置类里（如 `UserConfiguration` 的 `HasData`），随建库一并插入，只写这一次。
  种子里的口令哈希用**固定盐**生成（见 `Data/AppSeedData.cs`），值确定，既能作为模型种子，
  也能被 `PasswordHasher` 正常校验——默认账号 `admin` / `123456` 可照常登录。
- **结构变更用手工 SQL**：本项目**不依赖自动迁移机制**——启动时不再 `MigrateAsync`，
  仓库中也没有 `Migrations/` 目录与 `__EFMigrationsHistory` 表。需要加表 / 加列 / 改类型时，
  手工编写 SQL 并在目标库上执行，例如：

  ```sql
  -- 给账号表增加一列（SQLite 与 MySQL 的 DDL 写法不同，两个库需分别执行）
  ALTER TABLE AppUsers ADD COLUMN Birthday datetime(6) NULL;
  ```

  执行完 SQL 后，同步修改实体、`Data/Configurations` 下的映射，以及对应的 `HasData` 种子；
  SQLite（测试）与 MySQL（正式）两个环境的库都要各改一次。
- 表名统一带 `App` 前缀：`User → AppUsers`，前缀集中在 `Data/DbTablePrefix.cs`。
- **重置测试库**：停掉应用后直接删 `BeaverVideos.db`（连同 `-shm` / `-wal`），下次启动会重新建表并写入种子账号。

> `Data/AppDbContextFactory.cs`（设计时工厂）保留下来，仅在未来重新引入 EF Core 工具链时才用得上，
> 正常启动流程不依赖它；它按与运行时**相同**的规则解析 `DatabaseType` 与环境变量。

## 认证与权限

### 登录后的跳转规则

登录成功后由 `AccountController.ResolveRedirectUrl(isAdmin, returnUrl)` 决定去向：

| 账号类型 | ReturnUrl | 跳转目标 |
| --- | --- | --- |
| 管理员 | 空 | `/Workbench/Dashboard`（默认进后台） |
| 管理员 | 任意 | 该 ReturnUrl（含 `/System/*` 等后台路径） |
| 普通用户 | 空 | `/Home/Index` |
| 普通用户 | 非后台路径（如 `/Home/Detail?...`） | 该 ReturnUrl |
| 普通用户 | 后台路径（`/System/*`、`/Workbench/*`） | 丢弃 ReturnUrl，回 `/Home/Index` |

「后台路径」的判定在 `ResolveRedirectUrl` 内的 `IsBackstage()`：以 `/system` 或 `/workbench`
开头（不区分大小写）即视为后台。这样普通用户即使手工构造后台 ReturnUrl 也进不去。

### 权限模型

只用「一个布尔字段 + 角色声明」，不引入角色表（后台角色管理目前仍是骨架）：

- `Entities/User.IsAdmin`：是否管理员
- 登录时 `AccountController.SignInAsync()` 按该字段下发
  `ClaimTypes.Role = AppRoles.Admin` 或 `AppRoles.User`（常量见 `Security/AppRoles.cs`）
- `Areas/System/**`、`Areas/Workbench/**` 的控制器统一标注 `[Authorize(Roles = AppRoles.Admin)]`

普通用户（含自助注册的账号）访问后台时，Cookie 认证会跳到
`CookieAuthenticationOptions.AccessDeniedPath = "/Home/AccessDenied"`，
由 `HomeController.AccessDenied()` 返回 **403** + `Views/Home/AccessDenied.cshtml` 友好提示页。

### 自助注册

登录页底部「还没有账号？立即注册」→ `/Account/Register`（`Views/Account/Register.cshtml`）。

- 表单元数据见 `Services/Account/Dtos/RegisterDto.cs`：登录名 3–64 位且仅 `[a-zA-Z0-9_]`、
  口令 6–64 位、二次确认由 `[Compare]` 校验
- 实现见 `AccountAppService.RegisterAsync()`：判重、拒绝占用保留名 `admin`、
  口令经 `IPasswordHasher<User>` 哈希，**一律写入 `IsAdmin = false`**
- 注册成功后直接登录并按上表跳转（普通用户 → `/Home/Index`）
- 内置 `admin` 的 `IsAdmin = true` 由建库时的种子数据直接写入（见 `UserConfiguration.HasData`），
  不再有启动时的兜底修正逻辑

## 运行

### 测试 / 开发环境（SQLite）

```bash
dotnet restore
dotnet run                      # launchSettings 预设 ASPNETCORE_ENVIRONMENT=Development
```

默认管理员账号：`admin` / `123456`（开发环境下登录页会自动预填），数据文件 `BeaverVideos.db`。
开发环境默认地址为 `http://localhost:5114`（见 `Properties/launchSettings.json`）。

### 正式环境（MySQL）

```bash
# 1) 替换 appsettings.json 中的 ConnectionStrings:Default
# 2) 不要设置 ASPNETCORE_ENVIRONMENT（或设为 Production）
dotnet publish -c Release -o out
cd out && dotnet BeaverVideos.dll
```

首次启动时执行 `EnsureCreatedAsync()`，按当前模型在 MySQL 上建表，并写入 `HasData` 声明的默认管理员账号；
库已存在时不做任何改动，后续表结构变更请手工执行 SQL（见上文「建库与结构变更」）。

## 与重构前的对应关系

| 重构前 | 重构后 |
| --- | --- |
| `Beaver.Web` / `Beaver.HttpApi` / `Beaver.Application` / `Beaver.Application.Contracts` / `Beaver.Domain` / `Beaver.Domain.Shared` | 合并进单一项目（按特性分目录，业务服务与 DTO 归入 `Services/{模块}/`） |
| `Beaver.EntityFrameworkCore`（`AbpDbContext` + `IIdentityDbContext`） | `Data/AppDbContext.cs`（原生 `DbContext`） |
| `Volo.Abp.EntityFrameworkCore.Sqlite` | `Microsoft.EntityFrameworkCore.Sqlite`（+ `Pomelo.EntityFrameworkCore.MySql`） |
| `Volo.Abp.Identity.*`（`IdentityUser` / `AbpSignInManager` / `IIdentityUserAppService`） | 自建 `Entities/User` + Cookie 认证 + `Services/Account/AccountAppService` |
| `Volo.Abp.PermissionManagement.*`、`Volo.Abp.Emailing`、`Volo.Abp.Quartz` | 移除（原项目未实际使用） |
| `Beaver.DbMigrator` 控制台程序 | 启动时 `Database.EnsureCreatedAsync()`（仅首次建库，不升级结构） |
| `Beaver.Test`（空 MSTest 模板）、Books 示例契约、`BookStore` 权限与本地化 | 移除（无任何调用方） |

## 已知事项

1. **后台用户 / 角色管理目前是页面骨架**：原实现依赖 ABP Identity，移除该组件后，
   `/System/User`、`/System/Role` 仍可访问，但表格数据接口返回空列表，
   `/System/ClaimType`、`/System/OrganizationUnit`、`/System/SecurityLog` 为占位页。
   待办事宜见对应控制器中的 `TODO` 注释。**新增后台功能时记得给控制器加
   `[Authorize(Roles = AppRoles.Admin)]`**，否则默认只需登录即可访问。
2. **`admin` 是保留登录名**：注册接口会拒绝该用户名。目前没有「从后台把普通用户提升为管理员」的入口，
   需要时直接改库中 `AppUsers.IsAdmin`。
3. `FluentResults` 固定为 **3.15.0**：`FluentResults.Extensions.AspNetCore` 0.2.0 是针对 3.15.0
   编译的，若使用 4.0.0（重构前的实际解析结果）会让 `ToActionResult()` 抛
   `MissingMethodException`，影视详情页在上游接口报错时会返回 500。
4. 影视数据来自第三方接口（`api.web.360kan.com` 等），其可用性不受本项目控制。
5. **表结构变更靠手工 SQL**：项目不使用 EF Core 迁移，库里没有 `__EFMigrationsHistory` 表，
   因此不要对已有库执行 `dotnet ef database update`（会尝试从零建表而报「表已存在」），
   也不要指望启动时自动升级结构。
6. **`appsettings.json` 里的 MySQL 连接串是占位值**（`password=CHANGE_ME`），部署前必须替换；
   应用启动时用 `ServerVersion.AutoDetect` 连库探测一次 MySQL 版本，数据库不可达会直接启动失败，
   请确保网络与账号可用。

## 开源协议

[MIT License](LICENSE)
