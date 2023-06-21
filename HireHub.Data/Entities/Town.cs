using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HireHub.Data.Entities
{
    public class Town
    {
        public int Id { get; set; }
        public string TownName { get; set; } = null!;
        public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }

}