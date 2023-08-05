using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HayvanBarinagi.Models
{
    public class Customers : IdentityUser
    {
        [Required]
        public long IDNo { get; set; }
        [Required]
        public string NameSurname { get; set; }
        public string? Address { get; set; }
    }
}
