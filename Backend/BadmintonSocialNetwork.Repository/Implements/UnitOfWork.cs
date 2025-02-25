using BadmintonSocialNetwork.Repository.Data;
using BadmintonSocialNetwork.Repository.Interfaces;
using BadmintonSocialNetwork.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private BadmintonSocialNetworkDBContext _dbContext;

        public UnitOfWork(BadmintonSocialNetworkDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IGenericRepository<Account> _accountRepository;
        private IGenericRepository<AccountRole> _accountRoleRepository;
        private IGenericRepository<Role> _roleRepository;

        public IGenericRepository<Account> AccountRepository
        {
            get
            {

                if (_accountRepository == null)
                {
                    _accountRepository = new GenericRepository<Account>(_dbContext);
                }
                return _accountRepository;
            }
        }

        public IGenericRepository<AccountRole> AccountRoleRepository
        {
            get
            {

                if (_accountRoleRepository == null)
                {
                    _accountRoleRepository = new GenericRepository<AccountRole>(_dbContext);
                }
                return _accountRoleRepository;
            }
        }

        public IGenericRepository<Role> RoleRepository
        {
            get
            {

                if (_roleRepository == null)
                {
                    _roleRepository = new GenericRepository<Role>(_dbContext);
                }
                return _roleRepository;
            }
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
