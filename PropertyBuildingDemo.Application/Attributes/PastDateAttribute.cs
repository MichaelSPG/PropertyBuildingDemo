using System.ComponentModel.DataAnnotations;

namespace PropertyBuildingDemo.Application.Attributes
{
    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateValue = (DateTime)value;
                if (dateValue >= DateTime.Now)
                {
                    return new ValidationResult("Date must be in the past.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
