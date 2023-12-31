﻿using HireHub.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireHub.Data.Models.Entities
{
    public class HiringRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; } = null!;

        public string CandidateId { get; set; } = null!;

        [ForeignKey(nameof(CandidateId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public DateTime DateOfHiring { get; set; }
        public bool IsHired { get; set; }
    }
}