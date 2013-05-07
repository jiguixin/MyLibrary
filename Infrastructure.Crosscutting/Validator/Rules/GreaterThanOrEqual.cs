
#region Usings
using System;
using System.Collections.Generic;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;
using Infrastructure.Crosscutting.Declaration;

#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This item is greater than or equal to a value
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class GreaterThanOrEqual<ObjectType, DataType> : Rule<ObjectType, DataType> where DataType : IComparable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="MinValue">Min value</param>
        /// <param name="ErrorMessage">Error message</param>
        public GreaterThanOrEqual(Func<ObjectType, DataType> ItemToValidate, DataType MinValue, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.MinValue = MinValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Min value
        /// </summary>
        protected virtual DataType MinValue { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            GenericComparer<DataType> Comparer = new GenericComparer<DataType>();
            if (Comparer.Compare(ItemToValidate(Object), MinValue) < 0)
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// GreaterThanOrEqual attribute
    /// </summary>
    public class GreaterThanOrEqual : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="Value">Value to compare to</param>
        public GreaterThanOrEqual(object Value, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.Value = (IComparable)Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// value to compare to
        /// </summary>
        public IComparable Value { get; set; }

        #endregion
    }
}
