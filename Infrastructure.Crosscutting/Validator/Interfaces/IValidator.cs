
#region Usings



#endregion

namespace Infrastructure.Crosscutting.Validator.Interfaces
{
    /// <summary>
    /// Validator interface
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validates an object
        /// </summary>
        /// <param name="Object">Object to validate</param>
        void Validate(object Object);
    }
}
