using System.Collections.Generic;
using System.Transactions;

namespace Domain.Seedwork.PersistenceModel
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private Variables

        public List<IPersistableCollection> PersistableCollections { get; private set; }

        #endregion

        #region Constructors

        public UnitOfWork()
        {
            PersistableCollections = new List<IPersistableCollection>();
        }

        #endregion

        #region IUnitOfWork Members

        public void RegisterPersistableCollection<TPersistableCollection>(TPersistableCollection collection) where TPersistableCollection : class, IPersistableCollection
        {
            if (!PersistableCollections.Contains(collection))
            {
                PersistableCollections.Add(collection);
            }
        }
        public virtual void SubmitChanges()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (IPersistableCollection persistableCollection in PersistableCollections)
                {
                    persistableCollection.PersistChanges();
                }
                transaction.Complete();
            }
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
        }

        #endregion
    }
}
