using CategoryTask.Interface;
using CategoryTask.Models.Data;
using CategoryTask.Models.ViewModel;
using CategoryTask.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CategoryTask.Api.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategory _iCategoryService;

        public CategoryApiController(ICategory iCategoryService)
        {
            _iCategoryService = iCategoryService;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var categories = await _iCategoryService.GetAllCategoriesAsync(pageNumber, pageSize);
                return Ok(new { categories });
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
                var category = await _iCategoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound($"Category with Id = {id} not found");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // POST: api/CategoryApi
        [HttpPost]
        public async Task<IActionResult> Create(addCategoryRequest addCategoryRequest)
        {
            try
            {
                var categry = new Category
                {
                    Name = addCategoryRequest.Name,
                    IsActive = addCategoryRequest.IsActive,
                };
                await _iCategoryService.AddCategoryAsync(categry);
                return Ok(categry);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        // PUT: api/CategoryApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category model)
        {
            if (id != model.Id)
            {
                return BadRequest("Category ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingCategory = await _iCategoryService.GetByIdAsync(id);
                if (existingCategory != null)
                {
                    var editRequest = new editCategoryRequest
                    {
                        Id = id,
                        IsActive = existingCategory.IsActive,
                        Name = existingCategory.Name,
                    };
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // DELETE: api/CategoryApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(deleteViewModel deleteViewModel)
        {
            try
            {
                var result = await _iCategoryService.DeleteCategoryAsyn(deleteViewModel.Id);
               
                return NoContent();
                

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
