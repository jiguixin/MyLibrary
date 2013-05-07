
#region Usings
using System;
using System.Collections.Generic;
using Infrastructure.Crosscutting.Validator.Interfaces;
using System.Collections;
#endregion

namespace Infrastructure.Crosscutting.Validator
{
    /// <summary>
    /// Validation manager
    /// </summary>
    public static class ValidationManager
    {
        #region Fields

        /// <summary>
        /// Validators
        /// </summary>
        private static Dictionary<Type, IValidator> Validators = new Dictionary<Type, IValidator>();

        #endregion

        #region Functions

        /// <summary>
        /// Gets a specific validator
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <returns>The specified validator</returns>
        public static Validator<ObjectType> GetValidator<ObjectType>()
        {
            return (Validator<ObjectType>)GetValidator(typeof(ObjectType));
        }

        /// <summary>
        /// Gets a specific validator
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <returns>The specified validator</returns>
        public static IValidator GetValidator(Type ObjectType)
        {
            if (!Validators.ContainsKey(ObjectType))
            {
                System.Type ValidatorType = typeof(Validator<>);
                ValidatorType = ValidatorType.MakeGenericType(ObjectType);
                IValidator Validator = (IValidator)Activator.CreateInstance(ValidatorType);
                Validators.Add(ObjectType, Validator);
            }
            return Validators[ObjectType];
        }

        /// <summary>
        /// Validates an object
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Object">The object to validate</param>
        public static void Validate<ObjectType>(ObjectType Object)
        {
            if (Object is IEnumerable)
                ValidationManager.Validate((IEnumerable)Object);
            ValidationManager.GetValidator<ObjectType>().Validate(Object);
        }

        /// <summary>
        /// Validates an list of objects
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Objects">List of objects to validate</param>
        public static void Validate(IEnumerable Objects)
        {
            Type ObjectType = Objects.GetType().GetGenericArguments()[0];
            IValidator Validator = ValidationManager.GetValidator(ObjectType);
            foreach (object Object in Objects)
                Validator.Validate(Object);
        }

        #endregion
    }
}
