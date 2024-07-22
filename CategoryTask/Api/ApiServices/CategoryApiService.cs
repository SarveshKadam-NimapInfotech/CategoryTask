using CategoryTask.Models.Data;
using CategoryTask.Models.ViewModel;

namespace CategoryTask.Api.ApiServices
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient.GetAsync($"/api/category?pageNumber={pageNumber}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<Category>>();
            return result.Categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/category/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Category>();
        }


        public async Task CreateCategoryAsync(addCategoryRequest addCategoryRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/category", addCategoryRequest);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCategoryAsync(editCategoryRequest editCategoryRequest)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/category/{editCategoryRequest.Id}", editCategoryRequest);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(deleteViewModel deleteViewModel)
        {
            var response = await _httpClient.DeleteAsync($"/api/category/{deleteViewModel.Id}");
            response.EnsureSuccessStatusCode();
        }
    }

    public class ApiResponse<T>
    {
        public IEnumerable<T> Categories { get; set; }
        public int TotalCategories { get; set; }

        public IEnumerable<T> Products { get; set; }
        public int TotalProducts { get; set; }
    }
}
