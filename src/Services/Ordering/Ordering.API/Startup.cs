using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ordering.Application;
using Ordering.Infrastructure;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Messages.Common;
using Ordering.API.EventBusConsumer;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);


            //we configure masstansit here becoz it required to connect to rabbitmq,required 2 parameters 1-context,2-configuation to make connection to rabbitmq
            // MassTransit-RabbitMQ Configuration
            services.AddMassTransit(config => //configure masstansit to connect to rabitmq
            {

                config.AddConsumer<BasketCheckoutConsumer>();//consume event
                config.UsingRabbitMq((ctx, cfg) =>  //this acts as action method where context fo ibusregistation,configuration for rabitmq
                {
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);//need to set url to make connection  ,config host 

                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                     {
                         c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);

                     });
                });
            });
            services.AddMassTransitHostedService(); //imp service ,need to work with rabbitmq and masstansit,this provided masstransit as working hosted service

            // General Configuration
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<BasketCheckoutConsumer>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
