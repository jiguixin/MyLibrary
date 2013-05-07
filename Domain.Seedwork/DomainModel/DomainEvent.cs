using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Seedwork.DomainModel
{
    #region Domain Event

    public interface IDomainEvent
    {
        IList<object> Results { get; }
        T GetTypedResult<T>();
        IList<T> GetTypedResults<T>();
    }
    public class DomainEvent : IDomainEvent
    {
        public DomainEvent() { Results = new List<object>(); }
        public IList<object> Results { get; private set; }

        public T GetTypedResult<T>()
        {
            var filteredResults = GetTypedResults<T>();
            if (filteredResults.Count() > 0)
            {
                return filteredResults[0];
            }
            return default(T);
        }
        public IList<T> GetTypedResults<T>()
        {
            return Results.OfType<T>().ToList();
        }
    }

    #endregion

    #region Get Object Event

    public class GetObjectEvent<TObject, TObjectId> : DomainEvent where TObject : class, IObject<TObjectId>
    {
        public GetObjectEvent(TObjectId domainObjectId)
        {
            this.ObjectId = domainObjectId;
        }
        public TObjectId ObjectId { get; private set; }
    }

    #endregion

    #region Add Object Events

    public class PreAddObjectEvent<TObject> : DomainEvent where TObject : class
    {
        public PreAddObjectEvent(TObject obj)
        {
            this.Object = obj;
        }
        public TObject Object { get; private set; }
    }
    public class AddObjectEvent<TObject> : DomainEvent where TObject : class
    {
        public AddObjectEvent(TObject obj)
        {
            this.Object = obj;
        }
        public TObject Object { get; private set; }
    }
    public class ObjectAddedEvent<TObject> : DomainEvent where TObject : class
    {
        public ObjectAddedEvent(TObject obj)
        {
            this.Object = obj;
        }
        public TObject Object { get; private set; }
    }

    #endregion

    #region Remove Object Events

    public class PreRemoveObjectEvent<TObject> : DomainEvent where TObject : class
    {
        public PreRemoveObjectEvent(TObject obj)
        {
            this.Object = obj;
        }
        public TObject Object { get; private set; }
    }
    public class RemoveObjectEvent<TObject> : DomainEvent where TObject : class
    {
        public RemoveObjectEvent(TObject obj)
        {
            this.Object = obj;
        }
        public TObject Object { get; private set; }
    }
    public class ObjectRemovedEvent<TObject> : DomainEvent where TObject : class
    {
        public ObjectRemovedEvent(TObject obj)
        {
            this.Object = obj;
        }
        public TObject Object { get; private set; }
    }

    #endregion

    #region Find Object Events

    public abstract class FindObjectsEvent<TObject> : DomainEvent where TObject : class
    {
    }

    #endregion
}
