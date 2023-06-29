using HireHub.Web.ViewModels.Jobs;

namespace HireHub.Web.ViewModels.Company
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateCompanyVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public string? ContactPhone { get; set; }
    }
}
