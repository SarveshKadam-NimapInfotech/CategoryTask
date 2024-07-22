using Microsoft.EntityFrameworkCore;

namespace CategoryTask.Models.Data
{
    public class CategoryProductDbContext:DbContext
    {
        public CategoryProductDbContext(DbContextOptions<CategoryProductDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<AppSetting> AppSettings { get; set; }
    }
}
