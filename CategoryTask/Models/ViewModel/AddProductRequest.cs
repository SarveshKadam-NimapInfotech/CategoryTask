namespace CategoryTask.Models.ViewModel
{
    public class AddProductRequest
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public int CategoryId { get; set; }
    }
}
