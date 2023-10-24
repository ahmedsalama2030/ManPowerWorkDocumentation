using System;
using System.ComponentModel.DataAnnotations;
using Application.Dtos.Auth.Users;
using Core.Enums;


namespace Application.Dtos.Users
{
    public class UserEditDto
    {
        [Required(ErrorMessage = "field-required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "field-required")]
        [EmailAddress(ErrorMessage = "email-error")]
        public string Email { get; set; }
        public string JobTitle { get; set; }
         public int UserType { get; set; }
        public string Degree { get; set; }
 
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
 
    }
}