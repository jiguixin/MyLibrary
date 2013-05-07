using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using Infrastructure.Data.Ado.Internal;
using System.Data.Common;

namespace Infrastructure.Data.Ado
{
    /// <summary>Simple and Lightweight Database Executor.</summary>
    public partial class DbExecutor : IDisposable
    {
        #region Var

        readonly IDbConnection connection;
        readonly char parameterSymbol;
        // Transaction
        readonly bool isUseTransaction;
        readonly IsolationLevel isolationLevel;
        IDbTransaction transaction;
        bool isTransactionCompleted = false;

        private DbDataAdapter adapter;
        private DbCommandBuilder builder;
        private DbCommand cmd;
         

        #endregion

        #region ctor
         
        /// <summary>Create standard executor.</summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="parameterSymbol">Command parameter symbol. SqlServer = '@', MySql = '?', Oracle = ':'</param>
        public DbExecutor(IDbConnection connection, char parameterSymbol = '@')
        {
            Contract.Requires<ArgumentNullException>(connection != null);

            parameterSymbol = GetParameterSymbol(connection, parameterSymbol);

            this.connection = connection;
            this.parameterSymbol = parameterSymbol;
            this.isUseTransaction = false;
        }

        /// <summary>Use transaction.</summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="isolationLevel">Transaction IsolationLevel.</param>
        /// <param name="parameterSymbol">Command parameter symbol. SqlServer = '@', MySql = '?', Oracle = ':'</param>
        public DbExecutor(IDbConnection connection, IsolationLevel isolationLevel, char parameterSymbol = '@')
        {
            Contract.Requires<ArgumentNullException>(connection != null);

            parameterSymbol = GetParameterSymbol(connection, parameterSymbol);

            this.connection = connection;
            this.parameterSymbol = parameterSymbol;
            this.isUseTransaction = true;
            this.isolationLevel = isolationLevel;
        }
      
        #endregion
          
        #region Basic ADO.NET DbCommand Wrapper --- public Method

        /// <summary>Executes and returns the data records.</summary>
        /// <param name="query">SQL code.</param>
        /// <param name="parameter">PropertyName parameterized to PropertyName. if null then no use parameter.</param>
        /// <param name="commandType">Command Type.</param>
        /// <param name="commandBehavior">Command Behavior.</param>
        /// <returns>Query results.</returns>
        public IEnumerable<IDataRecord> ExecuteReader(string query, object parameter = null, CommandType commandType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(query));
            Contract.Ensures(Contract.Result<IEnumerable<IDataRecord>>() != null);

            return YieldReaderHelper(query, parameter, commandType, commandBehavior);
        }

