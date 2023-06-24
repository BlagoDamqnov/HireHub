using HireHub.Data.Entities;
using HireHub.Web.ViewModels.Categories;
using HireHub.Web.ViewModels.Countries;
using HireHub.Web.ViewModels.Towns;

namespace HireHub.Web.ViewModels.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateJobVM
    { 
        public string Title { get; set; } = null!;
        public string Logo { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Requirements { get; set; } = null!;
        public int CategoryId { get; set; }
        public ICollection<CategoryVM> Categories { get; set; } = new List<CategoryVM>();
        public int CountryId { get; set; }
        public ICollection<CountryVM> Countries { get; set; } = new List<CountryVM>();
        public int TownId { get; set; }
        public ICollection<TownVM> Towns{ get; set; } = new List<TownVM>();
        public decimal MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; } = false; // For change
        public bool IsApproved { get; set; } = false; // For change
    }
}
