using System.ComponentModel.DataAnnotations;

namespace CategoryTask.Models.Data
{
    public class AppSetting
    {
        [Key]
        public int Id { get; set; }
        public bool UseApi { get; set; }
    }
}