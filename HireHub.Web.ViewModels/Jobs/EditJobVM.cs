namespace HireHub.Web.ViewModels.Jobs
{
    using HireHub.Web.ViewModels.Categories;
    using HireHub.Web.ViewModels.Countries;
    using HireHub.Web.ViewModels.Towns;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static HireHub.Common.EntityValidation.Job;

    // ReSharper disable once InconsistentNaming
    public class EditJobVM
    {
        public ICollection<CategoryVM> Categories { get; set; } = new List<CategoryVM>();
        public int CategoryId { get; set; }
        public ICollection<CountryVM> Countries { get; set; } = new List<CountryVM>();
        public int CountryId { get; set; }
        public string? CountryName { get; set; }

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "Description must be between {2} and {1} length")]
        public string Description { get; set; } = null!;

        public Guid Id { get; set; }

        public decimal? MaxSalary { get; set; }

        public decimal MinSalary { get; set; }

        [StringLength(RequirementsMaxLength, MinimumLength = RequirementsMinLength, ErrorMessage = "Requirements must be between {2} and {1} length")]
        public string Requirements { get; set; } = null!;

        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Title must be between {2} and {1} length")]
        public string Title { get; set; } = null!;

        public int TownId { get; set; }
        public string? TownName { get; set; }
        public ICollection<TownVM> Towns { get; set; } = new List<TownVM>();
    }
}