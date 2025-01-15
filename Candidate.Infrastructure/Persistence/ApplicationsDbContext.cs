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
    }
}
