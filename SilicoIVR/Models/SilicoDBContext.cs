using Microsoft.EntityFrameworkCore;
using SilicoIVR.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilicoIVR.Models
{
    public class SilicoDBContext : DbContext
    {
        public SilicoDBContext(DbContextOptions<SilicoDBContext> options) 
            : base (options)
        { }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<IvrOption> IvrOptions { get; set; }
        public DbSet<Call> Calls { get; set; }
        public DbSet<Recording> Recordings { get; set; }
    }
}
