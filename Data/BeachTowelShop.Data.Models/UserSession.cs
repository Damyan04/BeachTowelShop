using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeachTowelShop.Data.Models
{
    public class UserSession
    {
        [Key]
        public string Id { get; set; }
        public List<TextProperty> TextProperties { get; set; } = new List<TextProperty>();
        public DateTime ExpireDate { get; set; } = DateTime.UtcNow.AddDays(5);
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
