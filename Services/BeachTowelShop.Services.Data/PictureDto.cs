using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public class PictureDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Path { get; set; }

        public string ProductId { get; set; }
    }
}
