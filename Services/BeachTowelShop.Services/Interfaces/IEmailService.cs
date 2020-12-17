using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeachTowelShop.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string to,string subject,string html);
    }
}
