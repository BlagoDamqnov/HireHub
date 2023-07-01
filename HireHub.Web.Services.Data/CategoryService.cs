using HireHub.Data;
using HireHub.Web.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Web.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CategoryService:ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
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
