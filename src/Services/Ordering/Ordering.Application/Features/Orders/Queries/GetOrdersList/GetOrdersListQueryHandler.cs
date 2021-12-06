using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    //this class trigered when request comes
    class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>//specify request obj type and expected response type
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {//this handle class triggered when request comes to GetOrdersListQuery class

            var orderList =await  _orderRepository.GetOrdersByUserName(request.UserName);//return enumerable orderlist
            return _mapper.Map<List<OrdersVm>>(orderList);//map order entity to our dto ordervm obj model

        }
    }
}
