using PropertyBuildingDemo.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyBuildingDemo.Application.Dto
{
    /// <summary>
    /// Data transfer object (DTO) for property image information.
    /// </summary>
    public class PropertyImageDto
    {
        /// <summary>
        /// Gets or sets the ID of the property image.
        /// </summary>
        public long IdPropertyImage { get; set; }

        /// <summary>
        /// Gets or sets the ID of the property associated with this image.
        /// </summary>
        [ForeignKey(nameof(Property))]
        public long IdProperty { get; set; }

        /// <summary>
        /// Gets or sets the binary data of the image.
        /// </summary>
        [Required]
        public byte[] File { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image is enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
