﻿using ETicaretAPI.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        public Task CreateOrderAsync(CreateOrder createOrder);
        Task<ListOrder> GetAllOrdersAsync(int page,int size);
        Task<SingleOrder> GetOrderByIdAsync(string Id);
        Task<(bool,CompletedOrderDto)> CompletedOrderAsync(string id);
    }
}
