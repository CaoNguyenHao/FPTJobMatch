using System;
using FPTJobMatch.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FPTJobMatch.Models
{
    public class CandidateContext : ApplicationDbContext
    {
        public CandidateContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Candidate> Candidates { get; set; }
    }

    public class Candidate
    {
        public int Id { get; set; }
        public string JobPosition { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkExperiences { get; set; }
        public CandidateStatus Status { get; set; }
        public string? ImageUrl { get; set; }

    }
}

