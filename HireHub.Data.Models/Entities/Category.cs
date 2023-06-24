using System.ComponentModel.DataAnnotations;

namespace HireHub.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}