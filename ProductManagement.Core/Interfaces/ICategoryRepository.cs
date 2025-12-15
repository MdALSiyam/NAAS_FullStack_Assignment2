using ProductManagement.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(string id);
        Task CreateAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(string id);
    }
}