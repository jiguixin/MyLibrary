using System;
using System.Collections.Generic;

namespace Domain.Seedwork.DomainModel
{
    [Serializable]
    public class DomainException : Exception
    {
        public IValidationError ValidationError { get; private set; }

        public DomainException()
        {
            ValidationError = new ValidationError();
        }
        public DomainException(string errorKey) : this()
        {
            ValidationError.AddError(errorKey);
        }
        public DomainException(string errorKey, params object[] parameters) : this()
        {
            ValidationError.AddError(errorKey, parameters);
        }
        public DomainException(string errorKey, IList<object> parameters) : this()
        {
            ValidationError.AddError(errorKey, parameters);
        }
    }
    [Serializable]
    public class ObjectNotExistException : DomainException
    {
        public ObjectNotExistException(string errorKey, object domainObjectId)
            : base(errorKey, domainObjectId)
        {
        }
    }
    [Serializable]
    public class RemoveObjectFailedException : DomainException
    {
        public RemoveObjectFailedException(string errorKey, object domainObjectId)
            : base(errorKey, domainObjectId)
        {
        }
    }
    [Serializable]
    public class DuplicatedNameException : DomainException
    {
        public DuplicatedNameException(string errorKey, string name)
            : base(errorKey, name)
        {
        }
    }
}
