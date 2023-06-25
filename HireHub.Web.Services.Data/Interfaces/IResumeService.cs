using HireHub.Web.ViewModels.Resume;

namespace HireHub.Web.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IResumeService
    {
        Task AddResumeAsync(AddResumeVM model, string userId);

    }
}
