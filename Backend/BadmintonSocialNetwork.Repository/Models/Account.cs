using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("UserName", TypeName = "varchar(32)")]
        public required string UserName { get; set; }

        [Column("Avatar", TypeName = "varchar(150)")]
        public required string Avatar { get; set; }

        [Column("CoverPhoto", TypeName = "varchar(150)")]
        public required string CoverPhoto { get; set; }

        [Column("Password", TypeName = "varchar(256)")]
        public required string PasswordHash { get; set; }

        [Column("FullName", TypeName = "text")]
        public required string FullName { get; set; }

        [Column("Email", TypeName = "varchar(32)")]
        public required string Email { get; set; }

        [Column("IsConfirmedEmail")]
        public bool IsConfirmedEmail { get; set; }

        [Column("PhoneNumber", TypeName = "varchar(12)")]
        public string PhoneNumber { get; set; }

        [Column("Address", TypeName = "text")]
        public string Address { get; set; }

        [Column("IsConfirmedPhoneNumber")]
        public bool IsConfirmedPhoneNumber { get; set; }
        [Column("DateOfBirth")]
        public DateOnly DateOfBirth { get; set; }

        [Column("Gender", TypeName = "varchar(6)")]
        public string Gender { get; set; }

        [Column("EmailOtp")]
        public int EmailOtp { get; set; }

        [Column("PhoneNumberOtp")]
        public int PhoneNumberOtp { get; set; }
        public ICollection<AccountRole> AccountRoles { get; set; }
        public ICollection<ClubMember> ClubMemberships { get; set; }

        // Friendships
        public ICollection<Friend> FriendsRequested { get; set; }
        public ICollection<Friend> FriendsReceived { get; set; }

    }
}
