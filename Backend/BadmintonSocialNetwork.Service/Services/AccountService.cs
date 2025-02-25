using AutoMapper;
using BadmintonSocialNetwork.Repository.Interfaces;
using BadmintonSocialNetwork.Repository.Models;
using BadmintonSocialNetwork.Service.DTOs;
using BadmintonSocialNetwork.Service.Enums;
using BadmintonSocialNetwork.Service.Ultities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BadmintonSocialNetwork.Service.Services
{
    public interface IAccountService
    {
        Task<ResponseDTO<AccountVM>> SignUp(AccountCM accountCM);
        Task<ResponseDTO<IEnumerable<AccountVM>>> GetAccounts(int currentPage, int pageSize);
        Task<ResponseDTO<AccountVM>> GetAccountById(int accountId);
        Task<ResponseDTO<AccountVM>> UpdateAccount(int accountId, AccountUM accountUM);
        Task<ResponseDTO<AccountVM>> DeleteAccount(int accountId);
        Task<ResponseDTO<Account>> GetAccountByUserNameOrEmail(string usernameOrEmail);
        Task<IList<string>> GetUserRoles(int id);
        Task<ResponseDTO<AccountVM>> ConfirmEmail(int accountId);
        Task<ResponseDTO<AccountVM>> ConfirmEmailOtp(int accountId, int otp);
        Task<int> GetCurrentAccountFromToken();
        Task<ResponseDTO<AccountVM>> ChangeEmail(int accountId, string newEmail);
        Task<bool> ChangePassword(int accountId, string password);
    }
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        public AccountService(IUnitOfWork unitOfWork, 
                              IMapper mapper,
                              IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mailService = mailService;
        }

        public async Task<ResponseDTO<AccountVM>> DeleteAccount(int accountId)
        {
            var responseDTO = new ResponseDTO<AccountVM>();
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByID(accountId);
                if (account is null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Cannot find any account with id = {accountId}!";
                    responseDTO.Data = null;
                    return responseDTO;
                }
                await _unitOfWork.AccountRepository.DeleteAsync(accountId);
                await _unitOfWork.SaveAsync();
                responseDTO.IsSuccess = true;
                responseDTO.Message = "Delete account successfully!";
                responseDTO.Data = _mapper.Map<Account, AccountVM>(account);
                return responseDTO;
            } catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO<AccountVM>> GetAccountById(int accountId)
        {

            var responseDTO = new ResponseDTO<AccountVM>();
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByID(accountId);
                if (account is null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Cannot find any account with id = {accountId}!";
                    responseDTO.Data = null; 
                    return responseDTO;
                }
                var result = _mapper.Map<Account, AccountVM>(account);
                responseDTO.IsSuccess = true;
                responseDTO.Message = $"Found an account with id = {accountId}";
                responseDTO.Data = result;
                return responseDTO;
            } catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO<IEnumerable<AccountVM>>> GetAccounts(int currentPage, int pageSize)
        {
            var responseDTO = new ResponseDTO<IEnumerable<AccountVM>>();
            try
            {
                var accounts = await _unitOfWork.AccountRepository
                                            .Get(_ => _.Id > 0, _ => _.OrderByDescending(ord => ord.Id),
                                            "", currentPage, pageSize);
                var result = _mapper.Map<IEnumerable<Account>, IEnumerable<AccountVM>>(accounts);
                responseDTO.IsSuccess = true;
                responseDTO.Message = "Query successfully!";
                responseDTO.Data = result;
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO<AccountVM>> SignUp(AccountCM accountCM)
        {
            var responseDTO = new ResponseDTO<AccountVM>();
            try
            {
                var existedAccount = (await _unitOfWork.AccountRepository
                                                        .Get(_ => _.UserName == accountCM.UserName || 
                                                        _.Email == accountCM.Email))
                                                        .FirstOrDefault();
                if (existedAccount is not null)
                {
                    var existedAccountVM = _mapper.Map<Account, AccountVM>(existedAccount);
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "This account is already existed!";
                    responseDTO.Data = existedAccountVM;
                    return responseDTO;
                }

                //insert a new account
                var addedAccount = _mapper.Map<AccountCM, Account>(accountCM);
                addedAccount.IsConfirmedEmail = false;
                addedAccount.IsConfirmedPhoneNumber = false;
                addedAccount.PasswordHash = PasswordHasher.HashPassword(accountCM.Password);
                if (addedAccount.Gender.Equals("Male"))
                {
                    addedAccount.Avatar = "https://localhost:7018/male";
                } else
                {
                    addedAccount.Avatar = "https://localhost:7018/female";
                }
                await _unitOfWork.AccountRepository.InsertAsync(addedAccount);
                await _unitOfWork.SaveAsync();

                //insert a new account role
                var addedAccountRole = new AccountRole
                {
                    AccountId = addedAccount.Id,
                    Account = addedAccount,
                    RoleId = (int)RoleEnums.User,
                    Role = await _unitOfWork.RoleRepository.GetByID((int)RoleEnums.User)
                };
                await _unitOfWork.AccountRoleRepository.InsertAsync(addedAccountRole);
                await _unitOfWork.SaveAsync();

                //send email to confirm
                var mailContent = new MailContent
                {
                    To = addedAccount.Email,
                    Subject = "Confirm your email",
                    Body = $"<h1>Welcome to Badminton Social Network</h1><p>Please click " +
                    $"<a href='https://localhost:7018/api/Account/confirm-email/{addedAccount.Id}'>here</a> to confirm your email</p>"
                };
                var isSendSuccess = await _mailService.SendMailAsync(mailContent);
                if (!isSendSuccess)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = "Cannot send email to confirm!";
                    responseDTO.Data = null;
                    return responseDTO;
                }

                //return the added account
                var addedAccountVM = _mapper.Map<Account, AccountVM>(addedAccount);
                addedAccountVM.RoleName = addedAccountRole.Role.RoleName;
                responseDTO.IsSuccess = true;
                responseDTO.Message = "Please check your email to confirm!";
                responseDTO.Data = addedAccountVM;
                return responseDTO;
            } catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO<AccountVM>> UpdateAccount(int accountId, AccountUM accountUM)
        {
            var responseDTO = new ResponseDTO<AccountVM>();
            try
            {
                var updatedAccount = await _unitOfWork.AccountRepository.GetByID(accountId);
                if (updatedAccount is null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Cannot find any account with id = {accountId}!";
                    responseDTO.Data = null;
                    return responseDTO;
                }
                updatedAccount.UserName = accountUM.UserName;
                updatedAccount.FullName = accountUM.FullName;
                updatedAccount.Email = accountUM.Email;
                updatedAccount.PhoneNumber = accountUM.PhoneNumber;
                updatedAccount.DateOfBirth = accountUM.DateOfBirth;
                updatedAccount.Gender = accountUM.Gender;
                _unitOfWork.AccountRepository.Update(updatedAccount);
                await _unitOfWork.SaveAsync();

                responseDTO.IsSuccess = true;
                responseDTO.Message = $"Update account successfully";
                responseDTO.Data = _mapper.Map<AccountVM>(updatedAccount);
                return responseDTO;
            } catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO<Account>> GetAccountByUserNameOrEmail(string usernameOrEmail)
        {
            var responseDTO = new ResponseDTO<Account>();
            try
            {
                var account = (await _unitOfWork.AccountRepository.Get(_ => _.UserName == usernameOrEmail 
                                                                           || _.Email == usernameOrEmail))
                                                                  .FirstOrDefault();
                if (account is null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Cannot find any account with username or email = {usernameOrEmail}!";
                    responseDTO.Data = null;
                    return responseDTO;
                }
                responseDTO.IsSuccess = true;
                responseDTO.Message = $"Found an account with username or email = {usernameOrEmail}";
                responseDTO.Data = account;
                return responseDTO;
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<IList<string>> GetUserRoles(int id)
        {
            try
            {
                var roles = (await _unitOfWork.AccountRoleRepository.Get(_ => _.AccountId == id, null, "Role"))
                                                                            .Select(ac => ac.Role.RoleName)
                                                                            .ToList();

                return roles;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public async Task<ResponseDTO<AccountVM>> ConfirmEmail(int accountId)
        {
            var responseDTO = new ResponseDTO<AccountVM>();
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByID(accountId);
                if (account is null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Cannot find any account with id = {accountId}!";
                    responseDTO.Data = null;
                    return responseDTO;
                }
                account.IsConfirmedEmail = true;
                _unitOfWork.AccountRepository.Update(account);
                await _unitOfWork.SaveAsync();
                responseDTO.IsSuccess = true;
                responseDTO.Message = "Confirm email successfully!";
                responseDTO.Data = _mapper.Map<Account, AccountVM>(account);
                return responseDTO;
            } 
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<int> GetCurrentAccountFromToken()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("No active HTTP context found.");
            }

            var user = httpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var accountIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (accountIdClaim == null)
            {
                throw new InvalidOperationException("No account ID claim found in token.");
            }

            if (!int.TryParse(accountIdClaim.Value, out var accountId))
            {
                throw new InvalidOperationException("Invalid account ID claim value.");
            }

            return accountId;
        }

        public async Task<ResponseDTO<AccountVM>> ConfirmEmailOtp(int accountId, int otp)
        {
            var responseDTO = new ResponseDTO<AccountVM>();
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByID(accountId);
                if (account is null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Cannot find any account with id = {accountId}!";
                    responseDTO.Data = null;
                    return responseDTO;
                }

                if (account.EmailOtp == otp)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Invalid otp!";
                    responseDTO.Data = null;
                    return responseDTO;
                } else
                {
                    account.EmailOtp = 0;
                    _unitOfWork.AccountRepository.Update(account);
                    await _unitOfWork.SaveAsync();
                    responseDTO.IsSuccess = true;
                    responseDTO.Message = "Otp is valid. Now, you can change your email!";
                    responseDTO.Data = _mapper.Map<Account, AccountVM>(account);
                    return responseDTO;
                }
                
            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<ResponseDTO<AccountVM>> ChangeEmail(int accountId, string newEmail)
        {
            var responseDTO = new ResponseDTO<AccountVM>();
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByID(accountId);
                if (account is null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Cannot find any account with id = {accountId}!";
                    responseDTO.Data = null;
                    return responseDTO;
                }

                account.Email = newEmail;
                _unitOfWork.AccountRepository.Update(account);
                await _unitOfWork.SaveAsync();
                responseDTO.IsSuccess = true;
                responseDTO.Message = "Change your email successfully!";
                responseDTO.Data = _mapper.Map<Account, AccountVM>(account);
                return responseDTO;

            }
            catch (Exception ex)
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;
                responseDTO.Data = null;
                return responseDTO;
            }
        }

        public async Task<bool> ChangePassword(int accountId, string password)
        {
            try
            {
                var account = await _unitOfWork.AccountRepository.GetByID(accountId);
                if (account is null)
                {
                    return false;
                }

                account.PasswordHash = PasswordHasher.HashPassword(password);
                _unitOfWork.AccountRepository.Update(account);
                await _unitOfWork.SaveAsync();
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
