namespace PropertyBuildingDemo.Domain.Common
{
    /// <summary>
    /// Represents paging parameters for retrieving data.
    /// </summary>
    public class PageArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageArgs"/> class with default values.
        /// </summary>
        public PageArgs()
        {
            PageIndex = 0;
            PageSize = 15;
        }

        /// <summary>
        /// Gets or sets the index of the page to retrieve.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the size of each page (number of items per page).
        /// </summary>
        public int PageSize { get; set; }
    }
}
