using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Entities;

namespace RecipeManager.DAL.DataContext
{
    public class DbDoseContext : DbContext
    {
        public DbSet<Dose> Doses { get; set; }

        public DbDoseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("Filename=Doses.db");

        }
    }
}
