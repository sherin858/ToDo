using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ToDoContext : IdentityDbContext<User>
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
        : base(options)
        {

        }

        public DbSet<ToDoItem> TodoItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ToDoItem>().HasKey(a => a.Id);
            builder.Entity<ToDoItem>(e =>
            {
                e.Property(e => e.Title).HasMaxLength(100).IsRequired();
                e.Property(e => e.Description);
            });
        }
    }
}
