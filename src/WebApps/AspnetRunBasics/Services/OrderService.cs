using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class OrderService: IOrderService
    {
        private readonly HttpClient httpClient;
        //this client now looks for orderservice which we have configured in startup
        public OrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var response = await httpClient.GetAsync($"/Order/{userName}");    //consume internal microservices
            return await response.ReadContentAs<List<OrderResponseModel>>();
        }
    }
}
