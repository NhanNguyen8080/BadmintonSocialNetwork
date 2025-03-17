using BadmintonSocialNetwork.Repository.Implements;
using BadmintonSocialNetwork.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Account> AccountRepository { get; }
        IGenericRepository<AccountRole> AccountRoleRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<Post> PostRepository { get; }
        IGenericRepository<Like> LikeRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        IGenericRepository<Bookmark> BookmarkRepository { get; }

        Task SaveAsync();
    }
}
