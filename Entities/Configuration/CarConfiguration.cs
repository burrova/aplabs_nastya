using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasData
            (
            new Car
            {
                Id = new Guid("19d4c053-49b6-410c-bc78-2d54a9991870"),
                Brand = "Mitsubishi",
                Name = "Outlander",
                Horsepower = 200,
                Type = "Vnedorozhnik",
                Price = 1400000
            },
            new Car
            {
                Id = new Guid("29d4c053-49b6-410c-bc78-2d54a9991870"),
                Brand = "Toyota",
                Name = "Corolla",
                Horsepower = 190,
                Type = "Universal",
                Price = 900000
            }
            );
        }
    }
}
