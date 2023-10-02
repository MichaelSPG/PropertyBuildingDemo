using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Application.Services
{
    /// <summary>
    /// Service for managing property traces.
    /// </summary>
    public class PropertyTraceService : IPropertyTraceService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTraceService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work used for data access.</param>
        public PropertyTraceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Adds multiple property traces to the repository.
        /// </summary>
        /// <param name="inPropertyTraces">The collection of property traces to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added property traces.</returns>
        public async Task<IEnumerable<PropertyTrace>> AddMultipleTraces(IEnumerable<PropertyTrace> inPropertyTraces)
        {
            inPropertyTraces = await _unitOfWork.GetRepository<PropertyTrace>().AddRangeAsync(inPropertyTraces);
            await _unitOfWork.Complete();
            return inPropertyTraces;
        }
    }
}
