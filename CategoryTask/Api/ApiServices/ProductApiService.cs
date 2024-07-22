using CategoryTask.Models.Data;
using CategoryTask.Models.ViewModel;

namespace CategoryTask.Api.ApiServices
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<Product>> GetProductsAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync($"/api/product?pageNumber={pageNumber}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<Product>>();
            return result.Products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/product/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<Product>>();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task CreateProductAsync(AddProductRequest addProduct)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/product", addProduct);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProductAsync(EditProductRequest editProduct)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/product/{editProduct.Id}", editProduct);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(deleteProductRequest deleteProduct)
        {
            var response = await _httpClient.DeleteAsync($"/api/product/{deleteProduct.Id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
