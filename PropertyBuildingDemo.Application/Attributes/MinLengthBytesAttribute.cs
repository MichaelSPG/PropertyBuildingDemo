using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Application.Attributes
{
    /// <summary>
    /// Custom validation attribute for ensuring that a byte array property has a minimum length in bytes.
    /// </summary>
    public class MinLengthBytesAttribute : ValidationAttribute
    {
        private readonly int minLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinLengthBytesAttribute"/> class with the specified minimum length.
        /// </summary>
        /// <param name="minLength">The minimum length in bytes that the byte array property must have.</param>
        public MinLengthBytesAttribute(int minLength)
        {
            this.minLength = minLength;
        }

        /// <summary>
        /// Validates whether the provided value is a byte array and has a length greater than or equal to the minimum length.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// <see cref="ValidationResult.Success"/> if the byte array property meets the minimum length requirement;
        /// otherwise, a <see cref="ValidationResult"/> with an error message indicating the validation failure.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is byte[] byteArray && byteArray.Length < minLength)
            {
                return new ValidationResult($"The {validationContext.DisplayName} must have a minimum length of {minLength} bytes.");
            }

            return ValidationResult.Success;
        }
    }
}
