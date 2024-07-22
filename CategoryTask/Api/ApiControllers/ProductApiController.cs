using CategoryTask.Interface;
using CategoryTask.Models.Data;
using CategoryTask.Models.ViewModel;
using CategoryTask.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CategoryTask.Api.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProduct _iProductService;
        private readonly ICategory _iCategoryService;

        public ProductApiController(IProduct iProductService, ICategory iCategoryService)
        {
            _iProductService = iProductService;
            _iCategoryService = iCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var products = await _iProductService.GetAllAsync(pageNumber, pageSize);
                return Ok(new { products });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _iProductService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound("Product with Id {id} not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProductRequest addProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var products = new Product
                {
                    Name = addProduct.Name,
                    IsActive = addProduct.IsActive,
                    CategoryId = addProduct.CategoryId
                };
                await _iProductService.AddAsync(products);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, int pageNo = 1, int pageSize = 10)
        {
            var products = await _iProductService.GetByIdAsync(id);
            if (products == null)
            {
                // Log that the product was not found
                return RedirectToAction("List");
            }

            var categories = await _iCategoryService.GetAllCategoriesAsync(pageNo, pageSize);
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
            return NoContent();
        }
           

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(deleteProductRequest deleteProduct)
        {

            try
            {

                var result = await _iProductService.DeleteAsync(deleteProduct.Id);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
