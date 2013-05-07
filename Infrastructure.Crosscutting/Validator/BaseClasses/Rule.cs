
#region Usings
using System;
using Infrastructure.Crosscutting.Validator.Interfaces;

#endregion

namespace Infrastructure.Crosscutting.Validator.BaseClasses
{
    /// <summary>
    /// Rule base class
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public abstract class Rule<ObjectType, DataType> : IRule<ObjectType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public Rule(Func<ObjectType, DataType> ItemToValidate, string ErrorMessage)
            : base()
        {
            this.ErrorMessage = ErrorMessage;
            this.ItemToValidate = ItemToValidate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Error message thrown if Validate is not valid
        /// </summary>
        public virtual string ErrorMessage { get; set; }

        /// <summary>
        /// Item to validate
        /// </summary>
        public virtual Func<ObjectType, DataType> ItemToValidate { get; set; }

        #endregion

        #region Functions

        public abstract void Validate(ObjectType Object);

        #endregion
    }
}