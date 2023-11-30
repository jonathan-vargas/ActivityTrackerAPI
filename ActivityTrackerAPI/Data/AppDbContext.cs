using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ActivityTrackerAPI.Model;

namespace ActivityTrackerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ActivityTrackerAPI.Model.Activity> Activity { get; set; } = default!;

        public DbSet<ActivityTrackerAPI.Model.Employee>? Employee { get; set; }

        public DbSet<ActivityTrackerAPI.Model.Error>? Error { get; set; }
    }
}
