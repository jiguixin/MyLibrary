
#region Usings
using System;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;

#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This item is equal to the value
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public class NotNaN<ObjectType> : Rule<ObjectType, double>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public NotNaN(Func<ObjectType, double> ItemToValidate, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
        }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            if (double.IsNaN(ItemToValidate(Object)))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// NotNaN attribute
    /// </summary>
    public class NotNaN : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public NotNaN(string ErrorMessage = "")
            : base(ErrorMessage)
        {
        }

        #endregion
    }
}
