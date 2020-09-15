using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API.Models;

namespace Web_API.Data
{
    public class ApiContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }

        public DbSet<User> users { get; set; }
        public DbSet<Board> leagues { get; set; }
        public DbSet<UserBoard> userBoard { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //setup FK relationship
            modelBuilder.Entity<UserBoard>()
                .HasOne(ul => ul.board)
                .WithMany(l => l.users)
                .HasForeignKey(ul => ul.boardId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserBoard>()
                .HasOne(ul => ul.user)
                .WithMany(u => u.leagues)
                .HasForeignKey(ul => ul.userId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }

        public DbSet<Web_API.Models.Job> Job { get; set; }
    }
}
