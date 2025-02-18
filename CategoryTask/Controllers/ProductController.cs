﻿using CategoryTask.Api.ApiServices;
using CategoryTask.Interface;
using CategoryTask.Models.Data;
using CategoryTask.Models.ViewModel;
using CategoryTask.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CategoryTask.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct _product;
        private readonly ICategory _category;
        private readonly IAppSettingsService _appSettingsService;
        private readonly ProductApiService _productApiService;
        private bool _useApi;

        public ProductController(IProduct product, ICategory category, IAppSettingsService appSettingsService, ProductApiService productApiService)
        {
            this._product = product;
            this._category = category;
            _appSettingsService = appSettingsService;
            _productApiService = productApiService;

        }
        private async Task InitializeSettingsAsync()
        {
            _useApi = await _appSettingsService.GetUseApiFlagAsync();
        }

        [HttpGet]
        public async Task<IActionResult> ListProduct(int pageNumber = 1, int pageSize = 10)
        {
            await InitializeSettingsAsync();

            IEnumerable<Product> products;
            if (_useApi)
            {
                var response = await _productApiService.GetProductsAsync(pageNumber, pageSize);
                products = response;
            }
            else
            { 
                 products = await _product.GetAllAsync(pageNumber, pageSize);
            }

            return View(products);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductRequest addProduct)
        {
            await InitializeSettingsAsync();
            Product products;
            if (_useApi)
            {
                await _productApiService.CreateProductAsync(addProduct);
            }
            else
            { 
            
                products = new Product
                {
                    Name = addProduct.Name,
                    IsActive = addProduct.IsActive,
                    CategoryId = addProduct.CategoryId
                };
                await _product.AddAsync(products);
            }
            return RedirectToAction("List");
        }
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id, int pageNo = 1, int pageSize = 10)
        {
            await InitializeSettingsAsync();
            var products = await _product.GetByIdAsync(id);
            if (products == null)
            {
                // Log that the product was not found
                return RedirectToAction("List");
            }

            Product product;
            EditProductRequest editProduct;
            if (_useApi)
            {
                product = await _productApiService.GetProductByIdAsync(id);
            }
            else
            { 
                    var categories = await _category.GetAllCategoriesAsync(pageNo, pageSize);
                if (categories == null)
                {
                    // Log that categories were not found
                    return RedirectToAction("List");
                }

                editProduct = new EditProductRequest
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
            }

            return View("List");
        }



        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductRequest editProduct)
        {
            await InitializeSettingsAsync();

            if (_useApi)
            {
                await _productApiService.UpdateProductAsync(editProduct);
            }
            else
            { 
                var products = new Product
                {
                    Id = editProduct.Id,
                    Name = editProduct.Name,
                    CategoryId = editProduct.CategoryId,
                    IsActive = editProduct.IsActive,


                };
                var result = await _product.UpdateAsync(products, editProduct.Id);
            }
            return View("List");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(deleteProductRequest deleteProduct)
        {
            await InitializeSettingsAsync();

            if (_useApi)
            {
                await _productApiService.DeleteProductAsync(deleteProduct);
            }
            else
            { 
                await _product.DeleteAsync(deleteProduct.Id);
            }
            
            return View("List");
        }
    }
}
