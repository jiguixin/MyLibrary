using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Data.Ef.DbContextProvider
{
    /// <summary>
    /// DbContext动态组建类
    /// </summary>
    internal class DbContextBuilder : DbModelBuilder, IDbContextBuilder
    {
        // 数据库Provider
        private readonly DbProviderFactory _dbProviderFactory;
        // 连接字符串配置信息
        private readonly ConnectionStringSettings _connectionStringSettings;
        // 当存在数据库时,是否重新创建
        private readonly bool _recreateDatabaseIfExists;
        // 是否延迟加载
        private readonly bool _lazyLoadingEnabled;

        public DbContextBuilder(string connectionStringName, string mappingAssemblyPath, string mappingNamespace,
                                bool recreateDatabaseIfExists, bool lazyLoadingEnabled)
        {
            this.Conventions.Remove<IncludeMetadataConvention>();
            _connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
            _dbProviderFactory = DbProviderFactories.GetFactory(_connectionStringSettings.ProviderName);
            _recreateDatabaseIfExists = recreateDatabaseIfExists;
            _lazyLoadingEnabled = lazyLoadingEnabled;

            AddConfigurations(mappingAssemblyPath, mappingNamespace);
        }

        /// <summary>
        /// 新增DbContext的相关映射配置
        /// </summary>
        /// <param name="mappingAssemblyPath">相关映射配置类所在DLL路径</param>
        /// <param name="mappingNamespace">相关映射配置类所在命名空间</param>
        private void AddConfigurations(string mappingAssemblyPath, string mappingNamespace)
        {
            var asm = Assembly.LoadFrom(GetAssemblyPath(mappingAssemblyPath));
            if (asm == null)
                throw new ApplicationException(string.Format("找不到映射配置类DLL,路径:{0}", mappingAssemblyPath));

            var types = asm.GetTypes()
                .Where(c =>
                       !string.IsNullOrEmpty(c.Namespace)
                       && c.Namespace.ToLower() == mappingNamespace.ToLower()
                       && c.BaseType != null
                       && c.BaseType.IsGenericType
                       && c.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
                ).ToList();

            if (types.Count == 0)
                throw new ApplicationException("没有找到映射配置类");

            types.ForEach(c =>
            {
                dynamic configurationInstance = Activator.CreateInstance(c);
                this.Configurations.Add(configurationInstance);
            });
        }

        private static string GetAssemblyPath(string assemblyPath)
        {
            return (assemblyPath.IndexOf(".dll") == -1)
                       ? assemblyPath.Trim() + ".dll"
                       : assemblyPath.Trim();
        }

        #region Implementation of IDbContextBuilder

        /// <summary>
        /// 创建DbContext
        /// </summary>
        /// <returns></returns>
        public DbContext BuildDbContext()
        {
            var connection = _dbProviderFactory.CreateConnection();
            connection.ConnectionString = _connectionStringSettings.ConnectionString;

            var dbModel = this.Build(connection);
 
            var ctx = dbModel.Compile().CreateObjectContext<ObjectContext>(connection);

            if (!ctx.DatabaseExists())
            {
                ctx.CreateDatabase();
            }
            else if (_recreateDatabaseIfExists)
            {
                ctx.DeleteDatabase();
                ctx.CreateDatabase();
            }

            return new DbContext(ctx, false);
        }

        #endregion
    }
}
