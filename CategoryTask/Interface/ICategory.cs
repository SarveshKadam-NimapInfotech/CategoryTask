using CategoryTask.Models.Data;

namespace CategoryTask.Interface
{
    public interface ICategory
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(int pageNumber, int pageSize);
        Task<Category> GetByIdAsync(int id);
        Task AddCategoryAsync(Category addCategory);
        Task<bool> UpdateCategoryAsync(int id, Category Updatecategory);
        Task<bool> DeleteCategoryAsyn(int id);
        Task DeactivateCategoryAsync(int id);
        Task ActivateCategoryAsync(int id);
    }
}
