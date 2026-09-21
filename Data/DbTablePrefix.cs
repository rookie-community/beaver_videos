namespace Beaver.Data
{
    /// <summary>
    /// 数据库表名前缀：所有表名统一以它开头（如实体 User -> 表 AppUsers）。
    /// 实体类本身不带前缀，前缀集中在实体配置的 ToTable 中拼接，需要整体更换前缀时只改这一处。
    /// </summary>
    public static class DbTablePrefix
    {
        public const string App = "App";
    }
}
