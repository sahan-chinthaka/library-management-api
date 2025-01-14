namespace library_management_api.Models
{
    public class BookDto
    {
        public required string Name { get; set; }

        public required string Author { get; set; }

        public string? Description { get; set; }

        public string? ImageURl { get; set; }

        public required DateTime CreatedDate { get; set; }

        public string? Publisher { get; set; }
    }
}
