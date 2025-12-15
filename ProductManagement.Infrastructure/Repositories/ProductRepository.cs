using MongoDB.Driver;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Product>("Products");
        }

        public async Task<List<Product>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetByIdAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product product) =>
            await _collection.InsertOneAsync(product);

        public async Task UpdateAsync(Product product) =>
            await _collection.ReplaceOneAsync(x => x.Id == product.Id, product);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }

}
