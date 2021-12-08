using AutoMapper;
using Basket.API.DiscountServices;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
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
        private readonly IBasketRepository _Repository;     //when we inject any repo  or service here with constructor we need to register them in startup project under the service config  
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcServices, IMapper mapper,IPublishEndpoint publishEndpoint)
        {
            this._Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._discountGrpcService = discountGrpcServices;
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
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


        [Route("[action]")]//need to right chackout in url to perform checkout operation
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {//Todo:
            // get existing basket with total price            
            // perfrom basket checkout event , then bsketCheckoutevent= Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket, becoz when once we send basketcheckout event to the queue this will be consume by odeing microsevices and it going to create new ode there with given basket info.

            // get existing basket with total price
            var basket = await _Repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // send checkout event to rabbitmq
            //Map checkoutevent to basketcheckoutevent
            var eventmessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventmessage.TotalPrice = basket.TotalPrice;
            //eventbus.Publish basketcheckoutevent using masstransit
            await _publishEndpoint.Publish(eventmessage);//publishing message to the rabbit mq



            // remove the basket
            await _Repository.DeleteBasket(basketCheckout.UserName);
            return Accepted();
        }
    }
}                   
