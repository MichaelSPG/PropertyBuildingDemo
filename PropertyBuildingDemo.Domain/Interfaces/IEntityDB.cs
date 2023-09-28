using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// The interface representation the common columns in a table object
    /// </summary>
    public interface IEntityDB
    {
        /// <summary>
        /// Gets the id of current table object (Entity)
        /// </summary>
        /// <returns>@Id of current Entity</returns>
        long GetId();
        /// <summary>
        /// Is deleted field of a table, we are not deleteting records from table, so we mark it as deleted
        /// </summary>
        bool IsDeleted { get; }
        /// <summary>
        /// Time of the creation of a table record
        /// </summary>
        DateTime CreatedTime { get; set; }
        /// <summary>
        /// The update time of a table record
        /// </summary>
        DateTime UpdatedTime { get; set; }
    }
}
