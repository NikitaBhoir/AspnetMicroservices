using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviours
{
    class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {//this class executes all validations present in our project , if there is any error in validation then throws custom exceptions
        private readonly IEnumerable<IValidator<TRequest>> validators;  //collect all validato obj

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;   //inject validator in behaviour
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) //next to call next method to execute next behaviour afternone behaviou
        {
            if (validators.Any())   //if there is any fluentVallidation in my application then peform that validations
            {
                var context = new ValidationContext<TRequest>(request);     //ValidationContext also provided by fluent validation,alidate specific instance

                var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));    //actual validation oeation handle here with task obj
                var failures = validationResults.SelectMany(er => er.Errors).Where(f => f != null).ToList();    //checking any validation failure is there
                if (failures.Count != 0)
                    throw new ValidationException(failures);
                //basically this is by default fluentvalidation exception but in ou poject we created ou own exception so we indicate that  in namespace to use our own custom validationexceptions
            }
            return await next();    //imp, without this cannt procced to next mediato pipeline methods
        }
    }
}
