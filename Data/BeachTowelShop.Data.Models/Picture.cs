using BeachTowelShop.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeachTowelShop.Data.Models
{
  public  class Picture : IModel
    {
        public Picture()
        {
            ProductPictures = new HashSet<ProductPicture>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string Path { get; set; }

        public ICollection<ProductPicture> ProductPictures { get; set; }
    }
}
