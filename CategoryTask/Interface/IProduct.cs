using CategoryTask.Models.Data;

namespace CategoryTask.Interface
{
    public interface IProduct
    {
        Task<IEnumerable<Product>> GetAllAsync(int pageNumber, int pageSize);
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product addproduct);
        Task<bool> UpdateAsync(Product updateProduct, int id);
        Task<bool> DeleteAsync(int id);
    }
}
