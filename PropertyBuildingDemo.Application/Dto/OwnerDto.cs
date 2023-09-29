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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the owner.
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the photo of the owner (as a byte array).
        /// </summary>
        [Required]
        public byte[] Photo { get; set; }

        /// <summary>
        /// Gets or sets the birthday of the owner.
        /// </summary>
        public DateTime BirthDay { get; set; }
    }
}
