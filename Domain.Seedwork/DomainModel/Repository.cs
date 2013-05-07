using System.Collections.Generic;
using System.Linq;
using Infrastructure.Crosscutting.Declaration;

namespace Domain.Seedwork.DomainModel
{
    public sealed class Repository
    {
        #region Get Object

        public static TObject Get<TObject, TObjectId>(TObjectId domainObjectId) where TObject : class, IObject<TObjectId>
        {
            var queryObjectEvent = new GetObjectEvent<TObject, TObjectId>(domainObjectId);
            EventPublisher.Publish(queryObjectEvent);
            return queryObjectEvent.GetTypedResult<TObject>();
        }

        #endregion

        #region Add Object

        internal static object Add(object obj)
        {
            var preAddObjectEvent = typeof(PreAddObjectEvent<>).MakeGenericType(obj.GetType()).GetConstructors()[0].Invoke(new object[] { obj }) as IDomainEvent;
            var addObjectEvent = typeof(AddObjectEvent<>).MakeGenericType(obj.GetType()).GetConstructors()[0].Invoke(new object[] { obj }) as IDomainEvent;
            var objectAddedEvent = typeof(ObjectAddedEvent<>).MakeGenericType(obj.GetType()).GetConstructors()[0].Invoke(new object[] { obj }) as IDomainEvent;

            EventPublisher.Publish(preAddObjectEvent);
            EventPublisher.Publish(addObjectEvent);
            EventPublisher.Publish(objectAddedEvent);

            return obj;
        }
        public static TObject Add<TObject>(TObject obj) where TObject : class
        {
            EventPublisher.Publish(new PreAddObjectEvent<TObject>(obj));
            EventPublisher.Publish(new AddObjectEvent<TObject>(obj));
            EventPublisher.Publish(new ObjectAddedEvent<TObject>(obj));
            return obj;
        }
        public static void Add<TObject>(params TObject[] objects) where TObject : class
        {
            objects.ForEach(obj => Add(obj));
        }
        public static void Add<TObject>(IEnumerable<TObject> objects) where TObject : class
        {
            objects.ForEach(obj => Add(obj));
        }

        #endregion

        #region Remove Object

        public static void Remove<TObject, TObjectId>(TObjectId id) where TObject : class, IObject<TObjectId>
        {
            var obj = Get<TObject, TObjectId>(id);
            if (obj != null)
            {
                Remove(obj);
            }
        }
        public static void Remove<TObject>(TObject obj) where TObject : class
        {
            EventPublisher.Publish(new PreRemoveObjectEvent<TObject>(obj));
            EventPublisher.Publish(new RemoveObjectEvent<TObject>(obj));
            EventPublisher.Publish(new ObjectRemovedEvent<TObject>(obj));
        }
        public static void Remove<TObject>(params TObject[] objects) where TObject : class
        {
            objects.ForEach(obj => Remove(obj));
        }
        public static void Remove<TObject>(IEnumerable<TObject> objects) where TObject : class
        {
            objects.ForEach(obj => Remove(obj));
        }

        #endregion

        #region Find Objects

        public static List<TObject> Find<TObject>(FindObjectsEvent<TObject> findObjectsEvent) where TObject : class
        {
            EventPublisher.Publish(findObjectsEvent);
            var result = findObjectsEvent.GetTypedResult<IEnumerable<TObject>>();
            if (result != null)
            {
                return result.ToList();
            }
            return new List<TObject>();
        }

        #endregion
    }
}
