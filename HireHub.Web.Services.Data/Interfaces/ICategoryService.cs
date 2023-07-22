namespace HireHub.Web.Services.Data.Interfaces
{
    using HireHub.Web.ViewModels.Categories;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<IEnumerable<string>> GetAllCategoryNames();

        Task<bool> IsExist(int id);

        Task Create(CreateVM model);
    }
}