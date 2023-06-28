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
            public const int LogoUrlMinLength = 10;
            public const int LogoUrlMaxLength = 200;
        }
    }
}
