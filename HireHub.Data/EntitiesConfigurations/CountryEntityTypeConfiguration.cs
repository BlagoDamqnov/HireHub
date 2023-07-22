using HireHub.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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