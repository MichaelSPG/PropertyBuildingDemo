using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PropertyBuildingDemo.Application.Dto
{
    /// <summary>
    /// Data transfer object (DTO) for property information.
    /// </summary>
    public class PropertyDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDto"/> class.
        /// </summary>
        public PropertyDto()
        {
            PropertyImages = new List<PropertyImageDto>();
            PropertyTraces = new List<PropertyTraceDto>();
        }

        /// <summary>
        /// Gets or sets the ID of the property.
        /// </summary>
        public long IdProperty { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the property.
        /// </summary>
        [StringLength(40)]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the price of the property.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the internal code of the property.
        /// </summary>
        public string InternalCode { get; set; }

        /// <summary>
        /// Gets or sets the year of the property.
        /// </summary>
        [Required]
        public long Year { get; set; }

        /// <summary>
        /// Gets or sets the ID of the property owner.
        /// </summary>
        [Required]
        public long IdOwner { get; set; }

        /// <summary>
        /// Gets or sets a collection of property images associated with this property.
        /// </summary>
        public IEnumerable<PropertyImageDto> PropertyImages { get; set; }

        /// <summary>
        /// Gets or sets a collection of property traces associated with this property.
        /// </summary>
        public IEnumerable<PropertyTraceDto> PropertyTraces { get; set; }
    }
}
