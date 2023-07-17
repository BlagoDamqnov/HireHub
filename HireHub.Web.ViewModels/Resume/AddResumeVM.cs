using System.ComponentModel.DataAnnotations;

using static HireHub.Common.EntityValidation.Resume;

namespace HireHub.Web.ViewModels.Resume
{

    public class AddResumeVM
    {
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "Name must be between {2} and {1} length")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name must contain only letters, digits and whitespaces")]
        public string Name { get; set; } = null!;

        [StringLength(ResumePathMaxLength, MinimumLength = ResumePathMinLength, ErrorMessage = "Resume path must be between {2} and {1} length")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Resume path must contain only letters, digits and whitespaces")]
        public string ResumePath { get; set; } = null!;
    }
}
