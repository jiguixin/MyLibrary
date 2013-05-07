using System;

namespace Domain.Seedwork.PersistenceModel
{
    public interface IUnitOfWork : IDisposable
    {
        void RegisterPersistableCollection<TPersistableCollection>(TPersistableCollection collection) where TPersistableCollection : class, IPersistableCollection;
        void SubmitChanges();
    }
}
