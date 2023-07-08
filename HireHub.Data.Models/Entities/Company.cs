using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HireHub.Data.Entities;

using static HireHub.Common.EntityValidation.Company;

namespace HireHub.Data.Models.Entities
{
    using System.Collections.Generic;

    public class Company
    {
        [Key]
        public int Id { get; set; }

        [StringLength(CompanyNameMaxLength, MinimumLength = CompanyNameMinLength,ErrorMessage = "Company name must be between {2} and {1} length")]
        public string Name { get; set; } = null!;

        [StringLength(ContactEmailMaxLength, MinimumLength = ContactEmailMinLength, ErrorMessage = "Contact email must be between {2} and {1} length")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Contact email is not valid")]
        public string ContactEmail { get; set; }  = null!;

        [StringLength(ContactPhoneMaxLength, MinimumLength = ContactPhoneMinLength, ErrorMessage = "Contact phone must be between {2} and {1} length")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Contact phone is not valid")]
        public string? ContactPhone { get; set; }
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
        public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
        
    }
}
