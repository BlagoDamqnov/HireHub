using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static HireHub.Common.EntityValidation.Town;

namespace HireHub.Data.Entities
{
    public class Town
    {
        public int Id { get; set; }

        [StringLength(TownNameMaxLength, MinimumLength = TownNameMinLength,ErrorMessage = "Town name must be between {2} and {1} characters long.")]
        public string TownName { get; set; } = null!;
        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }

}