using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeachTowelShop.Data.Models
{
   public class TextProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string BackgroundColor { get; set; }
        public string Text { get; set; }

        public string FontWeight { get; set; }

        public string FontSize { get; set; }

        public string Stroke { get; set; } 
        public string Fill { get; set; }

        public string FontFamily { get; set; }

        public string Underline { get; set; }
        public string FontStyle { get; set; }
        public string Overline { get; set; }
        public string UserSessionId { get; set; }
        public UserSession UserSession{ get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }
        public string CartItemId { get; set; }
        public CartItem CartItem { get; set; }
    }
}
