using System;

namespace Domain.Seedwork.DomainModel
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MannualAttribute : Attribute
    {
    }
}
