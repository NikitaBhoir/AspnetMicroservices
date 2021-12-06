using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    //when request commons to meadiator then 1 st is validate the command  request and then handle the request
    class CheckoutOrderCommandValidator:AbstractValidator<CheckoutOrderCommand>//here 1st we validate the CheckoutOrderCommand before ecute the command handler
    {
        public CheckoutOrderCommandValidator()
        {//by using rulefor specify validation rule for specific property
            RuleFor(p => p.UserName)//provide expession
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(p=>p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than zero.");

        }
    }
}
