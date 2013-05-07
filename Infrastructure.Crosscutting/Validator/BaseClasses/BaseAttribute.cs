
#region Usings

using System;

#endregion

namespace Infrastructure.Crosscutting.Validator.BaseClasses
{
    /// <summary>
    /// Base class for the attributes
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BaseAttribute : Attribute 
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public BaseAttribute(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }

        #endregion
        
        #region Public Properties

        /// <summary>
        /// Error message thrown if it is not valid
        /// </summary>
        public string ErrorMessage { get; set; }

        #endregion
    }
}