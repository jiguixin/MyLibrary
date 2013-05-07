
#region Usings
using System;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;
using System.Collections;
#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This is a cascade rule
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class Cascade<ObjectType, DataType> : Rule<ObjectType, DataType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public Cascade(Func<ObjectType, DataType> ItemToValidate, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
        }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            try
            {
                object Item = ItemToValidate(Object);
                if (Item is IEnumerable)
                    ValidationManager.Validate((IEnumerable)Item);
                else
                    ValidationManager.Validate(Item);
            }
            catch (Exception e) { throw new NotValid(ErrorMessage + e.Message); }
        }

        #endregion
    }

    /// <summary>
    /// Cascade attribute
    /// </summary>
    public class Cascade : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public Cascade(string ErrorMessage = "")
            : base(ErrorMessage)
        {
        }

        #endregion
    }
}