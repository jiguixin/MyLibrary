 
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
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class Required<ObjectType, DataType> : Rule<ObjectType, DataType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="DefaultValue">Default value</param>
        public Required(Func<ObjectType, DataType> ItemToValidate, DataType DefaultValue, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.DefaultValue = DefaultValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Default value
        /// </summary>
        protected virtual DataType DefaultValue { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            GenericEqualityComparer<DataType> Comparer = new GenericEqualityComparer<DataType>();
            if (Comparer.Equals(ItemToValidate(Object), DefaultValue))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// Required attribute
    /// </summary>
    public class Required : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="DefaultValue">Default value</param>
        public Required(object DefaultValue, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.DefaultValue = DefaultValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Default value
        /// </summary>
        public object DefaultValue { get; set; }

        #endregion
    }
}