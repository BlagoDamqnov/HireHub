using HireHub.Web.ViewModels.Application;

namespace HireHub.Web.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IApplicationService
    {
        Task<ApplyForJobVM> AddApplicationAsync( string userId);
        Task AddApply(ApplyForJobVM model, string jobId, string userId);
    }
}
