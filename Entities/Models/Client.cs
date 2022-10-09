using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Client
    {
        [Column("ClientId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Client name is a required field.")]
        [MaxLength(15, ErrorMessage = "Maximum length for the name is 15 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the surname is 30 characters.")]
        public string Surname { get; set; }
        public int Age { get; set; }

        [Required(ErrorMessage = "City is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the city is 30 characters.")]
        public string City { get; set; }
    }
}
