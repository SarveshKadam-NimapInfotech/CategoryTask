namespace CategoryTask.Models.ViewModel
{
    public class deleteProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
