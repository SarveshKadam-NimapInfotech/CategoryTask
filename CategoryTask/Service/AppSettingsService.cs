using CategoryTask.Interface;
using CategoryTask.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace CategoryTask.Service
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly CategoryProductDbContext _context;

        public AppSettingsService(CategoryProductDbContext context)
        {
            _context = context;
        }


        public async Task<bool> GetUseApiFlagAsync()
        {
            var setting = await _context.AppSettings.FirstOrDefaultAsync();
            return setting.UseApi;
        }

    }
}
