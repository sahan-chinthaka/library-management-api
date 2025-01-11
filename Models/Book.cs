using System.ComponentModel.DataAnnotations;

namespace library_management_api.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Author { get; set; }
        public string? Description { get; set; }

        public string? ImageURl { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public string? Publisher { get; set; }
    }
}
