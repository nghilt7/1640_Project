using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace _1640_Project.Models
{
    public class IdeasDbContext : DbContext
    {
        public IdeasDbContext() : base("IdeasDbContext")
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}