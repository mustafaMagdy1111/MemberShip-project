using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Member_Ship.Entities
{
    [Table("Item")]
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string Title { get; set; }

        [MaxLength(2045)]
        public string Description { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        [DisplayName("Image Url")]

        [MaxLength(1024)]
        public string ImageUrl { get; set; }

        [AllowHtml]
        public string Html { get; set; }

        [DefaultValue(0)]
        [DisplayName("Wait Days")]

        public int WaitDays { get; set; }

        public string HtmlShort
        {

            get => Html == null || Html.Length < 50 ? Html : Html.Substring(0, 50);
        }

        public int ProductId { get; set; }
        public int ItemTypetId { get; set; }
        public int SectiontId { get; set; }
        public int PartId { get; set; }
        public bool  IsFree { get; set; }

        [DisplayName("Item Type")]
        public ICollection<ItemType> itemTypes { get; set; }

        public ICollection<Section>sections  { get; set; }

        public ICollection <Part> parts { get; set; }

    }
}