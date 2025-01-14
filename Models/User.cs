using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace library_management_api.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
