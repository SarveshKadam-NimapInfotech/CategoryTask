using Microsoft.AspNetCore.Mvc.Rendering;

namespace CategoryTask.Models.ViewModel
{
    public class EditProductRequest
    {
        public int Id { get; set; }


        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
