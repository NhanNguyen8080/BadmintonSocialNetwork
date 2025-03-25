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
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<ClubMember> ClubMembers { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Bookmark> Bookmarks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClubMember>()
                .HasKey(cm => new { cm.ClubId, cm.AccountId });

            modelBuilder.Entity<ClubMember>()
                .HasOne(cm => cm.Club)
                .WithMany(c => c.Members)
                .HasForeignKey(cm => cm.ClubId);

            modelBuilder.Entity<ClubMember>()
                .HasOne(cm => cm.Account)
                .WithMany(a => a.ClubMemberships)
                .HasForeignKey(cm => cm.AccountId);

            // Friend table composite key
            modelBuilder.Entity<Friend>()
                .HasKey(f => new { f.RequesterId, f.AddresseeId });

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.Requester)
                .WithMany(a => a.FriendsRequested)
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.Addressee)
                .WithMany(a => a.FriendsReceived)
                .HasForeignKey(f => f.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
