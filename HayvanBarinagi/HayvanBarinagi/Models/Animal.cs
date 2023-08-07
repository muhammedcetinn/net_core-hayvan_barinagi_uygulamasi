using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HayvanBarinagi.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Animal name is not null!")]
        [MaxLength(20)]
        public string Name { get; set; }
        [Range(0, 12, ErrorMessage = "Age Must be between 0 to 12")]
        public string Age { get; set; }
        [Required]
        public string Features { get; set; }
        [ValidateNever]
        public int AnimalTypeId { get; set; }
        [ForeignKey("AnimalTypeId")]
        [ValidateNever]
        public AnimalType AnimalType { get; set; }
        [ValidateNever]
        public string ImageURL { get; set; }
        [DefaultValue(false)]
        public bool isRequest { get; set; }
        [DefaultValue(false)]
        public bool Status { get; set; }
        [DefaultValue("NULL")]
        public string Recipient { get; set; }
        [DefaultValue("NULL")]
        public string RecipientAbout { get; set; }

    }
}
