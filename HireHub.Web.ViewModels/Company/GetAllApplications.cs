using HireHub.Web.ViewModels.Users;

namespace HireHub.Web.ViewModels.Company
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GetAllApplications
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string LogoUrl { get; set; } = null!;
        public ICollection<GetInfo> Applicants { get; set; } = null!;
    }
}
