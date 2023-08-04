using System.ComponentModel.DataAnnotations;

namespace HayvanBarinagi.Models
{
    public class AnimalType
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Animal Type name is not null!")]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
