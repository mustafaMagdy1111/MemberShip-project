using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Member_Ship.Entities
{
    [Table("Subscription Product")]
    public class SubscriptionProduct
    {
        [Required]
        [Key,Column(Order =1)]
        public int ProductId { get; set; }

        [Key, Column(Order = 2)]
        public int SubscriptiontId { get; set; }

        [NotMapped]
        public int OldProductId { get; set; }

        [NotMapped]
        public int OldSubscriptiontId { get; set; }
    }
}