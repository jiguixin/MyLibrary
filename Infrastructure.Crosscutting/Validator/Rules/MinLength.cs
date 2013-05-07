
#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;

#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This item's length is greater than the length specified
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public class MinLength<ObjectType, DataType> : Rule<ObjectType, IEnumerable<DataType>>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="MinLength">Min length of the string</param>
        /// <param name="ErrorMessage">Error message</param>
        public MinLength(Func<ObjectType, IEnumerable<DataType>> ItemToValidate, int MinLength, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.MinLengthAllowed = MinLength;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Min length of the string
        /// </summary>
        protected virtual int MinLengthAllowed { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            IEnumerable<DataType> Value = ItemToValidate(Object);
            if (Value.IsNull())
                return;
            if (Value.Count() < MinLengthAllowed)
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// Min length attribute
    /// </summary>
    public class MinLength : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="MinLength">Min length</param>
        public MinLength(int MinLength, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.MinLengthAllowed = MinLength;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Min length value
        /// </summary>
        public int MinLengthAllowed { get; set; }

        #endregion
    }
}