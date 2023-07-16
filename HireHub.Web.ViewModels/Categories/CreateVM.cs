using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static HireHub.Common.EntityValidation.Category;
namespace HireHub.Web.ViewModels.Categories
{
    public class CreateVM
    {
        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = "Category name must be between {2} and {1} length")]
        public string CategoryName { get; set; } = null!;
    }
}
