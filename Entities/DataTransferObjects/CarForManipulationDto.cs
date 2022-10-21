using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract class CarForManipulationDto
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Horsepower { get; set; }
        public double Price { get; set; }
    }
}
