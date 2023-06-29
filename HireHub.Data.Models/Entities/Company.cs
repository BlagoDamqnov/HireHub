using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HireHub.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HireHub.Data.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; }  = null!;
        public string? ContactPhone { get; set; }
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
        public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
        
    }
}
