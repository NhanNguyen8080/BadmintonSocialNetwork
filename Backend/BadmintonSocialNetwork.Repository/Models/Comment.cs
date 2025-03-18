using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string ImageLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid? ParentCommentID { get; set; }
        public Comment ParentComment { get; set; }
    }
}
