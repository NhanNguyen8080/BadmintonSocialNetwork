﻿// <auto-generated />
using System;
using BadmintonSocialNetwork.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BadmintonSocialNetwork.API.Migrations
{
    [DbContext(typeof(BadmintonSocialNetworkDBContext))]
    partial class BadmintonSocialNetworkDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Address");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("Avatar");

                    b.Property<string>("CoverPhoto")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("CoverPhoto");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("DateOfBirth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasColumnName("Email");

                    b.Property<int>("EmailOtp")
                        .HasColumnType("integer")
                        .HasColumnName("EmailOtp");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("FullName");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("varchar(6)")
                        .HasColumnName("Gender");

                    b.Property<bool>("IsConfirmedEmail")
                        .HasColumnType("boolean")
                        .HasColumnName("IsConfirmedEmail");

                    b.Property<bool>("IsConfirmedPhoneNumber")
                        .HasColumnType("boolean")
                        .HasColumnName("IsConfirmedPhoneNumber");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(256)")
                        .HasColumnName("Password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("varchar(12)")
                        .HasColumnName("PhoneNumber");

                    b.Property<int>("PhoneNumberOtp")
                        .HasColumnType("integer")
                        .HasColumnName("PhoneNumberOtp");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.AccountRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountRoles");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Bookmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("PostId");

                    b.ToTable("Bookmarks");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Club", b =>
                {
                    b.Property<Guid>("ClubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClubId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.ClubMember", b =>
                {
                    b.Property<Guid>("ClubId")
                        .HasColumnType("uuid");

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("ClubId", "AccountId");

                    b.HasIndex("AccountId");

                    b.ToTable("ClubMembers");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageLink")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentCommentID")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ParentCommentID");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Friend", b =>
                {
                    b.Property<int>("RequesterId")
                        .HasColumnType("integer");

                    b.Property<int>("AddresseeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("RequesterId", "AddresseeId");

                    b.HasIndex("AddresseeId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("PostId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<string>("AppearedPlace")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageFile")
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Visibility")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.AccountRole", b =>
                {
                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Account", "Account")
                        .WithMany("AccountRoles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Role", "Role")
                        .WithMany("AccountRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Bookmark", b =>
                {
                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.ClubMember", b =>
                {
                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Account", "Account")
                        .WithMany("ClubMemberships")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Club", "Club")
                        .WithMany("Members")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Club");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Comment", b =>
                {
                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Comment", "ParentComment")
                        .WithMany()
                        .HasForeignKey("ParentCommentID");

                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("ParentComment");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Friend", b =>
                {
                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Account", "Addressee")
                        .WithMany("FriendsReceived")
                        .HasForeignKey("AddresseeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Account", "Requester")
                        .WithMany("FriendsRequested")
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Addressee");

                    b.Navigation("Requester");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Like", b =>
                {
                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Post", b =>
                {
                    b.HasOne("BadmintonSocialNetwork.Repository.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Account", b =>
                {
                    b.Navigation("AccountRoles");

                    b.Navigation("ClubMemberships");

                    b.Navigation("FriendsReceived");

                    b.Navigation("FriendsRequested");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Club", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("BadmintonSocialNetwork.Repository.Models.Role", b =>
                {
                    b.Navigation("AccountRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
