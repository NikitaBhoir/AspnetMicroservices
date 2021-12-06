using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {//TContext indicates it take context object which we have declare in infrastructure
        //here we pass as action method in parameter becoz we peform seeding action after adding migration
        public static IHost MigrateDatabase<TContext>(this IHost host,Action<TContext, IServiceProvider>seeder,int? retry=0) where TContext : DbContext
        {
            int retryFoAvailability = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {

                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                    if (retryFoAvailability < 50)
                    {
                        System.Threading.Thread.Sleep(2000);//2 sec
                        MigrateDatabase<TContext>(host, seeder, retryFoAvailability);
                    }
                }

            }

            return host;
        }
        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
         where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);//to seed data after adding migration
        }

        }
}
