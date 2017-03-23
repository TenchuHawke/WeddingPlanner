using System;

using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models
{
    public class UserValidation : User
    {
        [Required]
        [MinLength(2)]
        public new string Name { get; set; }
        [Required]
        [EmailAddress]
        public new string Email { get; set; }
        [Required]
        [DataTypeAttribute(DataType.Password)]
        public new string Password { get; set; }
        [CompareAttribute("Password", ErrorMessage = "Password and confirmation must mach.")]
        public string PasswordConfirm { get; set; }
        public User ToUser()
        {
            User NewUser = new User
            {
                Name = this.Name,
                    Email = this.Email.ToLower(),
                    Password = this.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
            };
            return NewUser;
        }
    }
}