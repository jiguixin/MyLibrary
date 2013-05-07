using System.Collections.Generic;
using System.Linq;

namespace Domain.Seedwork.DomainModel
{
    public interface IValueObject
    {
        IEnumerable<object> GetAtomicValues();
    }
    public abstract class TValueObject<T> : IValueObject
    {
        public abstract IEnumerable<object> GetAtomicValues();

        public static bool operator ==(TValueObject<T> left, TValueObject<T> right)
        {
            return IsEqual(left, right);
        }
        public static bool operator !=(TValueObject<T> left, TValueObject<T> right)
        {
            return !IsEqual(left, right);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            TValueObject<T> other = (TValueObject<T>)obj;
            IEnumerator<object> thisValues = this.GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }
        public override int GetHashCode()
        {
            return GetAtomicValues()
               .Select(x => x != null ? x.GetHashCode() : 0)
               .Aggregate((x, y) => x ^ y);
        }

        private static bool IsEqual(TValueObject<T> left, TValueObject<T> right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }
    }
}
