
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
    /// This item's length is less than the length specified
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public class MaxLength<ObjectType, DataType> : Rule<ObjectType, IEnumerable<DataType>>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="MaxLength">Max length of the string</param>
        /// <param name="ErrorMessage">Error message</param>
        public MaxLength(Func<ObjectType, IEnumerable<DataType>> ItemToValidate, int MaxLength, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.MaxLengthAllowed = MaxLength;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Max length of the string
        /// </summary>
        protected virtual int MaxLengthAllowed { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            IEnumerable<DataType> Value = ItemToValidate(Object);
            if (Value.IsNull())
                return;
            if (Value.Count() > MaxLengthAllowed)
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// This item's length is less than the length specified
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public class MaxLength<ObjectType> : Rule<ObjectType, string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="MaxLength">Max length of the string</param>
        /// <param name="ErrorMessage">Error message</param>
        public MaxLength(Func<ObjectType, string> ItemToValidate, int MaxLength, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.MaxLengthAllowed = MaxLength;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Max length of the string
        /// </summary>
        protected virtual int MaxLengthAllowed { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            string Value = ItemToValidate(Object);
            if (Value.IsNull())
                return;
            if (Value.Count() > MaxLengthAllowed)
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// Max length attribute
    /// </summary>
    public class MaxLength : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="MaxLength">Max length</param>
        public MaxLength(int MaxLength, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.MaxLengthAllowed = MaxLength;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Max length value
        /// </summary>
        public int MaxLengthAllowed { get; set; }

        #endregion
    }
}