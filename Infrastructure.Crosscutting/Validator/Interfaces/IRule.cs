
#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Infrastructure.Crosscutting.Validator.Interfaces
{
    /// <summary>
    /// Rule interface
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public interface IRule<ObjectType>
    {
        #region Functions

        /// <summary>
        /// Validates
        /// </summary>
        /// <param name="Object">Object to validate</param>
        void Validate(ObjectType Object);

        #endregion
    }
}
