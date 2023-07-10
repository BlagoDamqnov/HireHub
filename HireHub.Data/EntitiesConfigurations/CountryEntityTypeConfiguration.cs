using HireHub.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireHub.Data.EntitiesConfigurations
{
    public class CountryEntityTypeConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(Seed());
        }
        private List<Country> Seed()
        {
            var countries = new List<Country>()
        {
            new Country
            {
                Id = 1,
                CountryName = "USA",
            },
            new Country
            {
                Id = 2,
                CountryName = "Bulgaria",
            },
        };

            return countries;
        }
    }
}
