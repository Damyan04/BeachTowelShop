using BeachTowelShop.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeachTowelShop.Data.Models
{
    public class Product 
    {
        public Product()
        {
           Comments = new HashSet<Comment>();

            ProductSizes = new HashSet<ProductSize>();
            ProductPictures = new HashSet<ProductPicture>();
            ProductCategories = new HashSet<ProductCategory>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description{ get; set; }

        public bool IsCustom { get; set; }

        public int OrderCount { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }

        public ICollection<ProductPicture> ProductPictures { get; set; }
        
        public ICollection<Comment> Comments { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

    }
}
