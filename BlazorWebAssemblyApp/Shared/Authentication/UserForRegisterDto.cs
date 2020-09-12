using System;
using System.ComponentModel.DataAnnotations;namespace BlazorWebAssemblyApp.Shared{
    public class UserForRegisterDto
    {
        [Required]
        [Display(Name = "Username")]
        public string  Username { get; set; }        
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters!")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "KnownAs")]
        public string KnownAs { get; set; }
        [Required]
        [Display(Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}