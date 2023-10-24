using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth.Users;
public class MarketerRegisterDto
{
        [Required(ErrorMessage = "field-required")]

        public string Name { get; set; }
          [Required(ErrorMessage = "field-required")]
        [EmailAddress(ErrorMessage = "email-error")]
        public string Email { get; set; }
          [Required(ErrorMessage = "field-required")]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

}
