using BadmintonSocialNetwork.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Service.DTOs
{
    public class LikeDTO
    {
        public int AccountId { get; set; }
        public int PostId { get; set; }
    }
    public class LikeVM : LikeDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string AccountName { get; set; }
    }

    public class LikeCM : LikeDTO
    {

    }
}
