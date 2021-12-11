using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Entities;

namespace RecipeManager.DAL.DataContext
{
   public class DbReportsContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }

        public DbReportsContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Reports.db");

        }
    }
}
