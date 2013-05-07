using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Seedwork.DomainModel;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;

namespace Domain.Seedwork.PersistenceModel
{
    public abstract class ObjectCollection<TObject, TObjectId> : IObjectCollection<TObject, TObjectId> where TObject : class, IObject<TObjectId>
    {
        #region Private Variables

        private List<TrackObject<TObject, TObjectId>> trackingObjectList = new List<TrackObject<TObject, TObjectId>>();

        #endregion

        #region Constructors

        public ObjectCollection()
        {
            var unitOfWork = InstanceLocator.Current.GetInstance<IUnitOfWork>();
            if (unitOfWork != null)
            {
                unitOfWork.RegisterPersistableCollection(this);
            }
        }

        #endregion

        #region Event Handlers

        protected TObject Handle(GetObjectEvent<TObject, TObjectId> evnt)
        {
            return Get(evnt.ObjectId);
        }
        protected void Handle(AddObjectEvent<TObject> evnt)
        {
            Add(evnt.Object);
        }
        protected void Handle(RemoveObjectEvent<TObject> evnt)
        {
            Remove(evnt.Object);
        }

        #endregion

        #region IObjectCollection<TObject, TObjectId> Members

        public virtual TObject Get(TObjectId id)
        {
            //Check whether the obj is removed.
            if (IsObjectRemoved(id))
            {
                return null;
            }

            //Try to get the obj from memory.
            var trackingObject = (from trackingObj in trackingObjectList where Equals(trackingObj.CurrentValue.Id, id) && trackingObj.Status != ObjectStatus.Removed select trackingObj).FirstOrDefault();
            if (trackingObject != null)
            {
                return trackingObject.CurrentValue;
            }

            //If we still cannot find the obj, then get it from persistence.
            TObject obj = GetFromPersistence(id);
            if (obj != null)
            {
                TrackObject(obj);
            }

            return obj;
        }
        public virtual void Add(TObject obj)
        {
            if ((from trackingObj in trackingObjectList where (trackingObj.Status == ObjectStatus.New || trackingObj.Status == ObjectStatus.Tracking) && Equals(trackingObj.CurrentValue.Id, obj.Id) select trackingObj).FirstOrDefault() != null)
            {
                throw new InvalidOperationException("The obj already exists.");
            }
            else
            {
                var trackingObject = (from trackingObj in trackingObjectList where trackingObj.Status == ObjectStatus.Removed && Equals(trackingObj.BackupValue.Id, obj.Id) select trackingObj).FirstOrDefault();
                if (trackingObject != null)
                {
                    trackingObject.CurrentValue = obj;
                    trackingObject.Status = ObjectStatus.Tracking;
                }
                else
                {
                    trackingObjectList.Add(new TrackObject<TObject, TObjectId>() { CurrentValue = obj, Status = ObjectStatus.New });
                }
            }
        }
        public virtual void Remove(TObject obj)
        {
            var trackingObject = (from trackingObj in trackingObjectList where trackingObj.Status == ObjectStatus.New && Equals(trackingObj.CurrentValue.Id, obj.Id) select trackingObj).FirstOrDefault();
            if (trackingObject != null)
            {
                trackingObjectList.Remove(trackingObject);
            }
            else
            {
                trackingObject = (from trackingObj in trackingObjectList where trackingObj.Status == ObjectStatus.Tracking && Equals(trackingObj.CurrentValue.Id, obj.Id) select trackingObj).FirstOrDefault();
                if (trackingObject != null)
                {
                    trackingObject.Status = ObjectStatus.Removed;
                }
            }
        }

        #endregion

        #region ICanPersist Members

        public virtual void PersistChanges()
        {
            PersistNewObjects(GetNewObjects());
            PersistModifiedObjects(GetModifiedObjects());
            PersistRemovedObjects(GetRemovedObjects());
        }

        #endregion

        #region Protected Methods

        protected void TrackObject(TObject obj)
        {
            var trackObject = trackingObjectList.Find(trackObj => Equals(Equals(trackObj.CurrentValue.Id, obj.Id)));
            if (trackObject == null)
            {
                trackingObjectList.Add(new TrackObject<TObject, TObjectId>() { BackupValue = CreateBackupObject(obj), CurrentValue = obj, Status = ObjectStatus.Tracking });
            }
        }
        protected void TrackObjects(IEnumerable<TObject> objects)
        {
            objects.ForEach(obj => TrackObject(obj));
        }
        protected IList<TObject> GetTrackingObjects()
        {
            return (from trackingObject in trackingObjectList where trackingObject.Status != ObjectStatus.Removed select trackingObject.CurrentValue).ToList();
        }
        protected IList<TObject> GetObjects(IEnumerable<TObject> domainObjectsFromDataPersistence, Func<TObject, bool> predicate)
        {
            TrackObjects(domainObjectsFromDataPersistence);
            return GetTrackingObjects().Where(predicate).ToList();
        }
        protected virtual TObject GetFromPersistence(TObjectId id)
        {
            return null;
        }
        protected virtual void PersistNewObjects(List<TObject> newObjects)
        {
        }
        protected virtual void PersistModifiedObjects(List<TObject> modifiedObjects)
        {
        }
        protected virtual void PersistRemovedObjects(List<TObject> removedObjects)
        {
        }

