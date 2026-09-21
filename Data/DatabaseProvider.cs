namespace Beaver.Data
{
    /// <summary>
    /// 数据库提供程序：取值与配置项 DatabaseType 一一对应（不区分大小写）。
    /// 集中放在数据层，供 AppModule 与设计时工厂统一分派。
    /// </summary>
    public enum DatabaseProvider
    {
        /// <summary>SQLite，本地文件库，默认值</summary>
        Sqlite = 1,

        /// <summary>MySQL，由 Pomelo.EntityFrameworkCore.MySql 承载</summary>
        MySql = 2
    }
}
