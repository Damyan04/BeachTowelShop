using BeachTowelShop.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeachTowelShop.Data.Models
{
   public class Comment 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        [Required]
        public string Description { get; set; } = null;
      
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
