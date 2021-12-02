using Basket.API.DiscountServices;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        //dependancy injections
        private readonly IBasketRepository _Repository;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcServices)
        {
            this._Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._discountGrpcService = discountGrpcServices;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _Repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));//if basket is null created the new shopping cart
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // Communicate with Discount.Grpc and calculate lastest prices of products into shhopping cart
            //consume discount grpc
            foreach(var item in basket.Items)
            {// communicate with discount gpc service when iterating over the basket items
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);// get coupon info, passing this product name from controller to service
                item.Price -= coupon.Amount;// so when discount coupon found diduct the value from actual item price to get discount
            }
            return Ok(await _Repository.UpdateBasket(basket));
        }
            [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _Repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
