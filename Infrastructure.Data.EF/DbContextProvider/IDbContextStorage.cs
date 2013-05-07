using System.Collections.Generic;
using System.Data.Entity;

namespace Infrastructure.Data.Ef.DbContextProvider
{
    /// <summary>
    /// DbContext数据仓库接口
    /// </summary>
    public interface IDbContextStorage
    {
        /// <summary>
        /// 根据KEY获取DbContext
        /// </summary>
        /// <param name="key">KEY</param>
        /// <returns>DbContext</returns>
        DbContext GetByKey(string key);

        /// <summary>
        /// 根据KEY保存DbContext到仓库
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="context">DbContext</param>
        void SetByKey(string key, DbContext context);

        /// <summary>
        /// 获取所有DbContext
        /// </summary>
        /// <returns>DbContext列表</returns>
        IEnumerable<DbContext> GetAll();
    }

    //该接口的实现方式如下：
    /*
    /// <summary>
    /// 用于WEB应用程序的DbContext
    /// </summary>
    public class WebDbContextStorage : IDbContextStorage
    {
        // DbContext容器
        private Dictionary<string, DbContext> _storage = new Dictionary<string, DbContext>();

        /// <summary>
        /// 构造函数
        /// 用于注册HttpApplication.EndRequest事件，关闭数据库连接
        /// </summary>
        /// <param name="app"></param>
        public WebDbContextStorage(HttpApplication app)
        {
            app.EndRequest += (sender, args) =>
            {
                foreach (var dbContext in GetAllDbContexts())
                {
                    if (dbContext.Database.Connection.State == ConnectionState.Open)
                        dbContext.Database.Connection.Close();
                }
            };
        }



        #region Implementation of IDbContextStorage

        /// <summary>
        /// 根据key获取DbContext
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>DbContext</returns>
        public DbContext GetByKey(string key)
        {
            DbContext context;
            return !_storage.TryGetValue(key, out context) ? null : context;
        }

        /// <summary>
        /// 根据Key将DbContext写入Storage
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="dbContext">DbContext</param>
        public void SetByKey(string key, DbContext dbContext)
        {
            _storage.Add(key, dbContext);
        }

        /// <summary>
        /// 获取所有DDbContext
        /// </summary>
        /// <returns>所有DDbContext</returns>
        public IEnumerable<DbContext> GetAllDbContexts()
        {
            return _storage.Values;
        }

        #endregion
    }
 */

}
