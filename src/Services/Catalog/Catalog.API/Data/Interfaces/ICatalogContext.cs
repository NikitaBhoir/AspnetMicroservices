using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data.Iterfaces
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; } //poperty to stores mongo data collections
    }
}
