using System;
using FPTJobMatch.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FPTJobMatch.Models
{
    public class JobContext : ApplicationDbContext
    {
        public JobContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Job> Jobs { get; set; }
    }

    public class Job
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public JobStatus Status { get; set; }
    }
}

