namespace Infrastructure.Data.Seedwork.Declaration
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited=false, AllowMultiple=false)]
    public class ColumnDescriptionAttribute : DescriptionAttribute
    {
        public ColumnDescriptionAttribute(string columnName) : base(columnName)
        {
        }

        public ColumnDescriptionAttribute(string tableName, string columnName) : base(tableName + "." + columnName)
        {
            this.TableName = tableName;
            this.ColumnName = columnName;
        }

        public string ColumnName { get; private set; }

        public string TableName { get; private set; }
    }
}

