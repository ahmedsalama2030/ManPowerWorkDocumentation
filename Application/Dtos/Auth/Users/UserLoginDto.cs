using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.Users
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "field-required")]
        [EmailAddress(ErrorMessage = "email-error")]

        public string Email { get; set; }
        [Required(ErrorMessage = "field-required")]
        public string Password { get; set; }
    }
}
