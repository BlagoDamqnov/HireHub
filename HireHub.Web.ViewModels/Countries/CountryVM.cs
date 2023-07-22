using HireHub.Data.Entities;

namespace HireHub.Web.ViewModels.Countries
{
    using System.Collections.Generic;

    public class CountryVM
    {
        public int CountryId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Town> Towns { get; set; } = new List<Town>();
    }
}