namespace HireHub.Web.Services.Data.Interfaces
{
    using HireHub.Web.ViewModels.Categories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<IEnumerable<string>> GetAllCategoryNames();
        Task<bool> IsExist(int id);
        Task Create(CreateVM model);
    }
}
