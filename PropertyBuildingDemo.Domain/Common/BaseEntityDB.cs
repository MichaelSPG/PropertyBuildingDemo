using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Common
{
    /// <summary>
    /// The base Table entity representation that implements the common columns
    /// </summary>
    public abstract class BaseEntityDB : IEntityDB
    {
        public BaseEntityDB()
        {
            CreatedTime = DateTime.Now;
            UpdatedTime = DateTime.Now;
        }
       
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public abstract long GetId();
    }
}
