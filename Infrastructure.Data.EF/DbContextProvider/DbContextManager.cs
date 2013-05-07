using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Ef.DbContextProvider
{
    /// <summary>
    /// DbContext管理类
    /// </summary>
    public static class DbContextManager
    {
        // DbContext仓库
        private static IDbContextStorage _storage;
        // DbContext动态组装器容器
        private static readonly Dictionary<string, IDbContextBuilder> _dbContextBuilders =
            new Dictionary<string, IDbContextBuilder>();
        // 线程锁
        private static readonly object SyncLock = new object();

        /// <summary>
        /// 初始化IDbContextStorage
        /// </summary>
        public static void InitStorage(IDbContextStorage storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// 初始化DbContext
        /// </summary>
        /// <param name="connectionStringName">连接字符串名称</param>
        /// <param name="mappingAssemblyPath">映射配置类所在DLL路径</param>
        /// <param name="mappingNamespace">映射配置类所在命名空间</param>
        /// <param name="recreateDatabaseIfExists">当存在数据库时,是否重新创建</param>
        /// <param name="lazyLoadingEnabled">是否延迟加载</param>
        public static void InitDbContext(string connectionStringName, string mappingAssemblyPath,
                                         string mappingNamespace, bool recreateDatabaseIfExists = false,
                                         bool lazyLoadingEnabled = true)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentNullException("connectionStringName");
            if (string.IsNullOrEmpty(mappingAssemblyPath))
                throw new ArgumentNullException("mappingAssemblyPath");
            if (string.IsNullOrEmpty(mappingNamespace))
                throw new ArgumentNullException("mappingNamespace");

            lock (SyncLock)
            {
                _dbContextBuilders.Add(connectionStringName,
                     new DbContextBuilder(connectionStringName, mappingAssemblyPath, mappingNamespace, recreateDatabaseIfExists, lazyLoadingEnabled));
            }
        }

        /// <summary>
        /// 关闭当前所有DbContext
        /// </summary>
        public static void CloseAllDbContexts()
        {
            foreach (var dbContext in _storage.GetAll())
            {
                if (dbContext.Database.Connection.State == ConnectionState.Open)
                    dbContext.Database.Connection.Close();
            }
        }

        /// <summary>
        /// 根据KEY获取当前DbContextStorage中的某个DbContext
        /// </summary>
        /// <param name="key">KEY</param>
        /// <returns>DbContext</returns>
        internal static DbContext CurrentByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (_storage == null)
                throw new ApplicationException("当前应用程序的DbContextStorage未进行初始化，请先初始化IDbContextStorage");

            DbContext dbContext;
            lock (SyncLock)
            {
                if (!_dbContextBuilders.ContainsKey(key))
                    throw new ApplicationException(string.Format("DbContextBuiders中不存在值为{0}的KEY", key));

                dbContext = _storage.GetByKey(key);
                if (dbContext == null)
                {
                    dbContext = _dbContextBuilders[key].BuildDbContext();
                    _storage.SetByKey(key, dbContext);
                }
            }
            return dbContext;
        }
    }
}
