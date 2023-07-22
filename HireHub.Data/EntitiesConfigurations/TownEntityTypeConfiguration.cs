using HireHub.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireHub.Data.EntitiesConfigurations
{
    public class TownEntityTypeConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.HasData(SeedWithUSA());
            builder.HasData(SeedWithBulgaria());
        }

        private List<Town> SeedWithUSA()
        {
            var towns = new List<Town>
            {
                new Town
                {
                    Id = 1,
                    TownName = "New York City",
                    CountryId = 1,
                },
                new Town
                {
                    Id = 2,
                    TownName = "Los Angeles",
                },
                new Town
                {
                    Id = 3,
                    TownName = "Chicago",
                    CountryId = 1,
                },
                new Town
                {
                    Id = 4,
                    TownName = "Houston",
                     CountryId = 1,
                },
                new Town
                {
                    Id = 5,
                    TownName = "Phoenix",
                     CountryId = 1,
                },
                new Town
                {
                    Id = 6,
                    TownName = "Philadelphia",
                     CountryId = 1,
                },
                new Town
                {
                    Id = 7,
                    TownName = "San Antonio",
                     CountryId = 1,
                },
                new Town
                {
                    Id = 8,
                    TownName = "San Diego",
                    CountryId = 1,
                },
                new Town
                {
                    Id = 9,
                    TownName = "Dallas",
                    CountryId = 1,
                },
                new Town
                {
                    Id = 10,
                    TownName = "San Jose",
                    CountryId = 1,
                }};

            return towns;
        }

        private List<Town> SeedWithBulgaria()
        {
            var towns = new List<Town>()
            {
                new Town{
                    Id = 11,
                    TownName = "Sofia",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 12,
                    TownName = "Plovdiv",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 13,
                    TownName = "Varna",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 14,
                    TownName = "Burgas",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 15,
                    TownName = "Ruse",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 16,
                    TownName = "Stara Zagora",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 17,
                    TownName = "Pleven",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 18,
                    TownName = "Sliven",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 19,
                    TownName = "Dobrich",
                    CountryId = 2,
                },
                new Town
                {
                    Id = 20,
                    TownName = "Shumen",
                    CountryId = 2,
                }
            };

            return towns;
        }
    }
}