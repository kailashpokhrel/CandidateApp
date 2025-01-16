using Candidate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candidate.Infrastructure.Persistence
{
    public class ApplicationsDbContext: DbContext
    {
        public ApplicationsDbContext(DbContextOptions<ApplicationsDbContext> options) : base(options) { }   
        public DbSet<CandidateProfile> CandidateProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CandidateProfile>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<CandidateProfile>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<CandidateProfile>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<CandidateProfile>()
                .HasIndex(c => c.Email)
                .IsUnique();  

            modelBuilder.Entity<CandidateProfile>()
                .Property(c => c.Comment)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<CandidateProfile>()
               .Property(c => c.BestTimeToCall)
               .HasMaxLength(50);

            modelBuilder.Entity<CandidateProfile>()
               .Property(c => c.GitHubProfileUrl)
               .HasMaxLength(500);

            modelBuilder.Entity<CandidateProfile>()
               .Property(c => c.LinkedInProfileUrl)
               .HasMaxLength(500);

            modelBuilder.Entity<CandidateProfile>()
               .Property(c => c.PhoneNumber)
               .HasMaxLength(15);

        }
    }
    
}
