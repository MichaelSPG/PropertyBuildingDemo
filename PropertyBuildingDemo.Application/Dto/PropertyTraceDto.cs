using System.ComponentModel.DataAnnotations;

namespace PropertyBuildingDemo.Application.Dto
{
    /// <summary>
    /// Data transfer object (DTO) for property trace information.
    /// </summary>
    public class PropertyTraceDto
    {
        /// <summary>
        /// Gets or sets the ID of the property trace.
        /// </summary>
        public long IdPropertyTrace { get; set; }

        /// <summary>
        /// Gets or sets the date of sale for the property.
        /// </summary>
        public DateTime DateSale { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the property trace.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the property trace.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the tax associated with the property trace.
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Gets or sets the ID of the property associated with this trace.
        /// </summary>
        public long IdProperty { get; set; }
    }
}
