using System;
using System.IO;
namespace ICCEmbedded.SharpZipLib.Core
{
    /// <summary>
    /// 根据日期来筛选指定文件
    /// </summary>
    public class DateTimeFilter:IScanFilter
    {
        #region var

        readonly DateTime _minTime; 

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtCondition"></param>
        public DateTimeFilter(DateTime dtCondition)
        {
            _minTime = dtCondition;
        }

        public bool IsMatch(string name)
        {
            return File.GetLastAccessTime(name) > _minTime;
        }
    }
}