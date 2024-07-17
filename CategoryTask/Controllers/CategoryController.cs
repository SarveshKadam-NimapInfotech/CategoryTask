using CategoryTask.Interface;
using CategoryTask.Models.Data;
using CategoryTask.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CategoryTask.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategory _category;

        private readonly HttpClient client;

        public CategoryController(ICategory category)
        {
            _category = category;
            
        }

        [HttpGet]
        public async Task<IActionResult> ListCategory(int pageNo = 1, int PageSize = 10)
        {
            var categories = await _category.GetAllCategoriesAsync(pageNo, PageSize);
            
            return View(categories);




        }
        [HttpGet]
        public async Task<IActionResult> AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(addCategoryRequest addCategoryRequest)
        {
            var categry = new Category
            {
                Name = addCategoryRequest.Name,
                IsActive = addCategoryRequest.IsActive,
            };
            await _category.AddCategoryAsync(categry);
            return Redirect("List");
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _category.GetByIdAsync(id);
            if (category != null)
            {
                var editRequest = new editCategoryRequest
                {
                    Id = id,
                    IsActive = category.IsActive,
                    Name = category.Name,
                };
                return View(editRequest);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(editCategoryRequest editCategoryRequest)
        {
            var category = new Category
            {
                Name = editCategoryRequest.Name,
                IsActive = editCategoryRequest.IsActive,
                Id = editCategoryRequest.Id
            };
            var result = await _category.UpdateCategoryAsync(editCategoryRequest.Id, category);
            return RedirectToAction("List");



        }
        [HttpPost]
        public async Task<IActionResult> Delete(deleteViewModel deleteViewModel)
        {
            var result = await _category.DeleteCategoryAsyn(deleteViewModel.Id);
            if (result == true)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");
        }
       
    }
}
