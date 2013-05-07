using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Ef.DbContextProvider
{
    public class DbContextStorageBase:IDbContextStorage
    {
        private Dictionary<string, DbContext> _storage = new Dictionary<string, DbContext>();

        #region Implementation of IDbContextStorage

        /// <summary>
        /// 根据KEY获取DbContext
        /// </summary>
        /// <param name="key">KEY</param>
        /// <returns>DbContext</returns>
        public DbContext GetByKey(string key)
        {
            DbContext dbContext;
            return _storage.TryGetValue(key, out dbContext) ? dbContext : null;
        }

        /// <summary>
        /// 根据KEY保存DbContext到仓库
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="context">DbContext</param>
        public void SetByKey(string key, DbContext context)
        {
            _storage.Add(key, context);
        }

        /// <summary>
        /// 获取所有DbContext
        /// </summary>
        /// <returns>DbContext列表</returns>
        public IEnumerable<DbContext> GetAll()
        {
            return _storage.Values;
        }

        #endregion
    }
}
