﻿namespace HireHub.Web.ViewModels.Resume
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetResumeVM
    {
        public int Id { get; set; }
        public string CreatorId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ResumePath { get; set; } = null!;
    }
}
