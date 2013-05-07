
#region Usings
using System;

#endregion

namespace Infrastructure.Crosscutting.Validator.Exceptions
{
    /// <summary>
    /// Exception thrown when an item is not valid
    /// </summary>
    public class NotValid : Exception
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExceptionText">Exception string</param>
        public NotValid(string ExceptionText)
            : base(ExceptionText)
        {
        }

        #endregion
    }
}
