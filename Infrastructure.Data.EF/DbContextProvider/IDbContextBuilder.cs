using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Ef.DbContextProvider
{
    /// <summary>
    /// DbContext动态组装器接口
    /// </summary>
    internal interface IDbContextBuilder
    {
        /// <summary>
        /// 创建DbContext
        /// </summary>
        /// <returns></returns>
        DbContext BuildDbContext();
    }
}
