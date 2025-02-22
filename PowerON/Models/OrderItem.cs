﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PowerON.Models
{
    public class OrderItem
    {
        //[Key]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        public decimal UnitPrice { get; set; }

        //[ForeignKey("Item")]
        public int ItemId { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }

    }
}
