using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.DiscountServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtosSevice.DiscountProtosSeviceClient _discountProtosSeviceClient;

        public DiscountGrpcService(DiscountProtosSevice.DiscountProtosSeviceClient discountProtosSeviceClient)
        {
            _discountProtosSeviceClient = discountProtosSeviceClient ?? throw new ArgumentNullException(nameof(discountProtosSeviceClient));
        }
        //passing the product name from basket controller
        public async Task<CouponModel> GetDiscount(string productName)// retun type is CouponModel because hee we used the client grpc obj/sevice
        {//prepare request
            var discountRequest = new GetDiscountRequest { ProductName = productName };//create awatable new quest of service typeS
            return await _discountProtosSeviceClient.GetDiscountAsync(discountRequest);// consuming grpc service as a client
        }
    }
}
