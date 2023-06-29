using HireHub.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace HireHub.Data.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationUser:IdentityUser
    {
        public ICollection<Application> Applications { get; set; } = new HashSet<Application>();
    }
}
