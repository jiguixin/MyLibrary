using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Infrastructure.Crosscutting.Declaration;

namespace Infrastructure.Crosscutting.Utility.CommomHelper
{
    /// <summary>
    /// The SqlHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public abstract class SqlHelper
    {
        //Database connection strings
        public static readonly string ConnStr =
            string.Format("server={0};database={1};uid={2};password={3};",
                          ConfigurationManager.AppSettings["Server"],
                          ConfigurationManager.AppSettings["Database"],
                          ConfigurationManager.AppSettings["UserId"],
                          ConfigurationManager.AppSettings["Password"]);

        // Hashtable to store cached parameters
        private static readonly Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText,
                                          params SqlParameter[] commandParameters)
        {
            try
            {
                var cmd = new SqlCommand();

                using (var conn = new SqlConnection(connectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText,
                                          params SqlParameter[] commandParameters)
        {
            var cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText,
                                          params SqlParameter[] commandParameters)
        {
            var cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText,
                                                  params SqlParameter[] commandParameters)
        {
            var cmd = new SqlCommand();
            var conn = new SqlConnection(connectionString);
            cmd.CommandTimeout = 180;
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText,
                                           params SqlParameter[] commandParameters)
        {
            var cmd = new SqlCommand();

            using (var connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText,
                                           params SqlParameter[] commandParameters)
        {
            var cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }


        /// <summary>
        /// 执行插入操作
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        /// <example>
        /// </example>
        public static int Insert(string connectionString, SqlCreator creator)
        {
            string sql = creator.GetInsertSql();
            var par = new SqlParameter[creator.Columns.Length + 1];
            par[0] = MakeOutputParam("@Id", SqlDbType.Int, 4);
            for (int i = 0; i < creator.Columns.Length; i++)
            {
                Column parm = creator.Columns[i];
                par[i + 1] = MakeInputParam(parm.ParameterName, parm.DbType, parm.Size, parm.Value);
            }

            if (ExecuteNonQuery(connectionString, CommandType.Text, sql, par) > 0)
            {
                try
                {
                    return Convert.ToInt32(par[0].Value);
                }
                catch
                {
                    return -1;
                }
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// 把列转变为SqlParameter其中最后有@OrderId转出参数，可以用于存储过程
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static SqlParameter[] GetParameterByColumns(Column[] columns)
        {
            var par = new SqlParameter[columns.Length + 1];
            par[par.Length - 1] = MakeOutputParam("@OrderId", SqlDbType.Int, 4);
            for (int i = 0; i < columns.Length; i++)
            {
                Column parm = columns[i];
                par[i] = MakeInputParam(parm.ParameterName, parm.DbType, parm.Size, parm.Value);
            }

            return par;
        }


        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static int Update(string connectionString, SqlCreator creator)
        {
            string sql = creator.GetUpdateSql();
            var par = new SqlParameter[creator.Columns.Length + creator.WhereColumns.Length];
            for (int i = 0; i < creator.Columns.Length; i++)
            {
                Column parm = creator.Columns[i];
                par[i] = MakeInputParam(parm.ParameterName, parm.DbType, parm.Size, parm.Value);
            }

            for (int i = 0; i < creator.WhereColumns.Length; i++)
            {
                Column parm = creator.WhereColumns[i];
                par[i + creator.Columns.Length] = MakeInputParam(parm.ParameterName, parm.DbType, parm.Size, parm.Value);
            }

            return ExecuteNonQuery(connectionString, CommandType.Text, sql, par);
        }


        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="creator">SqlCreator对象</param>
        /// <returns></returns>
        public static int Delete(string connectionString, SqlCreator creator)
        {
            string sql = creator.GetDeleteSql();
            var par = new SqlParameter[creator.WhereColumns.Length];
            for (int i = 0; i < creator.WhereColumns.Length; i++)
            {
                Column parm = creator.WhereColumns[i];
                par[i] = MakeInputParam(parm.ParameterName, parm.DbType, parm.Size, parm.Value);
            }

            return ExecuteNonQuery(connectionString, CommandType.Text, sql, par);
        }


        /// <summary>
        /// 执行选择操作，返回SqlDataReader
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static SqlDataReader SelectReader(string connectionString, SqlCreator creator)
        {
            string sql = creator.GetSelectSql();
            var par = new SqlParameter[creator.WhereColumns.Length];
            for (int i = 0; i < creator.WhereColumns.Length; i++)
            {
                Column parm = creator.WhereColumns[i];
                par[i] = MakeInputParam(parm.ParameterName, parm.DbType, parm.Size, parm.Value);
            }

            return ExecuteReader(connectionString, CommandType.Text, sql, par);
        }


        /// <summary>
        /// 执行选择操作，使用通用存储过程
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="condition"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static SqlDataReader SelectReader(string connectionString, string condition,
                                                 SqlCreator creator, PageParam page, out SqlParameter count,
                                                 out SqlParameter total)
        {
            if (string.IsNullOrEmpty(creator.PrimaryKey))
            {
                throw new Exception("必须为Creator对象指定PrimaryKey属性");
            }

            string orderBy = "";
            if (!string.IsNullOrEmpty(page.OrderBy))
            {
                orderBy = page.OrderBy + " " + page.OrderType;
            }

            var par = new[]
                          {
                              MakeOutputParam("@PageCount", SqlDbType.Int, 4),
                              MakeOutputParam("@TotalRecords", SqlDbType.Int, 4),
                              MakeInputParam("@TableName", SqlDbType.VarChar, 255, creator.TableName),
                              MakeInputParam("@PrimaryKey", SqlDbType.VarChar, 255, creator.PrimaryKey),
                              MakeInputParam("@Condition", SqlDbType.VarChar, 2000, condition),
                              MakeInputParam("@OrderBy", SqlDbType.VarChar, 500, orderBy),
                              MakeInputParam("@GroupBy", SqlDbType.VarChar, 255, creator.GroupBy),
                              MakeInputParam("@PageIndex", SqlDbType.Int, 4, page.Index),
                              MakeInputParam("@PageSize", SqlDbType.Int, 4, page.Size)
                          };

            count = par[0];
            total = par[1];

            return ExecuteReader(connectionString, CommandType.StoredProcedure, "SingleTableCommonPager", par);
        }


        /// <summary>
        /// 判断是否存在一条记录
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="creator">判断条件</param>
        /// <returns></returns>
        public static bool Exists(string connectionString, SqlCreator creator)
        {
            creator.Columns = new[]
                                  {
                                      new Column("count(1)")
                                  };


            var par = new SqlParameter[creator.WhereColumns.Length];
            for (int i = 0; i < creator.WhereColumns.Length; i++)
            {
                Column parm = creator.WhereColumns[i];
                par[i] = MakeInputParam(parm.ParameterName, parm.DbType, parm.Size, parm.Value);
            }


            object obj = ExecuteScalar(connectionString, CommandType.Text,
                                       creator.GetSelectSql(),
                                       par);

            int count = int.Parse(obj.ToString());

            return count > 0;
        }


        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        public static DataTable GetTable(string connectionString, CommandType cmdType, string cmdText,
                                         params SqlParameter[] commandParameters)
        {
            var dt = new DataTable();
            var cmd = new SqlCommand();
            using (var conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                var da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            var cachedParms = (SqlParameter[]) parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            var clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter) ((ICloneable) cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType,
                                           string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    if (parm != null)
                    {
                        //对日期屏蔽错误
                        if (parm.DbType == DbType.DateTime)
                        {
                            DateTime datetime = parm.Value.ToDateTime();
                            if (datetime < new DateTime(1900, 1, 1))
                            {
                                datetime = new DateTime(1900, 1, 1);
                                parm.Value = datetime;
                            }
                        }

                        cmd.Parameters.Add(parm);
                    }
                }
            }
        }


        /// <summary>
        /// 创建输入参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static SqlParameter MakeInputParam(string paramName, SqlDbType dbType, int size, object value)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }

            if (value.GetType() == typeof (string) && string.IsNullOrEmpty(value.ToString()))
            {
                value = DBNull.Value;
            }

            var par = new SqlParameter(paramName, dbType);
            if (size > 0)
            {
                par.Size = size;
            }
            par.Direction = ParameterDirection.Input;
            par.Value = value;
            return par;
        }


        /// <summary>
        /// 创建输出参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static SqlParameter MakeOutputParam(string paramName, SqlDbType dbType, int size)
        {
            var par = new SqlParameter(paramName, dbType, size);
            par.Direction = ParameterDirection.Output;
            return par;
        }
    }

    /// <summary>
    /// 封装构建SQL语句和其参数的类
    /// </summary>
    public class SqlCreator
    {
        public Column[] Columns = new Column[] {};
        public string GroupBy = "";
        public string OrderBy = "";
        public string PrimaryKey = "";
        public string TableName = "";
        public bool TableNameIsSubQuery; //表名是否为一个子查询。若为子查询，则不能在两端加 [] 符号
        public Column[] WhereColumns = new Column[] {};

        public SqlCreator(string tableName)
        {
            TableName = "[" + tableName + "]";
        }

        public SqlCreator(string tableName, Column[] columns)
        {
            TableName = "[" + tableName + "]";
            Columns = columns;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columns">列名称 , 若为查询。则将该参数值为 null</param>
        /// <param name="whereColumns">条件</param>
        public SqlCreator(string tableName, Column[] columns, Column[] whereColumns)
        {
            TableName = "[" + tableName + "]";
            Columns = columns;
            WhereColumns = whereColumns;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columns">列名称 , 若为查询。则将该参数值为 null</param>
        /// <param name="whereColumns">条件</param>
        /// <param name="tableNameIsSubQuery">表名是否为一个子查询</param>
        public SqlCreator(string tableName, Column[] columns, Column[] whereColumns, bool tableNameIsSubQuery)
        {
            TableNameIsSubQuery = tableNameIsSubQuery;
            if (TableNameIsSubQuery)
            {
                TableName = tableName;
            }
            else
            {
                TableName = "[" + tableName + "]";
            }

            Columns = columns;
            WhereColumns = whereColumns;
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="columns">列名称</param>
        /// <param name="where">条件</param>
        public SqlCreator(string tableName, Column[] columns, Column where)
        {
            TableName = "[" + tableName + "]";
            Columns = columns;
            WhereColumns = new Column[1];
            WhereColumns[0] = where;
        }


        /// <summary>
        /// 获取插入SQL语句
        /// </summary>
        /// <returns></returns>
        public string GetInsertSql()
        {
            var s = new StringBuilder();
            s.Append("insert into " + TableName + "(");
            for (int i = 0; i < Columns.Length - 1; i++)
            {
                s.Append(Columns[i].ColumnName + ",");
            }
            s.Append(Columns[Columns.Length - 1].ColumnName);
            s.Append(")");
            s.Append("values(");
            for (int i = 0; i < Columns.Length - 1; i++)
            {
                s.Append(Columns[i].ParameterName + ",");
            }
            s.Append(Columns[Columns.Length - 1].ParameterName);

            s.Append(")");
            s.Append(" set @Id=@@identity");
            return s.ToString();
        }

        /// <summary>
        /// 获取更新SQL语句
        /// </summary>
        /// <returns></returns>
        public string GetUpdateSql()
        {
            var s = new StringBuilder();
            s.Append("update " + TableName + " set ");
            for (int i = 0; i < Columns.Length - 1; i++)
            {
                s.Append(Columns[i].ColumnName + "=" + Columns[i].ParameterName + ",");
            }
            s.Append(Columns[Columns.Length - 1].ColumnName + "=" + Columns[Columns.Length - 1].ParameterName);

            if (WhereColumns.Length > 0)
            {
                s.Append(" where ");

                for (int i = 0; i < WhereColumns.Length - 1; i++)
                {
                    s.Append(WhereColumns[i].ColumnName + "=" + WhereColumns[i].ParameterName + " and ");
                }

                s.Append(WhereColumns[WhereColumns.Length - 1].ColumnName + "=" +
                         WhereColumns[WhereColumns.Length - 1].ParameterName);
            }
            return s.ToString();
        }


        /// <summary>
        /// 获得删除数据的Sql语句
        /// </summary>
        /// <returns>返回字符串，改字符串为执行删除操作的语句</returns>
        public string GetDeleteSql()
        {
            var s = new StringBuilder();
            s.Append("delete from " + TableName + "");
            if (WhereColumns.Length > 0)
            {
                s.Append(" where ");

                for (int i = 0; i < WhereColumns.Length - 1; i++)
                {
                    s.Append(WhereColumns[i].ColumnName + "=" + WhereColumns[i].ParameterName + " and ");
                }

                s.Append(WhereColumns[WhereColumns.Length - 1].ColumnName + "=" +
                         WhereColumns[WhereColumns.Length - 1].ParameterName);
            }
            return s.ToString();
        }


        /// <summary>
        /// 获取查询SQL语句
        /// </summary>
        /// <returns></returns>
        public string GetSelectSql()
        {
            var s = new StringBuilder();
            s.Append("select ");

            //若选择列为为空，则选择所有列
            if (Columns != null && Columns.Length > 0)
            {
                for (int i = 0; i < Columns.Length - 1; i++)
                {
                    s.Append(Columns[i].ColumnName + ",");
                }
                s.Append(Columns[Columns.Length - 1].ColumnName);
            }
            else
            {
                s.Append(" * ");
            }

            s.Append(" from " + TableName);

            if (WhereColumns != null && WhereColumns.Length > 0)
            {
                s.Append(" where ");

                for (int i = 0; i < WhereColumns.Length - 1; i++)
                {
                    s.Append(WhereColumns[i].ColumnName + "=" + WhereColumns[i].ParameterName + " and ");
                }

                s.Append(WhereColumns[WhereColumns.Length - 1].ColumnName + "=" +
                         WhereColumns[WhereColumns.Length - 1].ParameterName);
            }

            if (!string.IsNullOrEmpty(OrderBy))
            {
                s.Append(" order by " + OrderBy);
            }

            return s.ToString();
        }
    }

    /// <summary>
    /// 数据库列
    /// </summary>
    public class Column
    {
        public string ColumnName;
        public SqlDbType DbType;
        public ParameterDirection Direction;
        public string ParameterName;
        public int Size;
        public object Value;


        /// <summary>
        /// 只针对要Select的列
        /// </summary>
        /// <param name="columnName"></param>
        public Column(string columnName)
        {
            //对于一些特殊的列,不加[]符号,如count, sum等
            bool addBracket = true;
            if (columnName.ToLower().IndexOf("count(") > -1)
            {
                addBracket = false;
            }

            if (addBracket)
            {
                ColumnName = "[" + columnName + "]";
            }
            else
            {
                ColumnName = columnName;
            }
        }

        /// <summary>
        /// 输入参数
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        public Column(string columnName, string parameterName, SqlDbType dbType, int size, object value)
        {
            Direction = ParameterDirection.Input;
            ColumnName = "[" + columnName + "]";
            ParameterName = parameterName;
            DbType = dbType;
            Size = size;
            Value = value;
        }


        /// <summary>
        /// 创建输入参数，参数名称为 @加上列名
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        public Column(string columnName, SqlDbType dbType, int size, object value)
        {
            Direction = ParameterDirection.Input;
            ColumnName = "[" + columnName + "]";
            ParameterName = "@" + columnName;
            DbType = dbType;
            Size = size;
            Value = value;
        }
    }
}