        /// <summary>Executes and returns the data records enclosing DynamicDataRecord.</summary>
        /// <param name="query">SQL code.</param>
        /// <param name="parameter">PropertyName parameterized to PropertyName. if null then no use parameter.</param>
        /// <param name="commandType">Command Type.</param>
        /// <param name="commandBehavior">Command Behavior.</param>
        /// <returns>Query results. Result type is DynamicDataRecord.</returns>
        public IEnumerable<dynamic> ExecuteReaderDynamic(string query, object parameter = null, CommandType commandType = CommandType.Text, CommandBehavior commandBehavior = CommandBehavior.Default)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(query));
            Contract.Ensures(Contract.Result<IEnumerable<dynamic>>() != null);

            return YieldReaderDynamicHelper(query, parameter, commandType, commandBehavior);
        }

        /// <summary>Executes and returns the number of rows affected.</summary>
        /// <param name="query">SQL code.</param>
        /// <param name="parameter">PropertyName parameterized to PropertyName. if null then no use parameter.</param>
        /// <param name="commandType">Command Type.</param>
        /// <returns>Rows affected.</returns>
        public int ExecuteNonQuery(string query, object parameter = null, CommandType commandType = CommandType.Text)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(query));

            using (var command = PrepareExecute(query, commandType, parameter))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>Executes and returns the first column, first row.</summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="query">SQL code.</param>
        /// <param name="parameter">PropertyName parameterized to PropertyName. if null then no use parameter.</param>
        /// <param name="commandType">Command Type.</param>
        /// <returns>Query results of first column, first row.</returns>
        public T ExecuteScalar<T>(string query, object parameter = null, CommandType commandType = CommandType.Text)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(query));

            using (var command = PrepareExecute(query, commandType, parameter))
            {
                return (T)command.ExecuteScalar();
            }
        }

        /// <summary>Commit transaction.</summary>
        public void TransactionComplete()
        {
            if (transaction != null)
            {
                transaction.Commit();
                isTransactionCompleted = true;
            }
        }
         

        #endregion

        #region 通过得到DataTable 然后在调用EndCommitTable完成数据库的更新

        #region 匹配使用

        /// <summary>
        /// 通过SQL得到一个DataTable，然后你可以在修改该DataTable，如：增加、修改、删除一些数据
        /// 然后在调用配套方法EndCommitTable,这样有效的提高SQL的性能因为不用查询整张表。        
        /// </summary>
        /// <param name="factory">数据库实例的提供者</param>
        /// <param name="sqlQuery">SQL 查询语句.Eg:select * from Table where SysId=@SysId</param>
        /// <param name="parameter">查询语句后面的参数. Eg:new {SysId=111}</param>
        /// <param name="commandType">CommandType</param>
        /// <returns></returns>
        public DataTable BeginGetTable(DbProviderFactory factory,string sqlQuery,object parameter = null, CommandType commandType = CommandType.Text)
        {
            #region Validation

            Contract.Requires<ArgumentNullException>(factory != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(sqlQuery));

            Contract.Ensures(adapter != null);
            Contract.Ensures(cmd != null);
            Contract.Ensures(builder != null);

            #endregion

            adapter = factory.CreateDataAdapter();
            builder = factory.CreateCommandBuilder();
            cmd = factory.CreateCommand();
            
            DataTable dt = new DataTable();
            using (var command = PrepareExecute(sqlQuery, commandType, parameter))
            { 
                adapter.SelectCommand = (DbCommand)command;
                builder.DataAdapter = adapter;
                 
                adapter.Fill(dt);
            }
            return dt;
        }
          
        public string EndCommitTable(DataTable dtTarget)
        {
            #region Validation

            Contract.Requires<ArgumentNullException>(dtTarget != null);

            Contract.Assert(cmd != null, "调用EndCommitTable之前必须先调用BeginGetTable");
            Contract.Assert(adapter != null, "调用EndCommitTable之前必须先调用BeginGetTable");
            Contract.Assert(builder != null, "调用EndCommitTable之前必须先调用BeginGetTable");
            #endregion

            bool isInsertAI = false;
            try
            {
                isInsertAI = dtTarget.Rows.Count > 0 && dtTarget.Columns.Contains("SysId") &&
                             dtTarget.GetChanges(DataRowState.Added) != null;

                if (isInsertAI)
                {
                    this.cmd.CommandText = string.Format("{0};SELECT @@IDENTITY as [{1}]",
                                                             builder.GetInsertCommand().CommandText, "SysId");

                    this.cmd.CommandType = CommandType.Text;
                    if (transaction !=null)
                        cmd.Transaction = (DbTransaction)transaction;

                    this.cmd.Parameters.Clear();

                    foreach (DbParameter dbParameter in builder.GetInsertCommand().Parameters)
                    {
                        var parm = this.cmd.CreateParameter();
                        parm.DbType = dbParameter.DbType;
                        parm.Direction = dbParameter.Direction;
                        parm.ParameterName = dbParameter.ParameterName;
                        parm.Size = dbParameter.Size;
                        parm.Value = dbParameter.Value;
                        parm.SourceColumn = dbParameter.SourceColumn;
                        this.cmd.Parameters.Add(parm);
                    }

                    adapter.InsertCommand = this.cmd;
                    adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;
                }
                adapter.Update(dtTarget); 
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return ex.Message;
#endif
            }
            finally
            {
                DisposeDbProvider();
            }
            return null;
        }
         
        #endregion
            
        #endregion

        #region 可以得到独立的DataSet,DataTable,并可以调用CommitData 完成对数据库的更新

        public DataTable GetTable(DbProviderFactory factory,string sqlQuery, object parameter = null, CommandType commandType = CommandType.Text)
        {

            DataTable dt = new DataTable();
            using (var command = PrepareExecute(sqlQuery, commandType, parameter))
            {
                using (var adp = factory.CreateDataAdapter())
                {
                    using (var build = factory.CreateCommandBuilder())
                    {
                        build.DataAdapter = adp;

                        adp.SelectCommand = (DbCommand) command;
                        adp.Fill(dt);

                    }
                }  
            }
            return dt; 
        }

        public DataSet GetTables(DbProviderFactory factory, string sqlQuery, object parameter = null, CommandType commandType = CommandType.Text)
        {
            DataSet ds = new DataSet();
            using (var command = PrepareExecute(sqlQuery, commandType, parameter))
            {
                using (var adp = factory.CreateDataAdapter())
                {
                    using (var build = factory.CreateCommandBuilder())
                    {
                        build.DataAdapter = adp;

                        adp.SelectCommand = (DbCommand)command;
                        adp.Fill(ds); 
                    }
                }
            }
            return ds;
        }
         
        public string CommitData(DbProviderFactory factory, DataTable dtTarget, string tableName)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(tableName));
            Contract.Requires<ArgumentNullException>(dtTarget != null);

            bool isInsertAI = false;
            DataTable dtChange = null;

            try
            {
                dtChange = dtTarget.GetChanges();
                isInsertAI = dtTarget.Rows.Count > 0 && dtTarget.Columns.Contains("SysId") &&
                             dtTarget.GetChanges(DataRowState.Added) != null;

                using (var command = PrepareExecute("select * from " + tableName, CommandType.Text, null))
                {
                    using (var tmpCmd = factory.CreateCommand())
                    {
                        if (transaction != null)
                            tmpCmd.Transaction = (DbTransaction)transaction;
                        using (var adp = factory.CreateDataAdapter())
                        {
                            using (var build = factory.CreateCommandBuilder())
                            {
                                adp.SelectCommand = (DbCommand) command;
                                build.DataAdapter = adp;

                                if (isInsertAI)
                                {
                                    tmpCmd.CommandText = string.Format("{0};SELECT @@IDENTITY as [{1}]",
                                                                        build.GetInsertCommand().CommandText, "SysId");
                                     
                                    foreach (DbParameter dbParameter in build.GetInsertCommand().Parameters)
                                    {
                                        var parm = tmpCmd.CreateParameter();
                                        parm.DbType = dbParameter.DbType;
                                        parm.Direction = dbParameter.Direction;
                                        parm.ParameterName = dbParameter.ParameterName;
                                        parm.Size = dbParameter.Size;
                                        parm.Value = dbParameter.Value;
                                        parm.SourceColumn = dbParameter.SourceColumn;
                                        tmpCmd.Parameters.Add(parm);
                                    }

                                    adp.InsertCommand = tmpCmd;
                                    adp.InsertCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;
                                }
                                adp.Update(dtTarget);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return ex.Message;
#endif
            }

            return null;
        }
         
        #endregion
         
        #region Micro-ORM Methods

        /// <summary>Executes and mapping objects by ColumnName - PropertyName.</summary>
        /// <typeparam name="T">Mapping target Class.</typeparam>
        /// <param name="query">SQL code.</param>
        /// <param name="parameter">PropertyName parameterized to PropertyName. if null then no use parameter.</param>
        /// <param name="commandType">Command Type.</param>
        /// <returns>Mapped instances.</returns>
        public IEnumerable<T> Select<T>(string query, object parameter = null, CommandType commandType = CommandType.Text) where T : new()
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(query));
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            var accessors = AccessorCache.Lookup(typeof(T));
            return ExecuteReader(query, parameter, commandType, CommandBehavior.SequentialAccess)
                .Select(dr =>
                {
                    // if T is ValueType then can't set SetValue
                    // must be boxed
                    object result = new T();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        if (dr.IsDBNull(i)) continue;

                        var accessor = accessors[dr.GetName(i)];
                        if (accessor != null && accessor.IsWritable) accessor.SetValueDirect(result, dr[i]);
                    }
                    return (T)result;
                });
        }

        /// <summary>Executes and mapping objects to ExpandoObject. Object is dynamic accessable by ColumnName.</summary>
        /// <param name="query">SQL code.</param>
        /// <param name="parameter">PropertyName parameterized to PropertyName. if null then no use parameter.</param>
        /// <param name="commandType">Command Type.</param>
        /// <returns>Mapped results(dynamic type is ExpandoObject).</returns>
        public IEnumerable<dynamic> SelectDynamic(string query, object parameter = null, CommandType commandType = CommandType.Text)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(query));
            Contract.Ensures(Contract.Result<IEnumerable<dynamic>>() != null);

            return ExecuteReader(query, parameter, commandType, CommandBehavior.SequentialAccess)
                .Select(dr =>
                {
                    IDictionary<string, object> expando = new ExpandoObject();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        var value = dr.IsDBNull(i) ? null : dr.GetValue(i);
                        expando.Add(dr.GetName(i), value);
                    }
                    return expando;
                });
        }
          
        /// <summary>Insert by object's PropertyName.</summary>
        /// <param name="tableName">Target database's table.</param>
        /// <param name="insertItem">Table's column name extracted from PropertyName.</param>
        /// <returns>Rows affected.</returns>
        public int Insert(string tableName, object insertItem)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(tableName));
            Contract.Requires<ArgumentNullException>(insertItem != null);

            var propNames = AccessorCache.Lookup(insertItem.GetType())
                .Where(p => p.IsReadable)
                .ToArray();
            var column = string.Join(", ", propNames.Select(p => p.Name));
            var data = string.Join(", ", propNames.Select(p => parameterSymbol + p.Name));

            var query = string.Format("insert into {0} ({1}) values ({2})", tableName, column, data);

            Contract.Assume(query.Length > 0);
            return ExecuteNonQuery(query, insertItem);
        }

        /// <summary>Update by object's PropertyName.</summary>
        /// <param name="tableName">Target database's table.</param>
        /// <param name="updateItem">Table's column name extracted from PropertyName.</param>
        /// <param name="whereCondition">Where condition extracted from PropertyName.</param>
        /// <returns>Rows affected.</returns>
        public int Update(string tableName, object updateItem, object whereCondition)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(tableName));
            Contract.Requires<ArgumentNullException>(whereCondition != null);
            Contract.Requires<ArgumentNullException>(updateItem != null);

            var update = string.Join(", ", AccessorCache.Lookup(updateItem.GetType())
                .Where(p => p.IsReadable)
                .Select(p => p.Name + " = " + parameterSymbol + p.Name));

            var where = string.Join(" and ", AccessorCache.Lookup(whereCondition.GetType())
                .Select(p => p.Name + " = " + parameterSymbol + "__extra__" + p.Name));

            var query = string.Format("update {0} set {1} where {2}", tableName, update, where);

            Contract.Assume(query.Length > 0);
            using (var command = PrepareExecute(query, CommandType.Text, updateItem, whereCondition))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>Delete by object's PropertyName.</summary>
        /// <param name="tableName">Target database's table.</param>
        /// <param name="whereCondition">Where condition extracted from PropertyName.</param>
        /// <returns>Rows affected.</returns>
        public int Delete(string tableName, object whereCondition)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(tableName));
            Contract.Requires<ArgumentNullException>(whereCondition != null);

            var where = string.Join(" and ", AccessorCache.Lookup(whereCondition.GetType())
                .Select(p => p.Name + " = " + parameterSymbol + p.Name));

            var query = string.Format("delete from {0} where {1}", tableName, where);

            Contract.Assume(query.Length > 0);
            return ExecuteNonQuery(query, whereCondition);
        }

        #endregion

        #region OverWrite
         
        /// <summary>Dispose inner connection.</summary>
        public void Dispose()
        {
            try
            {
                if (transaction != null && !isTransactionCompleted)
                {
                    transaction.Rollback();
                    isTransactionCompleted = true;
                }
            }
            finally
            {
                if (connection != null) connection.Dispose();
                DisposeDbProvider();
            }
        }

      

        #endregion

        #region Helper
         
        IEnumerable<dynamic> YieldReaderDynamicHelper(string query, object parameter, CommandType commandType, CommandBehavior commandBehavior)
        {
            using (var command = PrepareExecute(query, commandType, parameter))
            using (var reader = command.ExecuteReader(commandBehavior))
            {
                var record = new DynamicDataRecord(reader); // reference same reader
                while (reader.Read()) yield return record;
            }
        }


        /// <summary>If connection is not open then open and create command.</summary>
        /// <param name="query">SQL code.</param>
        /// <param name="commandType">Command Type.</param>
        /// <param name="parameter">PropertyName parameterized to PropertyName. if null then no use parameter.</param>
        /// <param name="extraParameter">CommandName set to __extra__PropertyName.</param>
        /// <returns>Setuped IDbCommand.</returns>
        protected IDbCommand PrepareExecute(string query, CommandType commandType, object parameter, object extraParameter = null)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(query));
            Contract.Ensures(Contract.Result<IDbCommand>() != null);

            if (connection.State != ConnectionState.Open) connection.Open();
            if (transaction == null && isUseTransaction) transaction = connection.BeginTransaction(isolationLevel);

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = commandType;

            if (parameter != null)
            {
                foreach (var p in AccessorCache.Lookup(parameter.GetType()))
                {
                    if (!p.IsReadable) continue;

                    Contract.Assume(parameter != null);

                    if (p.GetValueDirect(parameter) is IDataParameter)
                    {
                        command.Parameters.Add(p.GetValueDirect(parameter));
                    }
                    else
                    {
                        var param = command.CreateParameter();
                        param.ParameterName = p.Name;
                        param.Value = p.GetValueDirect(parameter);
                        command.Parameters.Add(param);
                    }
                }
            }
            if (extraParameter != null)
            {
                foreach (var p in AccessorCache.Lookup(extraParameter.GetType()))
                {
                    if (!p.IsReadable) continue;

                    Contract.Assume(extraParameter != null);
                    var param = command.CreateParameter();
                    param.ParameterName = "__extra__" + p.Name;
                    param.Value = p.GetValueDirect(extraParameter);
                    command.Parameters.Add(param);
                }
            }

            if (transaction != null) command.Transaction = transaction;

            return command;
        }

        IEnumerable<IDataRecord> YieldReaderHelper(string query, object parameter, CommandType commandType, CommandBehavior commandBehavior)
        {
            using (var command = PrepareExecute(query, commandType, parameter))
            using (var reader = command.ExecuteReader(commandBehavior))
            {
                while (reader.Read()) yield return reader;
            }
        }

         
        //得到不同数据库的参数符号
        private char GetParameterSymbol(IDbConnection connection, char parameterSymbol)
        {
            switch (connection.ToString())
            {
                case "System.Data.OracleClient.OracleConnection":
                    parameterSymbol = ':';
                    break;
                case "MySql.Data.MySqlClient.MySqlConnection":
                    parameterSymbol = '?';
                    break;
            }
            return parameterSymbol;
        }

        private void DisposeDbProvider()
        {
            if (cmd != null) cmd.Dispose();
            if (adapter != null) adapter.Dispose();
            if (builder != null) builder.Dispose();
        }
        #endregion

    }
}