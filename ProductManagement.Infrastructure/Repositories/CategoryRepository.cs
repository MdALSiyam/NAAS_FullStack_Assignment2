using MongoDB.Driver;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _collection;

        public CategoryRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Category>("Categories");
        }

        public async Task<List<Category>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Category?> GetByIdAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Category category) =>
            await _collection.InsertOneAsync(category);

        public async Task UpdateAsync(Category category) =>
            await _collection.ReplaceOneAsync(x => x.Id == category.Id, category);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}