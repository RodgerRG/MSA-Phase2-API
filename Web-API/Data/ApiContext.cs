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

        DbSet<User> users { get; set; }
        DbSet<League> leagues { get; set; }
        DbSet<UserLeague> userLeague { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //setup keys and principal keys
            modelBuilder.Entity<UserLeague>().HasKey(ul => new { ul.userId, ul.leagueId });
            modelBuilder.Entity<UserLeague>()
                .HasOne(ul => ul.league)
                .WithMany(l => l.users)
                .HasForeignKey(ul => ul.leagueId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<UserLeague>()
                .HasOne(ul => ul.user)
                .WithMany(u => u.leagues)
                .HasForeignKey(ul => ul.userId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
