using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength=4,ErrorMessage = "You must specify password between 4 and 8")]
        public string password { get; set; }
    }
}