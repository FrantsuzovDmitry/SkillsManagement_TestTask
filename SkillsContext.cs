using Microsoft.EntityFrameworkCore;
using SkillsManagement.Models;

namespace SkillsManagement
{
    public class SkillsContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=skillsmanagementdb;Initial Catalog=TestTask");
        }
    }
}