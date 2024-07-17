using CategoryTask.Interface;
using CategoryTask.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CategoryTask.Service
{
    public class CategoryService : ICategory
    {
        private readonly CategoryProductDbContext categoryProductDb;

        public CategoryService(CategoryProductDbContext categoryProductDb)
        {
            this.categoryProductDb = categoryProductDb;
        }
        public async Task ActivateCategoryAsync(int id)
        {
            var category = await categoryProductDb.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsActive = true;
                await categoryProductDb.SaveChangesAsync();
                await ActivateProductsByCategoryIdAsync(id);

            }
        }

        private async Task ActivateProductsByCategoryIdAsync(int categoryId)
        {
            var products = await categoryProductDb.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            foreach (var product in products)
            {
                product.IsActive = true;
                categoryProductDb.Products.Update(product);
            }
            await categoryProductDb.SaveChangesAsync();
        }

        public async Task AddCategoryAsync(Category addCategory)
        {
            await categoryProductDb.Categories.AddAsync(addCategory);
            await categoryProductDb.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsyn(int id)
        {
            var category = await categoryProductDb.Categories.FindAsync(id);
            if (category != null)
            {
                categoryProductDb.Categories.Remove(category);
                await categoryProductDb.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task DeactivateCategoryAsync(int id)
        {
            var category = await categoryProductDb.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsActive = false;
                await categoryProductDb.SaveChangesAsync();
                await DeactivateProductsByCategoryIdAsync(id);

            }
        }

        private async Task DeactivateProductsByCategoryIdAsync(int categoryId)
        {
            var products = await categoryProductDb.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            foreach (var product in products)
            {
                product.IsActive = false;
                categoryProductDb.Products.Update(product);
            }
            await categoryProductDb.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize)
        {
            return await categoryProductDb.Categories.Skip((1 - pageNumber) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await categoryProductDb.Categories.FindAsync(id);
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category Updatecategory)
        {
            var category = await categoryProductDb.Categories.FindAsync(id);
            if (category != null)
            {


                category.Name = Updatecategory.Name;
                category.IsActive = Updatecategory.IsActive;
                //category.Products = Updatecategory.Products;

                await categoryProductDb.SaveChangesAsync();
                return true;



            }
            return false;
        }
    }
}
