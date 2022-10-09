using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Car
    {
        [Column("CarId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Car brand is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the brand is 20 characters.")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Car name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the name is 60 characters")]
        public string Name { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = "Horsepower is a required field.")]
        public int Horsepower { get; set; }
        public double Price { get; set; }
    }
}
