﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate.Domain.Entities
{
    public class CandidateProfile
    {
        [Key]
        public int CandidateId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        [MaxLength(50)]
        public string BestTimeToCall { get; set; }
        [MaxLength(500)]
        public string LinkedInProfileUrl { get; set; }
        [MaxLength(500)]
        public string GitHubProfileUrl { get; set; }
        [Required]
        [MaxLength(500)]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
