using System;
using System.Data;

namespace Infrastructure.Data.Core
{
    public interface IUnitOfWork: IDisposable
    {
        bool IsInTransaction { get; }

        void SaveChanges();
         
        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void RollBackTransaction();

        void CommitTransaction();


    }
}