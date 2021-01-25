using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeachTowelShop.Services.Interfaces
{
  public  interface IOrderService
    {
        UserSessionCartDto SaveToCart(UserSessionCartDto userSessionDto, List<UserTextSessionDto> userTextSessionDto);

        Task<List<UserSessionCartDto>> GetItemsInCart(string sessionId);

        Task<bool> HasItems(string sessionId);

        Task DeleteItemFromCart(string sessionId, UserSessionCartDto userSessionCartDto);

        UserSessionCartDto UpdateCart(string sessionId, UserSessionCartDto userSessionCartDto);

        UserSessionCartDto SaveItemToCart(UserSessionCartDto userSessionDto);
        void CreateOrder(UserDetailsDto userDetailsViewDto);

        double GetSumForSession(string sessionId);

       Task<string> GetItemFolderPath(string productId);

    }
}
