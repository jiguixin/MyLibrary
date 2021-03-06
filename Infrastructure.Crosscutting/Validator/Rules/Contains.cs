﻿
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
    /// Determines that the IEnumerable contains a specific item
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class Contains<ObjectType, DataType> : Rule<ObjectType, IEnumerable<DataType>>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="Value">Value that the IEnumerable needs to contain</param>
        /// <param name="ErrorMessage">Error message</param>
        public Contains(Func<ObjectType, IEnumerable<DataType>> ItemToValidate, DataType Value, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.Value = Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value to search for
        /// </summary>
        protected virtual DataType Value { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            GenericEqualityComparer<DataType> Comparer = new GenericEqualityComparer<DataType>();
            foreach (DataType Item in ItemToValidate(Object))
                if (Comparer.Equals(Item, Value))
                    return;
            throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// Contains attribute
    /// </summary>
    public class Contains : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="Value">Value to compare to</param>
        public Contains(object Value, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.Value = (IComparable)Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value to compare to
        /// </summary>
        public IComparable Value { get; set; }

        #endregion
    }
}