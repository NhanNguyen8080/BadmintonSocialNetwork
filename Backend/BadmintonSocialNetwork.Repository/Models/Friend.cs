using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Models
{
    [Table("Friends")]
    public class Friend
    {
        // Composite Key: RequesterId + AddresseeId
        public int RequesterId { get; set; }

        public int AddresseeId { get; set; }

        public string Status { get; set; } = "Pending"; // "Pending", "Accepted", "Rejected"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Account Requester { get; set; }
        public Account Addressee { get; set; }
    }

}
