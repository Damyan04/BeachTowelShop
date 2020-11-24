﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Data
{
   public  class UserTextSessionDto
    {
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
    }
}
