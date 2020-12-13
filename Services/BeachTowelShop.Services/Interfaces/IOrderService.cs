using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeachTowelShop.Services.Interfaces
{
  public  interface IOrderService
    {
        UserSessionCartDto SaveToCart(UserSessionCartDto userSessionDto, List<UserTextSessionDto> userTextSessionDto);

        List<UserSessionCartDto> GetItemsInCart(string sessionId);

        bool HasItems(string sessionId);

        void DeleteItemFromCart(string sessionId, UserSessionCartDto userSessionCartDto);

        UserSessionCartDto UpdateCart(string sessionId, UserSessionCartDto userSessionCartDto);

        UserSessionCartDto SaveItemToCart(UserSessionCartDto userSessionDto);
        void CreateOrder(UserDetailsDto userDetailsViewDto);

        double GetSumForSession(string sessionId);

       string GetItemFolderPath(string productId);

    }
}
