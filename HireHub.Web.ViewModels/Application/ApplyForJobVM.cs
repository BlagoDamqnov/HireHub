using HireHub.Web.ViewModels.Resume;

namespace HireHub.Web.ViewModels.Application
{
    using System;
    using System.Collections.Generic;

    public class ApplyForJobVM
    {
        public int ResumeId { get; set; }
        public IEnumerable<GetResumeVM> Resumes { get; set; } = new List<GetResumeVM>();
        public DateTime CreatedOn { get; set; }
    }
}