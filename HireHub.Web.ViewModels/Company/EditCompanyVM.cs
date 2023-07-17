using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static HireHub.Common.EntityValidation.Company;
namespace HireHub.Web.ViewModels.Company
{
    public class EditCompanyVM
    {
        public int Id { get; set; }

        [StringLength(CompanyNameMaxLength, MinimumLength = CompanyNameMinLength, ErrorMessage = "Company name must be between {2} and {1} length")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Company name must contain only letters, digits and whitespaces")]
        public string Name { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        [StringLength(ContactEmailMaxLength, MinimumLength = ContactEmailMinLength, ErrorMessage = "Contact email must be between {2} and {1} length")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Contact email is not valid")]
        public string ContactEmail { get; set; } = null!;

        [StringLength(ContactPhoneMaxLength, MinimumLength = ContactPhoneMinLength, ErrorMessage = "Contact phone must be between {2} and {1} length")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Contact phone is not valid")]
        public string? ContactPhone { get; set; }
    }
}
