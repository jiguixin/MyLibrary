namespace Infrastructure.Data.Seedwork.Declaration
{
    //using Ciloci.Flee;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class SchemaHelper
    {
        public static void ApplySchema<T>(KeyValuePair<Type, string[]> current, string baseMethodName, string modifyMethodName)
        {
            Debug.Assert(!string.IsNullOrEmpty(baseMethodName));
            foreach (string str in current.Value)
            {
                if (SchemaTables.Tables.Contains(str))
                {
                    if (typeof(T).GetMethod(baseMethodName) != null)
                    {
                        typeof(T).InvokeMember(baseMethodName, BindingFlags.InvokeMethod, null, null, new object[] { current.Key, SchemaTables.Tables[str], Evaluators });
                    }
                    if (!(string.IsNullOrEmpty(modifyMethodName) || (current.Key.GetMethod(modifyMethodName) == null)))
                    {
                        current.Key.InvokeMember(modifyMethodName, BindingFlags.InvokeMethod, null, null, null);
                    }
                }
            }
        }

        public static string DBColumnName(string str)
        {
            int index = str.IndexOf('[');
            if (index < 0)
            {
                return null;
            }
            int num2 = str.IndexOf(']', index + 1);
            if (num2 < 0)
            {
                return null;
            }
            return str.Substring(index + 1, (num2 - index) - 1);
        }

        public static void Initialize(SqlConnection cnn, string decimalSP)
        {
            ModelFinder.Initialize();
            Evaluators = new Dictionary<string, PropertySchema>();
            SchemaTables = new DataSet();
            ReadDBSchema(cnn, decimalSP);
        }

        private static void ReadDBSchema(SqlConnection cnn, string decimalSP)
        {
            //string name = null;
            //string str = null;
            //string key = null;
            //string str4 = null;
            //SqlDataAdapter adapter = new SqlDataAdapter(null, cnn);
            //DataColumn column = null;
            //ExpressionContext context = null;
            //IDynamicExpression expression = null;
            //DataTable dataTable = null;
            //DataTable table2 = null;
            //DataTable table3 = null;
            //DataTable table4 = null;
            //StringBuilder builder = new StringBuilder();
            //StringBuilder builder2 = new StringBuilder();
            //DataRow[] rowArray = null;
            //Debug.Assert(cnn.State == ConnectionState.Open);
            //dataTable = new DataTable();
            //adapter.SelectCommand.CommandText = "SELECT name, id FROM sys.sysobjects WHERE type='U' ORDER BY name";
            //adapter.Fill(dataTable);
            //foreach (DataRow row in dataTable.Rows)
            //{
            //    builder.Append(string.Format("SELECT * FROM {0};", row[0]));
            //    if (builder2.Length > 0)
            //    {
            //        builder2.Append(",");
            //    }
            //    builder2.Append("'");
            //    builder2.Append((string) row[0]);
            //    builder2.Append("'");
            //}
            //adapter.SelectCommand.CommandText = builder.ToString();
            //adapter.FillSchema(SchemaTables, SchemaType.Source);
            //if (!string.IsNullOrEmpty(decimalSP))
            //{
            //    table2 = new DataTable();
            //    adapter.SelectCommand.CommandText = decimalSP;
            //    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            //    adapter.Fill(table2);
            //}
            //table3 = new DataTable();
            //adapter.SelectCommand.CommandType = CommandType.Text;
            //adapter.SelectCommand.CommandText = string.Format("SELECT sys.syscomments.text AS ConstraintText, sys.sysobjects.parent_obj AS TableId FROM sys.syscomments INNER JOIN sys.sysobjects ON Sys.syscomments.id=sys.sysobjects.id WHERE sys.sysobjects.type='C' AND sys.sysobjects.parent_obj IN (SELECT id FROM sys.sysobjects WHERE name IN ({0})) ORDER BY TableId", builder2.ToString());
            //adapter.Fill(table3);
            //table4 = new DataTable();
            //adapter.SelectCommand.CommandText = string.Format("SELECT t.id AS TableId, sys.syscolumns.name AS ColumnName, sys.syscomments.text AS DefaultValue FROM sys.sysobjects INNER JOIN sys.syscolumns ON sys.sysobjects.id = sys.syscolumns.cdefault INNER JOIN sysobjects t ON sys.syscolumns.id = t.id INNER JOIN sys.syscomments ON sys.syscomments.id = sys.sysobjects.id WHERE sys.sysobjects.xtype = 'd' AND t.name IN ({0})", builder2.ToString());
            //adapter.Fill(table4);
            //Debug.WriteLine(string.Empty);
            //Debug.WriteLine(string.Empty);
            //Debug.WriteLine("******************装载数据库constrain/default value开始*********************");
            //for (int i = 0; i < SchemaTables.Tables.Count; i++)
            //{
            //    SchemaTables.Tables[i].TableName = (string) dataTable.Rows[i]["name"];
            //    rowArray = table3.Select(string.Format("TableId={0}", dataTable.Rows[i]["id"]));
            //    foreach (DataRow row in rowArray)
            //    {
            //        str = (string) row["ConstraintText"];
            //        if (string.IsNullOrEmpty(name = DBColumnName(str)))
            //        {
            //            Debug.WriteLine(string.Format("找不到{0}的column name", str));
            //        }
            //        else if (!SchemaTables.Tables[i].Columns.Contains(name))
            //        {
            //            Debug.WriteLine(string.Format("{0}没有{1}字段定义", SchemaTables.Tables[i].TableName, name));
            //        }
            //        else
            //        {
            //            column = SchemaTables.Tables[i].Columns[name];
            //            str = str.Replace("[", string.Empty).Replace("]", string.Empty);
            //            context = new ExpressionContext();
            //            context.Variables.DefineVariable(name, column.DataType);
            //            try
            //            {
            //                expression = context.CompileDynamic(str);
            //            }
            //            catch (ExpressionCompileException)
            //            {
            //                goto Label_03D0;
            //            }
            //            Evaluators.Add(string.Format("{0}_{1}", SchemaTables.Tables[i].TableName, name), new PropertySchema { Context = context, IExpr = expression });
            //        Label_03D0:;
            //        }
            //    }
            //    rowArray = table4.Select(string.Format("TableId={0}", dataTable.Rows[i]["id"]));
            //    foreach (DataRow row in rowArray)
            //    {
            //        key = string.Format("{0}_{1}", SchemaTables.Tables[i].TableName, row["ColumnName"]);
            //        str4 = ((string) row["DefaultValue"]).Replace("(", string.Empty).Replace(")", string.Empty);
            //        if (!Evaluators.ContainsKey(key))
            //        {
            //            Evaluators.Add(key, new PropertySchema { DefaultValueString = str4 });
            //        }
            //        else
            //        {
            //            Evaluators[key].DefaultValueString = str4;
            //        }
            //    }
            //    if (table2 != null)
            //    {
            //        rowArray = table2.Select(string.Format("TableName='{0}'", SchemaTables.Tables[i].TableName));
            //        foreach (DataRow row in rowArray)
            //        {
            //            key = string.Format("{0}_{1}", SchemaTables.Tables[i].TableName, row["ColumnName"]);
            //            if (!Evaluators.ContainsKey(key))
            //            {
            //                Evaluators.Add(key, new PropertySchema { Capacity = new byte?((byte) row["Capacity"]), Scale = new byte?((byte) row["Scale"]) });
            //            }
            //            else
            //            {
            //                Evaluators[key].Capacity = new byte?((byte) row["Capacity"]);
            //                Evaluators[key].Scale = new byte?((byte) row["Scale"]);
            //            }
            //        }
            //    }
            //}
            //Debug.WriteLine("******************装载数据库constraint/default value完成*********************");
            //Debug.WriteLine(" ");
            //adapter.Dispose();
        }

        public static void UnLoad()
        {
            ModelFinder.UnLoad();
            Evaluators.Clear();
            SchemaTables.Dispose();
        }

        public static Dictionary<string, PropertySchema> Evaluators
        {

            get; private set;
        }

        public static DataSet SchemaTables
        {

            get; private set;
        }
    }
}

