# BeaverVideos

这是一个基于 ASP.NET Core 的视频网站项目，主要功能包括视频展示、搜索、登录注册、密码修改等。项目使用了多种开源技术，包括 [Layui](https://www.layui.site/)、[Font Awesome](https://fontawesome.com/) 等前端框架，以及后端的 Entity Framework Core 和 ASP.NET Core Identity。

## 功能模块

- **身份认证**
  - 登录（支持防伪令牌）
  - 注册
  - 注销
  - 修改密码

- **视频展示**
  - 首页轮播图展示
  - 视频分类展示
  - 视频推荐
  - 视频详情页（包含播放链接、简介、演员等信息）

- **搜索功能**
  - 支持根据视频名称进行搜索
  - 支持自动补全建议

- **播放器集成**
  - 支持多个视频平台播放（如优酷、爱奇艺、B站等）
  - 视频播放页面支持解析多个视频源

- **后台管理**
  - 用户管理
  - 视频信息管理
  - 推荐内容管理

## 技术栈

- **后端**
  - ASP.NET Core 6.0
  - Entity Framework Core
  - ASP.NET Core Identity
  - IHttpClientFactory + Polly 实现 HTTP 请求管理
  - IDistributedCache 实现缓存
  - AutoMapper 实现 DTO 与 Model 映射

- **前端**
  - Layui
  - Font Awesome
  - Bootstrap
  - jQuery
  - HTML5 + CSS3

- **数据库**
  - 使用 SQLite（默认）或可配置为 SQL Server / MySQL / PostgreSQL

- **其他**
  - 使用 Bing 壁纸 API 获取背景图
  - 使用分布式缓存优化性能
  - 支持多语言（中文、英文）

## 项目结构

- **Controllers**
  - `HomeController`：首页、搜索、详情页
  - `AccountController`：登录、注册、注销、修改密码
  - `EquipmentController`：设备管理（后台）

- **Views**
  - 使用 Razor 视图引擎
  - 包含首页、搜索、播放、登录、注册等页面
  - 使用 `_Layout.cshtml` 作为主布局模板

- **Models**
  - 包含视图模型（ViewModel）和数据库模型（MovieDetail、MovieCarousel 等）

- **Services**
  - `IMovieService`：提供视频数据获取接口
  - `IBingWallpaperService`：获取 Bing 壁纸作为背景图

- **Data**
  - `ApplicationDbContext`：继承自 `IdentityDbContext`，用于用户和视频数据管理

- **wwwroot**
  - 静态资源（CSS、JS、图片、字体等）
  - 使用 `layui-theme-dark` 深色主题
  - 使用 `font-awesome` 图标库

## 安装与运行

### 前提条件

- .NET 6 SDK
- Node.js（用于前端资源构建）
- 数据库（SQLite 默认，也可配置为 SQL Server / MySQL / PostgreSQL）

### 安装步骤

1. 克隆项目：

   ```bash
   git clone https://gitee.com/swan_goose_2/beaver_videos.git
   cd beaver_videos
   ```

2. 恢复依赖：

   ```bash
   dotnet restore
   ```

3. 构建项目：

   ```bash
   dotnet build
   ```

4. 运行项目：

   ```bash
   dotnet run
   ```

   或使用 Visual Studio / Rider 直接运行。

5. 初始化数据库：

   ```bash
   dotnet ef database update
   ```

   或使用迁移脚本创建数据库结构。

### 前端资源构建（可选）

如果需要自定义前端样式或主题，可以进入 `wwwroot/lib` 目录进行二次开发：

```bash
cd wwwroot/lib/layui-theme-dark
npm install
npm run dev
```

## 使用说明

- 首页展示轮播图、推荐视频、分类视频等
- 用户可登录并修改密码
- 支持搜索视频名称、自动补全
- 视频详情页展示详细信息并提供多个播放源
- 后台可管理视频信息、推荐内容、用户权限等

## 扩展与二次开发

- 可扩展 `IMovieService` 接口以支持更多视频源
- 可使用 `xm-select`、`layui-theme-dark` 等组件进行 UI 优化
- 支持深色/浅色主题切换（通过 `dark` 类名控制）

## 开源协议

本项目采用 [MIT License](https://opensource.org/licenses/MIT)，请在使用时保留原始版权信息。

## 联系方式

- Gitee 仓库：[https://gitee.com/swan_goose_2/beaver_videos](https://gitee.com/swan_goose_2/beaver_videos)
- 作者：swan_goose_2

---

欢迎贡献代码、提交 Issue 或提出改进建议！