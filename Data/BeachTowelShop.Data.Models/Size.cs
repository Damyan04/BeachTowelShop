using BeachTowelShop.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeachTowelShop.Data.Models
{
    public class Size 

    {
        public Size()
        {
            ProductSizes = new HashSet<ProductSize>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Pixels { get; set; }

        public string Price { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }

        public string SizePicturePath { get; set; }
    }
}
