using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace messanger
{
    class Context : DbContext
    {
        public Context()
        {
            Database.EnsureCreated();
        }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-468M5GV;Database=MessangerDb;Trusted_Connection=true;");
        }
    }
}