        #endregion

        #region Private Methods

        private bool IsObjectRemoved(TObjectId id)
        {
            return (from trackingObject in trackingObjectList where trackingObject.Status == ObjectStatus.Removed && Equals(trackingObject.BackupValue.Id, id) select trackingObject).FirstOrDefault() != null;
        }
        private ObjectBackupObject<TObjectId> CreateBackupObject(TObject obj)
        {
            var backupObject = new ObjectBackupObject<TObjectId>() { Id = obj.Id };

            foreach (var propertyInfo in GetTrackingProperties(obj))
            {
                backupObject.TrackingProperties.Add(propertyInfo.Name, CopyValue(propertyInfo.GetValue(obj, null)));
            }

            return backupObject;
        }
        private object CopyValue(object value)
        {
            if (value is IEnumerable)
            {
                List<object> subValueList = new List<object>();
                IEnumerator enumerator = ((IEnumerable)value).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    subValueList.Add(CopyValue(enumerator.Current));
                }
                return subValueList;
            }
            else
            {
                return value;
            }
        }
        private bool IsObjectModified(TrackObject<TObject, TObjectId> trackingObject)
        {
            if (trackingObject.Status == ObjectStatus.Tracking && trackingObject.CurrentValue != null)
            {
                foreach (var propertyInfo in GetTrackingProperties(trackingObject.CurrentValue))
                {
                    var backupValue = trackingObject.BackupValue.TrackingProperties[propertyInfo.Name];
                    var currentValue = propertyInfo.GetValue(trackingObject.CurrentValue, null);
                    if (!IsValueEqual(backupValue, currentValue))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool IsValueEqual(object firstValue, object secondValue)
        {
            if (ReferenceEquals(firstValue, null) ^ ReferenceEquals(secondValue, null))
            {
                return false;
            }
            if (ReferenceEquals(firstValue, null))
            {
                return true;
            }
            if (firstValue.GetType() != secondValue.GetType())
            {
                return false;
            }

            if (firstValue is IEnumerable && secondValue is IEnumerable)
            {
                var firstValueEnumerator = ((IEnumerable)firstValue).GetEnumerator();
                var secondValueEnumerator = ((IEnumerable)secondValue).GetEnumerator();
                var hasFirstSubValue = firstValueEnumerator.MoveNext();
                var hasSecondSubValue = secondValueEnumerator.MoveNext();

                while (hasFirstSubValue && hasSecondSubValue)
                {
                    if (!IsValueEqual(firstValueEnumerator.Current, secondValueEnumerator.Current))
                    {
                        return false;
                    }
                    hasFirstSubValue = firstValueEnumerator.MoveNext();
                    hasSecondSubValue = secondValueEnumerator.MoveNext();
                }
                if (!hasFirstSubValue && !hasSecondSubValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (firstValue is IEnumerable && !(secondValue is IEnumerable))
            {
                return false;
            }
            else if (!(firstValue is IEnumerable) && secondValue is IEnumerable)
            {
                return false;
            }

            var valueType = firstValue.GetType();
            if (valueType.IsValueType || valueType == typeof(string) || valueType.IsClass && typeof(IValueObject).IsAssignableFrom(valueType))
            {
                return firstValue == secondValue;
            }

            return false;
        }
        private List<PropertyInfo> GetTrackingProperties(TObject obj)
        {
            return (from propertyInfo in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    select propertyInfo).ToList();
        }
        private List<TObject> GetNewObjects()
        {
            return (from trackingObject in trackingObjectList where trackingObject.Status == ObjectStatus.New select trackingObject.CurrentValue).ToList();
        }
        private List<TObject> GetModifiedObjects()
        {
            return (from trackingObject in trackingObjectList where IsObjectModified(trackingObject) select trackingObject.CurrentValue).ToList();
        }
        private List<TObject> GetRemovedObjects()
        {
            return (from trackingObject in trackingObjectList where trackingObject.Status == ObjectStatus.Removed select trackingObject.CurrentValue).ToList();
        }

        #endregion
    }

    public class TrackObject<TObject, TObjectId> where TObject : class, IObject<TObjectId>
    {
        public ObjectBackupObject<TObjectId> BackupValue { get; set; }
        public TObject CurrentValue { get; set; }
        public ObjectStatus Status { get; set; }
    }
    public class ObjectBackupObject<TObjectId>
    {
        public ObjectBackupObject() { TrackingProperties = new Dictionary<string, object>(); }
        public TObjectId Id { get; set; }
        public Dictionary<string, object> TrackingProperties { get; private set; }
    }
    public enum ObjectStatus
    {
        New,
        Tracking,
        Removed
    }
}
