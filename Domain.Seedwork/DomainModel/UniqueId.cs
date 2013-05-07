using System;
using System.Collections.Generic;

namespace Domain.Seedwork.DomainModel
{
    public class UniqueId : TValueObject<UniqueId>
    {
        public UniqueId() : this(Guid.NewGuid())
        { }
        public UniqueId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentNullException("UniqueId");
            }
            this.Value = value;
        }

        public Guid Value { get; private set; }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
