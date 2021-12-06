using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)//create extension method
        {
            //add extensions
            services.AddAutoMapper(Assembly.GetExecutingAssembly());//check if there is any class inherited from profile class
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); //check if there is any validation
            services.AddMediatR(Assembly.GetExecutingAssembly());//check if there is any request and requesthandler class exist

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }
    }
}
