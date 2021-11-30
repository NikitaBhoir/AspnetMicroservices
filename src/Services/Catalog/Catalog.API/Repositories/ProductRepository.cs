using Catalog.API.Data.Iterfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _Context;//obj of mongodb drivers ,it contains all cli commands

        public ProductRepository(ICatalogContext catalogContext)
        {
            _Context = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }


        public async Task CreateProduct(Product productentity)
        {
            await _Context.Products.InsertOneAsync(productentity);
        }

        public async Task<bool> UpdateProduct(Product productentity)
        {
            var UpdateResult = await _Context.Products.ReplaceOneAsync(filter: g => g.Id == productentity.Id, replacement: productentity);

            return UpdateResult.IsAcknowledged && UpdateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filterData = Builders<Product>.Filter.Eq(p => p.Id, id);//elemMatch used to match name with elements in mongodb
            DeleteResult deleteResult = await _Context.Products.DeleteOneAsync(filterData);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;//filter records
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _Context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filterData = Builders<Product>.Filter.Eq(p => p.Category, categoryName);//elemMatch used to match name with elements in mongodb
            return await _Context.Products.Find(filterData).ToListAsync();//filter records
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filterData = Builders<Product>.Filter.Eq(p => p.Name, name);//elemMatch used to match name with elements in mongodb
            return await _Context.Products.Find(filterData).ToListAsync();//filter records
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _Context.Products.Find(p => true).ToListAsync();
        }


    }
}
