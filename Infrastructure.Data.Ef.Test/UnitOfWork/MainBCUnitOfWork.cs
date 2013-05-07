//===================================================================================
// Microsoft Developer and Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates;
using Infrastructure.Data.Ef.Test.BankingModule.Aggregates.BankAccountAgg;
using Infrastructure.Data.Ef.Test.UnitOfWork.Mapping;

namespace Infrastructure.Data.Ef.Test.UnitOfWork
{
    using System.Collections.Generic;

    public class MainBCUnitOfWork
        :DbContext,IMainBCUnitOfWork
    {
        #region IMainBCUnitOfWork Members

        IDbSet<Customer> _customers;
        public IDbSet<Customer> Customers
        {
            get 
            {
                if (_customers == null)
                    _customers = base.Set<Customer>();

                return _customers;
            }
        }

        
        IDbSet<BankAccount> _bankAccounts;
        public IDbSet<BankAccount> BankAccounts
        {
            get 
            {
                if (_bankAccounts == null)
                    _bankAccounts = base.Set<BankAccount>();

                return _bankAccounts;
            }
        }

        #endregion

        #region IQueryableUnitOfWork Members

        public IDbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item) 
            where TEntity : class
        {
            //attach and set as unchanged
            base.Entry<TEntity>(item).State = System.Data.EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) 
            where TEntity : class
        {
            //this operation also attach item in object state manager
            base.Entry<TEntity>(item).State = System.Data.EntityState.Modified;
        }
        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if not is attached, attach original and set current values
            base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
            base.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                               {
                                   entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                               });

                }
            } while (saveFailed);

        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
// ReSharper disable AssignNullToNotNullAttribute
            base.ChangeTracker.Entries()
// ReSharper restore AssignNullToNotNullAttribute
                              .ToList()
                              .ForEach(entry => entry.State = System.Data.EntityState.Unchanged);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        #endregion

        #region DbContext Overrides


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Remove unused conventions
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //把IncludeMetadataConvention 移除了就会导致使用DropCreateDatabaseIfModelChanges
            //出错，因为数据库里面没有了。EdmMetadata 表。
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            //dynamically load all configuration
            System.Type configType = typeof(BankAccountEntityTypeConfiguration);   //any of your configuration classes here
            var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new BankAccountEntityTypeConfiguration());


            //Add entity configurations in a structures way using 'TypeConfiguration’ classes
            //modelBuilder.Configurations.Add(new BankAccountEntityTypeConfiguration()); 
        }

        #endregion
    }
}
