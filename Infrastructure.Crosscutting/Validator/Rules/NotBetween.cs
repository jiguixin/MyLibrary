 
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
    /// This item is not between two values
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class NotBetween<ObjectType, DataType> : Rule<ObjectType, DataType> where DataType : IComparable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="MinValue">Min value</param>
        /// <param name="MaxValue">Max value</param>
        /// <param name="ErrorMessage">Error message</param>
        public NotBetween(Func<ObjectType, DataType> ItemToValidate, DataType MinValue, DataType MaxValue, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.MaxValue = MaxValue;
            this.MinValue = MinValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Min value
        /// </summary>
        protected virtual DataType MinValue { get; set; }

        /// <summary>
        /// Max value
        /// </summary>
        protected virtual DataType MaxValue { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            GenericComparer<DataType> Comparer = new GenericComparer<DataType>();
            if (Comparer.Compare(MaxValue, ItemToValidate(Object)) >= 0
                && Comparer.Compare(ItemToValidate(Object), MinValue) >= 0)
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// Between attribute
    /// </summary>
    public class NotBetween : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="MinValue">Min value to compare to</param>
        /// <param name="MaxValue">Max value to compare to</param>
        public NotBetween(object MinValue, object MaxValue, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.MinValue = (IComparable)MinValue;
            this.MaxValue = (IComparable)MaxValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Min value to compare to
        /// </summary>
        public IComparable MinValue { get; set; }

        /// <summary>
        /// Max value to compare to
        /// </summary>
        public IComparable MaxValue { get; set; }

        #endregion
    }
}