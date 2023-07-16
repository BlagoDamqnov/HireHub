namespace HireHub.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class EntityValidation
    {
        public static class Job
        {
            public const int TitleMinLength = 5;
            public const int TitleMaxLength = 50;
            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 1000;
            public const int RequirementsMinLength = 10;
            public const int RequirementsMaxLength = 1000;
            public const int MinSalaryMinValue = 0;
            public const int MinSalaryMaxValue = 1000000;
            public const int MaxSalaryMinValue = 0;
            public const int MaxSalaryMaxValue = 1000000;
        }
        public static class Resume
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 50;
            public const int ResumePathMinLength = 10;
            public const int ResumePathMaxLength = 2000;
        }

        public static class Town
        {
            public const int TownNameMinLength = 5;
            public const int TownNameMaxLength = 50;
        }
        public static class Country
        {
            public const int CountryNameMinLength = 5;
            public const int CountryNameMaxLength = 50;
        }

        public static class Company
        {
            public const int CompanyNameMinLength = 5;
            public const int CompanyNameMaxLength = 50;
            public const int ContactEmailMinLength = 5;
            public const int ContactEmailMaxLength = 50;
            public const int ContactPhoneMinLength = 5;
            public const int ContactPhoneMaxLength = 50;
            public const int LogoUrlMinLength = 10;
            public const int LogoUrlMaxLength = 2000;
        }

        public static class Category
        {
            public const int CategoryNameMinLength = 5;
            public const int CategoryNameMaxLength = 50;
        }
    }
}
