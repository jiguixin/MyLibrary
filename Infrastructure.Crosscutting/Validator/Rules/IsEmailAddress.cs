
#region Usings
using System;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;
#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// Email address
    /// </summary>
    public class IsEmailAddress<ObjectType> : Rule<ObjectType, string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public IsEmailAddress(Func<ObjectType, string> ItemToValidate, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
        }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            string Value = this.ItemToValidate(Object);
            if (string.IsNullOrEmpty(Value))
                return;
            System.Text.RegularExpressions.Regex TempReg = new System.Text.RegularExpressions.Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if(!TempReg.IsMatch(Value))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// IsEmailAddress attribute
    /// </summary>
    public class IsEmailAddress : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public IsEmailAddress(string ErrorMessage = "")
            : base(ErrorMessage)
        {
        }

        #endregion
    }
}