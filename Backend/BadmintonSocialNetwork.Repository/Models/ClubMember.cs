using BadmintonSocialNetwork.Repository.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Models
{
    [Table("ClubMembers")]
    public class ClubMember
    {
        // Composite Key: ClubId + AccountId
        public Guid ClubId { get; set; }
        public int AccountId { get; set; }
        public ClubRole Role { get; set; } = ClubRole.Member; // "Member", "Admin", "Owner"
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Club Club { get; set; }
        public Account Account { get; set; }
    }

}
