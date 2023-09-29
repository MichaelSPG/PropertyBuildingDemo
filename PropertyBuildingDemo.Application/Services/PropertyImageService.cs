using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Application.Services
{
    /// <summary>
    /// Service for managing property images.
    /// </summary>
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyImageService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work used for data access.</param>
        public PropertyImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Adds multiple property images to the database.
        /// </summary>
        /// <param name="inPropertyImages">The collection of property images to add.</param>
        /// <returns>The added property images.</returns>
        public Task<IEnumerable<PropertyImage>> AddMultipleImages(IEnumerable<PropertyImage> inPropertyImages)
        {
            return _unitOfWork.GetRepository<PropertyImage>().AddRangeAsync(inPropertyImages);
        }
    }
}
