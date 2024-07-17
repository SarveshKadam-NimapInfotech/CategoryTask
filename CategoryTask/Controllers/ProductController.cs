using CategoryTask.Interface;
using CategoryTask.Models.Data;
using CategoryTask.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CategoryTask.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _product;
        private readonly ICategory _category;

        public ProductController(IProduct product, ICategory category)
        {
            this._product = product;
            this._category = category;
            
        }

        [HttpGet]
        public async Task<IActionResult> ListProduct(int pageNumber = 1, int pageSize = 10)
        {
            var products = await _product.GetAllAsync(pageNumber, pageSize);
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductRequest addCategory)
        {
            var products = new Product
            {
                Name = addCategory.Name,
                IsActive = addCategory.IsActive,
                CategoryId = addCategory.CategoryId
            };
            await _product.AddAsync(products);
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id, int pageNo = 1, int pageSize = 10)
        {
            var products = await _product.GetByIdAsync(id);
            if (products == null)
            {
                // Log that the product was not found
                return RedirectToAction("List");
            }

            var categories = await _category.GetAllCategoriesAsync(pageNo, pageSize);
            if (categories == null)
            {
                // Log that categories were not found
                return RedirectToAction("List");
            }

            var editProduct = new EditProductRequest
            {
                Id = id,
                IsActive = products.IsActive,
                Name = products.Name,
                CategoryId = products.CategoryId,
                Categories = categories.Select(category => new SelectListItem
                {
                    Text = category.Name ?? "No Name", // Handle null category name
                    Value = category.Id.ToString() ?? string.Empty // Ensure this is a string
                }).ToList()
            };
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(editProduct);
        }



        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductRequest editProduct)
        {
            var products = new Product
            {
                Id = editProduct.Id,
                Name = editProduct.Name,
                CategoryId = editProduct.CategoryId,
                IsActive = editProduct.IsActive,


            };
            var result = await _product.UpdateAsync(products, editProduct.Id);
            if (result == true)
            {
                return RedirectToAction("List");
            }
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Delete(deleteProductRequest deleteProduct)
        {
            var result = await _product.DeleteAsync(deleteProduct.Id);
            if (result == true)
            {
                return RedirectToAction("List");
            }
            return View(result);
        }
    }
}
