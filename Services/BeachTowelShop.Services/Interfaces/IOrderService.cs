using BeachTowelShop.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeachTowelShop.Services.Interfaces
{
  public  interface IOrderService
    {
        Task<UserSessionCartDto> SaveToCart(UserSessionCartDto userSessionDto, List<UserTextSessionDto> userTextSessionDto);

        Task<List<UserSessionCartDto>> GetItemsInCart(string sessionId);

        Task<bool> HasItems(string sessionId);

        Task DeleteItemFromCart(string sessionId, UserSessionCartDto userSessionCartDto);

        Task<UserSessionCartDto> UpdateCart(string sessionId, UserSessionCartDto userSessionCartDto);

        Task<UserSessionCartDto> SaveItemToCart(UserSessionCartDto userSessionDto);
        Task CreateOrder(UserDetailsDto userDetailsViewDto);

        Task<double> GetSumForSession(string sessionId);

       Task<string> GetItemFolderPath(string productId);

    }
}
