using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using WalkingTec.Mvvm.Core;

namespace BeaverVideos.DataAccess
{
    public class DataContext : FrameworkContext
    {
        public DbSet<FrameworkUser> FrameworkUsers { get; set; }

        public DataContext(CS cs)
             : base(cs)
        {
        }

        public DataContext(string cs, DBTypeEnum dbtype)
            : base(cs, dbtype)
        {
        }

        public DataContext(string cs, DBTypeEnum dbtype, string version = null)
            : base(cs, dbtype, version)
        {
        }


        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public override async Task<bool> DataInit(object allModules, bool IsSpa)
        {
            var state = await base.DataInit(allModules, IsSpa);
            bool emptydb = false;
            try
            {
                emptydb = !Set<FrameworkUser>().Any() && !Set<FrameworkUserRole>().Any();
            }
            catch { }
            if (state || emptydb)
            {
                //when state is true, means it's the first time EF create database, do data init here
                //当state是true的时候，表示这是第一次创建数据库，可以在这里进行数据初始化
                var user = new FrameworkUser
                {
                    ITCode = "admin",
                    Password = Utils.GetMD5String("000000"),
                    IsValid = true,
                    Name = "Admin"
                };

                var userrole = new FrameworkUserRole
                {
                    UserCode = user.ITCode,
                    RoleCode = "001"
                };

                Set<FrameworkUser>().Add(user);
                Set<FrameworkUserRole>().Add(userrole);
                await SaveChangesAsync();
            }
            return state;
        }

        /// <summary>
        /// 根据实体类“Description”特性生成数据库字段备注
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                var tabtype = Type.GetType(item.ClrType.FullName);
                if (tabtype != null)
                {
                    var props = tabtype.GetProperties();
                    var descriptionAttrtable = tabtype.GetCustomAttribute<DescriptionAttribute>();
                    if (descriptionAttrtable != null)
                    {
                        modelBuilder.Entity(item.Name).HasComment(descriptionAttrtable.Description);
                    }
                    foreach (var prop in props)
                    {
                        var descriptionAttr = prop.GetCustomAttribute<DescriptionAttribute>();
                        if (descriptionAttr != null)
                        {
                            modelBuilder.Entity(item.Name).Property(prop.Name).HasComment(descriptionAttr.Description);
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// DesignTimeFactory for EF Migration, use your full connection string,
    /// EF will find this class and use the connection defined here to run Add-Migration and Update-Database
    /// </summary>
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private readonly ServiceProvider Provider;
        private readonly WTMContext Configcontext;
        public DataContextFactory()
        {
            var services = new ServiceCollection();
            services.AddWtmContextForConsole();
            Provider = services.BuildServiceProvider();
            Configcontext = Provider.GetRequiredService<WTMContext>();
        }
        public DataContext CreateDbContext(string[] args)
        {
            var configs = Configcontext.ConfigInfo;
            var connectionObject = configs.Connections.FirstOrDefault();
            return new DataContext(connectionObject.Value, (DBTypeEnum)connectionObject.DbType);
        }
    }
}
