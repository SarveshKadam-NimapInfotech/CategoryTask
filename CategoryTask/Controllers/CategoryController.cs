using CategoryTask.Api.ApiServices;
using CategoryTask.Interface;
using CategoryTask.Models.Data;
using CategoryTask.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CategoryTask.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategory _category;
        private readonly CategoryApiService _categoryApiService;
        private readonly IAppSettingsService _appSettingsService;
        private bool _useApi;

        public CategoryController(ICategory category, CategoryApiService categoryApiService, IAppSettingsService appSettingsService)
        {
            _category = category;
            _categoryApiService = categoryApiService;
            _appSettingsService = appSettingsService;

        }

        private async Task InitializeSettingsAsync()
        {
            _useApi = await _appSettingsService.GetUseApiFlagAsync();
        }

        [HttpGet]
        public async Task<IActionResult> ListCategory(int pageNo = 1, int PageSize = 10)
        {
            await InitializeSettingsAsync();
            IEnumerable<Category> categories;
            int totalCategories;
            if (_useApi)
            {
                var response = await _categoryApiService.GetCategoriesAsync(pageNo, PageSize);
                categories = response;
                totalCategories = categories.Count();
            }
            else
            {
                 categories = await _category.GetAllCategoriesAsync(pageNo, PageSize);

            }

                return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> AddCategory()
        {
            await InitializeSettingsAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(addCategoryRequest addCategoryRequest)
        {
            await InitializeSettingsAsync();
            if (_useApi)
            {
                await _categoryApiService.CreateCategoryAsync(addCategoryRequest);
            }
            else
            {
                var categry = new Category
                {
                    Name = addCategoryRequest.Name,
                    IsActive = addCategoryRequest.IsActive,
                };
                await _category.AddCategoryAsync(categry);

            }

            return Redirect("List");
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            await InitializeSettingsAsync();
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            Category category;
            if (_useApi)
            { 
                category = await _categoryApiService.GetCategoryByIdAsync(id);
            }
            else
            {
                category = await _category.GetByIdAsync(id);
            }

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
            await InitializeSettingsAsync();
            if (ModelState.IsValid)
            {
                Category category;
                if (_useApi)
                {
                    await _categoryApiService.UpdateCategoryAsync(editCategoryRequest);
                }
                else
                { 
                    category = new Category
                    {
                        Name = editCategoryRequest.Name,
                        IsActive = editCategoryRequest.IsActive,
                        Id = editCategoryRequest.Id
                    };
                    var result = await _category.UpdateCategoryAsync(editCategoryRequest.Id, category);
                }

            }
                return RedirectToAction("List");


        }
        [HttpPost]
        public async Task<IActionResult> Delete(deleteViewModel deleteViewModel)
        {
            await InitializeSettingsAsync();
            
            if (_useApi)
            {
                await _categoryApiService.DeleteCategoryAsync(deleteViewModel);
            }
            else
            {
                await _category.DeleteCategoryAsyn(deleteViewModel.Id);

            }
            
            return RedirectToAction("Edit");
        }
       
    }
}
