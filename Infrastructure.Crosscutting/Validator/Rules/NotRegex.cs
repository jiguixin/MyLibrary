 
#region Usings
using System;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;

#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This item is not equal to the regex string
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public class NotRegex<ObjectType> : Rule<ObjectType, string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="RegexString">Regex string</param>
        /// <param name="ErrorMessage">Error message</param>
        public NotRegex(Func<ObjectType, string> ItemToValidate, string RegexString, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            ValidationRegex = new System.Text.RegularExpressions.Regex(RegexString);
        }

        #endregion

        #region Properties

        protected virtual System.Text.RegularExpressions.Regex ValidationRegex { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            if (string.IsNullOrEmpty(ItemToValidate(Object)))
                throw new NotValid(ErrorMessage);
            if (ValidationRegex.IsMatch(ItemToValidate(Object)))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// Regex attribute
    /// </summary>
    public class NotRegex : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="RegexString">Regex string value</param>
        public NotRegex(string RegexString, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.RegexString = RegexString;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Regex string value
        /// </summary>
        public string RegexString { get; set; }

        #endregion
    }
}
