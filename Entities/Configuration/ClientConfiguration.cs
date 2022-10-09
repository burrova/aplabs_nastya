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
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasData
            (
            new Client
            {
                Id = new Guid("89d4c053-49b6-410c-bc78-2d54a9991870"),
                Name = "Nastya",
                Surname = "Burova",
                City = "Sochi",
            },
            new Client
            {
                Id = new Guid("99d4c053-49b6-410c-bc78-2d54a9991870"),
                Name = "Nastya",
                Surname = "Vlasenkova",
                City = "Saransk",
            }
            );
        }
    }
}
