using Microsoft.AspNetCore.Http;
using MimeKit.IO.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.DTOs
{
    public class CommentDTO
    {
        public string Content { get; set; }
        public Guid? ParentCommentID { get; set; }
    }

    public class CommentVM : CommentDTO
    {
        public Guid Id { get; set; }
        public string ImageLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public Guid PostId { get; set; }
    }

    public class CommentCM : CommentDTO
    {
        public Guid PostId { get; set; }
        public FormFile ImageFile{ get; set; }
    }

    public class CommentUM : CommentDTO
    {
        public FormFile ImageFile { get; set; }
    }
}
