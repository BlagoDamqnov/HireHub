using System.ComponentModel.DataAnnotations;

namespace HireHub.Data.Entities
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; } = null!;
        public ICollection<Town> Towns { get; set; } = new List<Town>();
    }
}