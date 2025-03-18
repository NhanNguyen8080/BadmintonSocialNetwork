using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.DTOs
{
    public class PostDTO
    {
        public string Content { get; set; }
        public string AppearedPlace { get; set; }
    }

    public class PostVM : PostDTO
    {
        public Guid Id { get; set; }
        public string? ImageLink { get; set; }
        public bool Status { get; set; }
        public int AccountId { get; set; }
        public string AcccountName { get; set; }
        public string AccountAvtImage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class PostCM : PostDTO
    {
        public FormFile? ImageFile { get; set; }
    }

    public class PostUM : PostDTO
    {
        public FormFile? ImageFile { get; set; }
    }
}
