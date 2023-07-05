﻿namespace HireHub.Web.ViewModels.Jobs
{
    using HireHub.Web.ViewModels.Categories;
    using HireHub.Web.ViewModels.Countries;
    using HireHub.Web.ViewModels.Towns;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using static HireHub.Common.EntityValidation.Job;
    // ReSharper disable once InconsistentNaming
    public class EditJobVM
    {
        public Guid Id { get; set; }
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Title must be between {2} and {1} length")]
        public string Title { get; set; } = null!;

        [StringLength(LogoUrlMaxLength, MinimumLength = LogoUrlMinLength, ErrorMessage = "Logo must be between {2} and {1} range")]
        public string Logo { get; set; } = null!;

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "Description must be between {2} and {1} length")]
        public string Description { get; set; } = null!;

        [StringLength(RequirementsMaxLength, MinimumLength = RequirementsMinLength, ErrorMessage = "Requirements must be between {2} and {1} length")]
        public string Requirements { get; set; } = null!;
        public string? TownName { get; set; }
        public string? CountryName { get; set; }
        public int CategoryId { get; set; }
        public ICollection<CategoryVM> Categories { get; set; } = new List<CategoryVM>();
        public int CountryId { get; set; }
        public ICollection<CountryVM> Countries { get; set; } = new List<CountryVM>();
        public int TownId { get; set; }
        public ICollection<TownVM> Towns { get; set; } = new List<TownVM>();
        public decimal MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }
    }
}
