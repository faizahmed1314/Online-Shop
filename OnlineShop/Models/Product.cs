using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        [Display(Name = "Available")]
        [Required]
        public bool IsAvailable { get; set; }
        public ProductTypes productTypes { get; set; }
        [Display(Name = "Product Type")]
        public int ProductTypesId { get; set; }
        public SpecialTag specialTag { get; set; }
        [Display(Name = "Special Tag")]
        public int SpecialTagId { get; set; }
    }
}
