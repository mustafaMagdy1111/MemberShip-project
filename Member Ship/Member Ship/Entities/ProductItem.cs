using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Member_Ship.Entities
{
    [Table("Product Item")]
    public class ProductItem
    {
        [Required]
        [Key,Column(Order =1)]
        public int ProductId { get; set; }

        [Key, Column(Order = 2)]
        public int ItemtId { get; set; }

        [NotMapped]
        public int OldProductId { get; set; }

        [NotMapped]
        public int OldItemtId { get; set; }
    }
}