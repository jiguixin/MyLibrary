using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Crosscutting.Declaration
{ 
    /// <summary>
    /// 枚举,生成缩略图模式
    /// </summary>
    public enum ThumbnailMod : byte
    {
        /// <summary>
        /// HW
        /// </summary>
        HW,
        /// <summary>
        /// W
        /// </summary>
        W,
        /// <summary>
        /// H
        /// </summary>
        H,
        /// <summary>
        /// Cut
        /// </summary>
        Cut
    }
    #region 返回时间类型枚举
    /// <summary>
    /// 关于返回值形式的枚举
    /// <see cref="Infrastructure.Crosscutting.Utility.CommomHelper.TimeHelper"/>
    /// </summary>
    public enum DiffResultFormat
    {
        /// <summary>
        /// 年数和月数
        /// </summary>
        yymm,
        /// <summary>
        /// 年数
        /// </summary>
        yy,
        /// <summary>
        /// 月数
        /// </summary>
        mm,
        /// <summary>
        /// 天数
        /// </summary>
        dd,
    }
    #endregion


    /// <summary>
    /// Represents a read table mode for ExcelHelper.ReadTable method
    /// </summary>
    public enum ExcelHelperReadTableMode
    {
        /// <summary>
        /// Read rows from all filled excel worksheet cells
        /// </summary>
        ReadFromWorkSheet,
        /// <summary>
        /// Read rows only from named range
        /// </summary>
        ReadFromNamedRange
    }
   
}
