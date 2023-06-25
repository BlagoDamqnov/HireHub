using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HireHub.Data.Entities
{
    public class Resume
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ResumePath { get; set; } = null!;
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public IdentityUser IdentityUser { get; set; } = null!;

        public ICollection<Application> Applications { get; set; } = new List<Application>();

    }
}