using System.ComponentModel.DataAnnotations;

using static HireHub.Common.EntityValidation.Country;

namespace HireHub.Data.Entities
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [StringLength(CountryNameMaxLength, MinimumLength = CountryNameMinLength, ErrorMessage = "Country name must be between {2} and {1} length")]
        public string CountryName { get; set; } = null!;

        public ICollection<Town> Towns { get; set; } = new List<Town>();
    }
}