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
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
           builder.HasData(Seed());
        }

        private List<Category> Seed()
        {
            var categories = new List<Category>()
            {
                new Category
                {
                    Id = 1,
                    CategoryName = "Full-Time"
                },
                new Category
                {
                    Id = 2,
                    CategoryName = "Part-Time"
                }
            };

            return categories;
        }
    }
}
