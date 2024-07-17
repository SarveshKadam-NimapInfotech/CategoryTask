using CategoryTask.Interface;
using CategoryTask.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CategoryTask.Service
{
    public class ProductService : IProduct
    {
        private readonly CategoryProductDbContext categoryProductDb;

        public ProductService(CategoryProductDbContext categoryProductDb)
        {
            this.categoryProductDb = categoryProductDb;
            
        }
        public async Task AddAsync(Product addproduct)
        {
            await categoryProductDb.Products.AddAsync(addproduct);
            await categoryProductDb.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Products = await categoryProductDb.Products.FindAsync(id);
            if (Products != null)
            {
                categoryProductDb.Products.Remove(Products);
                await categoryProductDb.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await categoryProductDb.Products
                            .Include(p => p.Category)
                            .Where(p => p.Category.IsActive == true)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await categoryProductDb.Products.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Product updateProduct, int id)
        {
            var products = await categoryProductDb.Products.FindAsync(id);
            if (products != null)
            {
                products.Name = updateProduct.Name;
                products.Id = id;
                products.CategoryId = updateProduct.CategoryId;
                products.IsActive = updateProduct.IsActive;
                await categoryProductDb.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
