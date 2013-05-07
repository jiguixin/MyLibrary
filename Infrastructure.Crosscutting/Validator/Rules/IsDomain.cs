
#region Usings
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;
#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// Is Domain
    /// </summary>
    public class IsDomain<ObjectType> : Rule<ObjectType,string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public IsDomain(Func<ObjectType, string> ItemToValidate, string ErrorMessage)
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
            if(!new System.Text.RegularExpressions.Regex(@"^(http|https|ftp)://([a-zA-Z0-9_-]*(?:\.[a-zA-Z0-9_-]*)+):?([0-9]+)?/?").IsMatch(Value))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// IsDomain attribute
    /// </summary>
    public class IsDomain : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public IsDomain(string ErrorMessage = "")
            : base(ErrorMessage)
        {
        }

        #endregion
    }
}
