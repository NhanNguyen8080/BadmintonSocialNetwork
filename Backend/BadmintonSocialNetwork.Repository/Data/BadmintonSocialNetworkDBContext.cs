using BadmintonSocialNetwork.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Data
{
    public class BadmintonSocialNetworkDBContext : DbContext
    {
        public BadmintonSocialNetworkDBContext()
        {
        }

        public BadmintonSocialNetworkDBContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<AccountRole> AccountRoles { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Bookmark> Bookmarks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
