using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        //when we refer base class with constuctor with parameters then you need to create constuctor with paramete here also with sub classes ,this is oops concept
        public OrderRepository(OrderContext _orderdbContext) : base(_orderdbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await orderdbContext.Orders
                .Where(o => o.UserName == userName)
                .ToListAsync();
               
            return orderList;
        }
    }
}
 /*public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                                .Where(o => o.UserName == userName)
                                .ToListAsync();
            return orderList;
        }
    }
*/