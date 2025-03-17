using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Models
{
    [Table("Bookmarks")]
    public class Bookmark
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
