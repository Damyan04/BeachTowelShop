using AutoMapper;
using BeachTowelShop.Data;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeachTowelShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IMapper _mapper;
        public OrderService(ApplicationDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public void CreateOrder(UserDetailsDto userDetailsViewDto)
        {
            var order = _mapper.Map<Order>(userDetailsViewDto);
          
       
            var cartItems = _appDbContext.CartItems.Where(a => a.UserSessionId == userDetailsViewDto.UsersessionId && a.OrderId == null).ToList();
            var textItems = _appDbContext.TextProperties.Where(a => a.UserSessionId==userDetailsViewDto.UsersessionId && a.OrderId == null).ToList();
            _appDbContext.Orders.Add(order);
            order.CartItems.AddRange(cartItems);
           order.TextProperties.AddRange(textItems);
            order.Status = "Unconfirmed";
            _appDbContext.Orders.Add(order);
            _appDbContext.SaveChanges();
           
        }

        public void DeleteItemFromCart(string sessionId, UserSessionCartDto userSessionDto)
        {
           var item= _appDbContext.CartItems.Where(a => a.UserSessionId == sessionId && a.ProductId == userSessionDto.ProductId&&a.Size.Contains(userSessionDto.Size)).FirstOrDefault();
           var textPropertiesList= _appDbContext.TextProperties.Where(a => a.UserSessionId == sessionId && a.CartItemId == item.Id).ToList();

            _appDbContext.CartItems.Remove(item);
            _appDbContext.TextProperties.RemoveRange(textPropertiesList);
            _appDbContext.SaveChanges();
        }

        public string GetItemFolderPath(string productId)
        {
            return _appDbContext.CartItems.Where(a => a.ProductId == productId).Select(a => a.DesignFolderPath).FirstOrDefault();
        }

        public List<UserSessionCartDto> GetItemsInCart(string sessionId)
        {
          var items= _appDbContext.CartItems.Where(a => a.UserSessionId == sessionId && a.OrderId == null).ToList();
         
            var itemsDto = _mapper.Map<List<UserSessionCartDto>>(items);
            return itemsDto;
        }

        public double GetSumForSession(string sessionId)
        {
          return  _appDbContext.CartItems.Where(a => a.UserSessionId == sessionId&&a.OrderId==null).Select(a => a.Sum).Sum();
        }

        public bool HasItems(string sessionId)
        {
           return _appDbContext.CartItems.Any(a => a.UserSessionId == sessionId && a.OrderId == null);
        }

        public void SaveItemToCart(UserSessionCartDto userSessionDto)
        {
            var session = _appDbContext.UserSessions.FirstOrDefault(a => a.Id == userSessionDto.UserSessionId);
            if (session==null)
            {
                var userSession = new UserSession() { Id=userSessionDto.UserSessionId };
                _appDbContext.UserSessions.Add(userSession);
                _appDbContext.SaveChanges();
            }
            var item = _mapper.Map<CartItem>(userSessionDto);
            var item2 = _appDbContext.CartItems.Where(a => a.UserSessionId == userSessionDto.UserSessionId && a.ProductId == userSessionDto.ProductId && a.Size.Contains(userSessionDto.Size)).FirstOrDefault();
            if (item2 != null)
            {
                item2.Count += userSessionDto.Count;
                item2.Sum += userSessionDto.Sum;
                _appDbContext.CartItems.Update(item2);
                _appDbContext.SaveChanges();
            }
            else
            {
                _appDbContext.CartItems.Add(item);
                _appDbContext.SaveChanges();
            }
            
        }

        public UserSessionCartDto SaveToCart(UserSessionCartDto userSessionDto, List<UserTextSessionDto> userTextSessionDto)
        {
            var session = _appDbContext.UserSessions.FirstOrDefault(a => a.Id == userSessionDto.UserSessionId);
            if (session == null)
            {
                var userSession = new UserSession() { Id = userSessionDto.UserSessionId };
                _appDbContext.UserSessions.Add(userSession);
                _appDbContext.SaveChanges();
            }
            var item = _mapper.Map<CartItem>(userSessionDto);
            _appDbContext.CartItems.Add(item);
            _appDbContext.SaveChanges();
            var textList = _mapper.Map<List<TextProperty>>(userTextSessionDto);
            textList.ForEach(a => a.CartItemId = item.Id);
            _appDbContext.TextProperties.AddRange(textList);
            _appDbContext.SaveChanges();
            return userSessionDto;
        }

        public void UpdateCart(string sessionId, UserSessionCartDto userSessionCartDto)
        {
            var item = _appDbContext.CartItems.Where(a => a.UserSessionId == sessionId && a.ProductId == userSessionCartDto.ProductId && a.Size.Contains(userSessionCartDto.Size)).FirstOrDefault();
            if (item != null)
            {
                item.Count = userSessionCartDto.Count;
                item.Sum = userSessionCartDto.Sum;
            }
            _appDbContext.SaveChanges();
            
        }
    }
}
