using BadmintonSocialNetwork.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.DTOs
{
    public class BookmarkDTO
    {
        public int AccountId { get; set; }
        public int PostId { get; set; }
    }

    public class BookmarkVM : BookmarkDTO
    {
        public string AccountName { get; set; }
        public string PostTitle { get; set; }
        public string PostImage { get; set; }
        public int AuthorId { get; set; }
        //public int ClubId { get; set; }

    }
}
