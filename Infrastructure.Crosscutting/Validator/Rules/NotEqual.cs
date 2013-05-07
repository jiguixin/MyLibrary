 

#region Usings
using System;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;
using Infrastructure.Crosscutting.Declaration;

#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This item is not equal to the value
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class NotEqual<ObjectType, DataType> : Rule<ObjectType, DataType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="Value">value</param>
        /// <param name="ErrorMessage">Error message</param>
        public NotEqual(Func<ObjectType, DataType> ItemToValidate, DataType Value, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.Value = Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// value
        /// </summary>
        protected virtual DataType Value { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            GenericEqualityComparer<DataType> Comparer = new GenericEqualityComparer<DataType>();
            if (Comparer.Equals(ItemToValidate(Object), Value))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// NotEqual attribute
    /// </summary>
    public class NotEqual : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="Value">Value to compare to</param>
        public NotEqual(object Value, string ErrorMessage = "")
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