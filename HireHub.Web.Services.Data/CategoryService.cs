using HireHub.Data;
using HireHub.Web.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Web.Services.Data
{
    using HireHub.Data.Entities;
    using HireHub.Web.ViewModels.Categories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(CreateVM model)
        {
            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                throw new InvalidOperationException("Category name is required!");
            }
            var isExistByName = await _context.Categories.AnyAsync(c => c.CategoryName.Trim().ToLower() == model.CategoryName.Trim().ToLower());
            if (isExistByName)
            {
                throw new InvalidOperationException("Category already exist!");
            }

            var category = new Category
            {
                CategoryName = model.CategoryName.Trim()
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetAllCategoryNames()
        {
            var names = await _context
                .Categories
                .Select(c => c.CategoryName)
                .ToListAsync();

            return names;
        }

        public Task<bool> IsExist(int id)
        {
            var isExist = _context.Categories.AnyAsync(c => c.Id == id);

            return isExist;
        }
    }
}