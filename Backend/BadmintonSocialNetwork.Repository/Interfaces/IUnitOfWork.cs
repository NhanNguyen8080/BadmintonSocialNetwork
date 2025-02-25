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

        Task SaveAsync();
    }
}
