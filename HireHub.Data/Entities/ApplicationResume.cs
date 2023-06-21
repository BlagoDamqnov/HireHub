using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HireHub.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationResume
    {
        public Guid ApplicationId { get; set; } 
        [ForeignKey(nameof(ApplicationId))]
        public Application Application { get; set; } = null!;
        public int ResumeId { get; set; }

        [ForeignKey(nameof(ResumeId))]
        public Resume Resume { get; set; } = null!;
    }
}
