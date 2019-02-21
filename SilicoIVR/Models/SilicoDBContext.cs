using Microsoft.EntityFrameworkCore;
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
    }
}
