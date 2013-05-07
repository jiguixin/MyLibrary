 
#region Usings
using System;
using System.Collections.Generic;
using Infrastructure.Crosscutting.Validator.BaseClasses;
using Infrastructure.Crosscutting.Validator.Exceptions;
using Infrastructure.Crosscutting.Declaration;

#endregion

namespace Infrastructure.Crosscutting.Validator.Rules
{
    /// <summary>
    /// This item is required
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public class RequiredString<ObjectType> : Rule<ObjectType, string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="DefaultValue">Default value</param>
        public RequiredString(Func<ObjectType,string> ItemToValidate,string DefaultValue,string ErrorMessage)
            : base(ItemToValidate,ErrorMessage)
        {
            this.DefaultValue = DefaultValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Default value
        /// </summary>
        protected virtual string DefaultValue { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            GenericEqualityComparer<string> Comparer = new GenericEqualityComparer<string>();
            if (Comparer.Equals(ItemToValidate(Object), DefaultValue))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }
}