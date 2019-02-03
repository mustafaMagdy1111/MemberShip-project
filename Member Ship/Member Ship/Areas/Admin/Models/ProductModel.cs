using Member_Ship.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Member_Ship.Areas.Admin.Models
{
    public class ProductModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        [Required]
        public string Title { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }

        [DisplayName("Image Url")]
        [MaxLength(1024)]
        public string ImageUrl { get; set; }

        public int ProductLinkTextId { get; set; }

        public int ProductTypeId { get; set; }

        [DisplayName("Product link Text")]
        public ICollection<ProductLinkText> productLinkTexts { get; set; }

        [DisplayName("Product Types")]

        public ICollection<ProductType> productTypes { get; set; }
        public string ProductType
        { get {
                return productTypes == null 
                    || productTypes.Count.Equals(0) ?
                    string.Empty : productTypes.First
                    (pt => pt.Id.Equals(ProductTypeId)).Title;

        } }
        public string ProductLinkText
        {
            get
            {
                return productLinkTexts == null
                    || productLinkTexts.Count.Equals(0) ?
                    string.Empty : productLinkTexts.First
                    (pt => pt.Id.Equals(ProductLinkTextId)).Title;

            }
        }

    }
}