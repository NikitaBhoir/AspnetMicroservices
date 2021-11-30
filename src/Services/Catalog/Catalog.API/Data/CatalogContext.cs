using Catalog.API.Data.Iterfaces;
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        //public IMongoCollection<Product> Products => throw new NotImplementedException();
        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));//create mongoclient class which is provide by mongodiver ,// it returns mongodb://localhost:27017
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));//returns CatalogDb

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));//returns Product collection
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
