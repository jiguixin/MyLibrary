namespace Infrastructure.Data.Seedwork.Declaration
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
    public class TableDescriptionAttribute : DescriptionAttribute
    {
        public TableDescriptionAttribute(string tableName) : base(tableName)
        {
        }
    }
}

