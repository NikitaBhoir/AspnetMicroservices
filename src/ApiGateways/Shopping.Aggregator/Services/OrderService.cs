using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class OrderService: IOrderService
    {
        private readonly HttpClient httpClient;
        //this client now looks for ordersevice which we have configured in statup
        public OrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var response = await httpClient.GetAsync($"/api/v1/Order/{userName}");//consume internal microservices
            return await response.ReadContentAs<List<OrderResponseModel>>();
           
        }
    }
}
