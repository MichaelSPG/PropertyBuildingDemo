using PropertyBuildingDemo.Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PropertyBuildingDemo.Application.Dto
{
    /// <summary>
    /// Data transfer object (DTO) for owner information.
    /// </summary>
    public class OwnerDto
    {
        /// <summary>
        /// Gets or sets the ID of the owner.
        /// </summary>
        public long IdOwner { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        [Required]
        [RegularExpression(@"^[A-Za-z\s\-]+$", ErrorMessage = "Name must contain only letters, spaces, and hyphens.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the owner.
        /// </summary>
        [Required]
        [RegularExpression(@"^[A-Za-z0-9\s\-,]+$", ErrorMessage = "Address must contain only alphanumeric characters, spaces, hyphens, and commas.")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the photo of the owner (as a byte array).
        /// </summary>
        [Required]
        [MinLengthBytesAttribute( minLength:200)]
        public byte[] Photo { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the owner.
        /// </summary>
        /// <remarks>
        /// The date of birth must fall within a valid age range of at least 18 and up to 100 years old.
        /// </remarks>
        [ValidAgeRange(18, 100, ErrorMessage = "The age must be between 18 and 100 years old.")]
        public DateTime BirthDay { get; set; }
    }
}
