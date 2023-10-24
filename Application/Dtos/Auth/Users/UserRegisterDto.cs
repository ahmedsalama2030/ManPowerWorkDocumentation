using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Auth.Users;
using Core.Enums;

namespace Application.Dtos.Users
{
    public class UserRegisterDto
    {

        [Required(ErrorMessage = "field-required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "field-required")]
        [EmailAddress(ErrorMessage = "email-error")]
        public string Email { get; set; }

        [Required(ErrorMessage = "field-required")]
        public string Password { get; set; }

         public int UserType { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserRoleRegisterDto> UserRole { get; set; }
        public string RoleName { get; set; }



    }
}
