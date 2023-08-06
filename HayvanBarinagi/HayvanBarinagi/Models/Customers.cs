using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HayvanBarinagi.Models
{
    public class Customers : IdentityUser
    {
        [Required]
        public string NameSurname { get; set; }
        public string? About { get; set; }
    }
}
