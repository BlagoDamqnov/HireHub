using System.ComponentModel.DataAnnotations;

using static HireHub.Common.EntityValidation.Category;

namespace HireHub.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength, ErrorMessage = "Category name must be between {2} and {1} length")]
        public string CategoryName { get; set; } = null!;

        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}