namespace Beaver.Data;

public interface IBookStoreDbSchemaMigrator
{
    Task MigrateAsync();
}
