
#region Usings
using System;
using Infrastructure.Crosscutting.Validator.BaseClasses;

#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This is a custom rule
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class Custom<ObjectType, DataType> : Rule<ObjectType, DataType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ActionToCall">Validation action to call</param>
        public Custom(Func<ObjectType, DataType> ItemToValidate, Action<ObjectType> ActionToCall)
            : base(ItemToValidate, "")
        {
            this.ActionToCall = ActionToCall;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Action to call
        /// </summary>
        protected virtual Action<ObjectType> ActionToCall { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            ActionToCall(Object);
        }

        #endregion
    }

    /// <summary>
    /// Cascade attribute
    /// </summary>
    public class Custom : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="Action">Function to call</param>
        public Custom(string Action, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.Action = Action;
        }

        #endregion

        #region Properties

        public string Action { get; set; }

        #endregion
    }
}