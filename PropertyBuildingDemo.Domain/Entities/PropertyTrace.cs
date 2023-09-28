﻿using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Entities
{
    public class PropertyTrace : BaseAuditableEntityDB
    {
        /// <summary>
        /// The primary key of this table (Owner)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long         IdPropertyTrace { get; set; }
        public DateTime?    DateSale{ get; set; }
        public string?      Name { get; set; }
        public decimal?     Value { get; set; }
        public decimal?     Tax{ get; set; }

        /// <summary>
        /// The property Id
        /// </summary>
        [ForeignKey(nameof(IdProperty))]
        public long IdProperty { get; set; }

        /// <summary>
        /// Implementation of GetId(), due to diferent names of columns ([Key]). for this is IdOwner.
        /// </summary>
        /// <returns>the Id of this property</returns>
        public override long GetId()
        {
            return IdPropertyTrace;
        }
    }
}
