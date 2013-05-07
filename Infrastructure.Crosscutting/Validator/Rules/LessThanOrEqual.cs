
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
    /// This item is less than or equal to a value
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class LessThanOrEqual<ObjectType, DataType> : Rule<ObjectType, DataType> where DataType:IComparable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="MaxValue">Max value</param>
        /// <param name="ErrorMessage">Error message</param>
        public LessThanOrEqual(Func<ObjectType, DataType> ItemToValidate,DataType MaxValue, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.MaxValue = MaxValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Max value
        /// </summary>
        protected virtual DataType MaxValue { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            GenericComparer<DataType> Comparer = new GenericComparer<DataType>();
            if (Comparer.Compare(ItemToValidate(Object), MaxValue) > 0)
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// LessThanOrEqual attribute
    /// </summary>
    public class LessThanOrEqual : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="Value">Value to compare to</param>
        public LessThanOrEqual(object Value, string ErrorMessage = "")
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
