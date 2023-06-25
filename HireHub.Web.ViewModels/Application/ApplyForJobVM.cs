using HireHub.Web.ViewModels.Resume;

namespace HireHub.Web.ViewModels.Application
{
    using HireHub.Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplyForJobVM
    { 
        public int ResumeId { get; set; }
        public IEnumerable<GetResumeVM> Resumes { get; set; } = new List<GetResumeVM>();
        public DateTime CreatedOn { get; set; }
    }
}
