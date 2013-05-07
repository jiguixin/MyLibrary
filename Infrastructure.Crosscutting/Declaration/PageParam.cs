using System;

namespace Infrastructure.Crosscutting.Declaration
{
    /// <summary>
    /// 结构体Pager:封装分页查询的相关参数
    /// </summary>
    /// 
    [Serializable]
    public class PageParam
    {
        /// <summary>
        /// 总页数,作为传出参数
        /// </summary>
        public int Count;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Index;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy;

        /// <summary>
        /// 排序类型 asc / desc
        /// </summary>
        public string OrderType;

        /// <summary>
        /// 页大小
        /// </summary>
        public int Size;

        /// <summary>
        /// 总记录数,作为传出参数
        /// </summary>
        public int Total;

        /// <summary>
        /// 无参数的构造方法
        /// </summary>
        public PageParam()
        {
        }

        /// <summary>
        /// 带参数的构造方法
        /// </summary>
        /// <param name="index">当前页数</param>
        /// <param name="size">页大小+</param>
        public PageParam(int index, int size)
        {
            Index = index;
            Size = size;
        }
    }
}