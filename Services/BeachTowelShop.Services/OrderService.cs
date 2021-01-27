﻿using AutoMapper;
using BeachTowelShop.Data;
using BeachTowelShop.Data.Models;
using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task CreateOrder(UserDetailsDto userDetailsViewDto)
        {
            var order = _mapper.Map<Order>(userDetailsViewDto);
          
       
            var cartItems = await _appDbContext.CartItems.Where(a => a.UserSessionId == userDetailsViewDto.UsersessionId && a.OrderId == null).ToListAsync();
            if (cartItems.Count>0)
            {

           
            var textItems = await _appDbContext.TextProperties.Where(a => a.UserSessionId==userDetailsViewDto.UsersessionId && a.OrderId == null).ToListAsync();
            _appDbContext.Orders.Add(order);
            order.CartItems.AddRange(cartItems);
           order.TextProperties.AddRange(textItems);
            order.Status = "Unconfirmed";
           
            _appDbContext.Orders.Add(order);
            await _appDbContext.SaveChangesAsync();
            foreach (var item in cartItems)
            {
               var productToUpdate= _appDbContext.Products.Where(a => a.Id == item.ProductId).FirstOrDefault();
                if (productToUpdate != null)
                {
                    productToUpdate.OrderCount += item.Count;
                    _appDbContext.Products.Update(productToUpdate);
                  await  _appDbContext.SaveChangesAsync();
                }
            }
        }
        }

        public async Task DeleteItemFromCart(string sessionId, UserSessionCartDto userSessionDto)
        {
           var item= _appDbContext.CartItems.Where(a => a.UserSessionId == sessionId && a.ProductId == userSessionDto.ProductId&&a.Size.Contains(userSessionDto.Size)&&a.OrderId==null).FirstOrDefault();
            if (item != null)
            {
                var textPropertiesList = await _appDbContext.TextProperties.Where(a => a.UserSessionId == sessionId && a.CartItemId == item.Id&&a.OrderId==null).ToListAsync();

                _appDbContext.CartItems.Remove(item);
                _appDbContext.TextProperties.RemoveRange(textPropertiesList);
               await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetItemFolderPath(string productId)
        {
            return _appDbContext.CartItems.Where(a => a.ProductId == productId&&a.OrderId==null).Select(a => a.DesignFolderPath).FirstOrDefault();
        }

        public async Task<List<UserSessionCartDto>> GetItemsInCart(string sessionId)
        {
          var items= await _appDbContext.CartItems.Where(a => a.UserSessionId == sessionId && a.OrderId == null).ToListAsync();
         
            var itemsDto = _mapper.Map<List<UserSessionCartDto>>(items);
            return itemsDto;
        }

        public async Task<double> GetSumForSession(string sessionId)
        {
          return  _appDbContext.CartItems.Where(a => a.UserSessionId == sessionId&&a.OrderId==null).Select(a => a.Sum).Sum();
        }

        public async Task<bool> HasItems(string sessionId)
        {
           return _appDbContext.CartItems.Any(a => a.UserSessionId == sessionId && a.OrderId == null);
        }

        public async Task<UserSessionCartDto> SaveItemToCart(UserSessionCartDto userSessionDto)
        {
            var session = _appDbContext.UserSessions.FirstOrDefault(a => a.Id == userSessionDto.UserSessionId);
            if (session==null)
            {
                var userSession = new UserSession() { Id=userSessionDto.UserSessionId };
                _appDbContext.UserSessions.Add(userSession);
               await _appDbContext.SaveChangesAsync();
            }
            var item = _mapper.Map<CartItem>(userSessionDto);
            var item2 = _appDbContext.CartItems.Where(a => a.UserSessionId == userSessionDto.UserSessionId && a.ProductId == userSessionDto.ProductId && a.Size.Contains(userSessionDto.Size)&&a.OrderId==null).FirstOrDefault();
            if (item2 != null)
            {
                item2.Count += userSessionDto.Count;
                item2.Sum += userSessionDto.Sum;
                _appDbContext.CartItems.Update(item2);
               await _appDbContext.SaveChangesAsync();
                userSessionDto.Count = item2.Count;
                userSessionDto.Sum = item2.Sum;
            }
            else
            {
                _appDbContext.CartItems.Add(item);
                await _appDbContext.SaveChangesAsync();
            }
            return userSessionDto;
            
        }

        public async Task<UserSessionCartDto> SaveToCart(UserSessionCartDto userSessionDto, List<UserTextSessionDto> userTextSessionDto)
        {
            var session = _appDbContext.UserSessions.FirstOrDefault(a => a.Id == userSessionDto.UserSessionId);
            if (session == null)
            {
                var userSession = new UserSession() { Id = userSessionDto.UserSessionId };
                _appDbContext.UserSessions.Add(userSession);
               await _appDbContext.SaveChangesAsync();
            }
            var item = _mapper.Map<CartItem>(userSessionDto);
            _appDbContext.CartItems.Add(item);
           await _appDbContext.SaveChangesAsync();
            var textList = _mapper.Map<List<TextProperty>>(userTextSessionDto);
            textList.ForEach(a => a.CartItemId = item.Id);
            _appDbContext.TextProperties.AddRange(textList);
           await _appDbContext.SaveChangesAsync();
            return userSessionDto;
        }

        public async Task<UserSessionCartDto> UpdateCart(string sessionId, UserSessionCartDto userSessionCartDto)
        {
            var item = _appDbContext.CartItems.Where(a => a.UserSessionId == sessionId && a.ProductId == userSessionCartDto.ProductId && a.Size==userSessionCartDto.Size && a.OrderId==null).FirstOrDefault();
            if (item != null)
            {
                item.Count = userSessionCartDto.Count;
                item.Sum =item.Price*item.Count;
                userSessionCartDto.Price = item.Price;
                userSessionCartDto.Sum = item.Sum;
                userSessionCartDto.Size = item.Size;
            }
            await _appDbContext.SaveChangesAsync();

            return userSessionCartDto;
            
        }
    }
}
