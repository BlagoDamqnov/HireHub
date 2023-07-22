using HireHub.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HireHub.Data.Models.Entities
{
    using System.Collections.Generic;

    public class ApplicationUser : IdentityUser
    {
        public ICollection<Application> Applications { get; set; } = new HashSet<Application>();
    }
}