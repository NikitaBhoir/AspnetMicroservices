using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;
using System;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<OrderContext>(options =>
            options.UseSqlServer(config.GetConnectionString("OrderingConnectionString")));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));//typeof used because this is requied for mediator,when  mediator cerates repo related objs it required for type convesion
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.Configure<EmailSettings>(e => config.GetSection("EmailSettings"));//get configuration from appsettings
            services.AddTransient<IEmailService, EmailService>();
            return services;
        }
    }
}
