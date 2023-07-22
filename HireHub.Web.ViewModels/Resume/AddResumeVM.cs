using System.ComponentModel.DataAnnotations;

using static HireHub.Common.EntityValidation.Resume;

namespace HireHub.Web.ViewModels.Resume
{
    public class AddResumeVM
    {
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Name must be between {2} and {1} length")]
        public string Name { get; set; } = null!;

        [StringLength(ResumePathMaxLength, MinimumLength = ResumePathMinLength, ErrorMessage = "Resume path must be between {2} and {1} length")]
        public string ResumePath { get; set; } = null!;
    }
}