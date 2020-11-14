using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeachTowelShop.Data.Models
{
   public  class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Count { get; set; }
        public string Size { get; set; }
        public string DesignFolderPath { get; set; }
        public string DesignName { get; set; }
        public double Price { get; set; }
        public string UserSessionId { get; set; }
        public UserSession UserSession { get; set; }
        public string ProductId { get; set; }
        public double Sum { get; set; }
        public List<OriginalPictureName> OriginalPictureNames { get; set; }
        public List<TextProperty> TextProperties { get; set; } = new List<TextProperty>();
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
