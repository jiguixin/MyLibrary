
using Domain.Seedwork.DomainModel;
using Infrastructure;

namespace Domain.Seedwork.PersistenceModel
{
    public interface IObjectCollection<TObject, TObjectId> : IPersistableCollection where TObject : class, IObject<TObjectId>
    {
        TObject Get(TObjectId id);
        void Add(TObject obj);
        void Remove(TObject obj);
    }
}
