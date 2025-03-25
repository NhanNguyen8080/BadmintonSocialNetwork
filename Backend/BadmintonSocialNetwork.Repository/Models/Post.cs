using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadmintonSocialNetwork.Repository.Enums;

namespace BadmintonSocialNetwork.Repository.Models
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string? ImageFile { get; set; }
        public string AppearedPlace { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }
        public PostVisibility Visibility { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
