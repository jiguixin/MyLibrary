 
#region Usings
using System;
using System.Collections.Generic;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;

#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This item is not empty
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class NotEmpty<ObjectType, DataType> : Rule<ObjectType, IEnumerable<DataType>>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public NotEmpty(Func<ObjectType, IEnumerable<DataType>> ItemToValidate, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
        }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            foreach (object Item in ItemToValidate(Object))
                return;
            throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// NotEmpty attribute
    /// </summary>
    public class NotEmpty : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public NotEmpty(string ErrorMessage = "")
            : base(ErrorMessage)
        {
        }

        #endregion
    }
}