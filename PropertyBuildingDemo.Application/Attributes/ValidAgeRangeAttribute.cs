using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Application.Attributes
{
    /// <summary>
    /// Specifies a custom validation attribute that enforces a valid age range on a <see cref="DateTime"/> property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ValidAgeRangeAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidAgeRangeAttribute"/> class with the specified minimum and maximum ages.
        /// </summary>
        /// <param name="minAge">The minimum age of the valid range (inclusive).</param>
        /// <param name="maxAge">The maximum age of the valid range (inclusive).</param>
        public ValidAgeRangeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        /// <summary>
        /// Validates whether the provided value is a <see cref="DateTime"/> and represents a date of birth within the valid age range.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// <see cref="ValidationResult.Success"/> if the date of birth corresponds to an age within the valid age range and is not in the future;
        /// otherwise, a <see cref="ValidationResult"/> with an error message indicating the validation failure.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime birthDate)
            {
                // Calculate the age by subtracting the birthDate from the current date.
                int age = DateTime.Now.Year - birthDate.Year;

                // Adjust the age if the birth date hasn't occurred yet this year.
                if (DateTime.Now < birthDate.AddYears(age))
                {
                    age--;
                }

                if (age >= _minAge && age <= _maxAge)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"The {validationContext.DisplayName} must have an age within the valid age range: {_minAge} to {_maxAge} years.");
        }
    }
}
