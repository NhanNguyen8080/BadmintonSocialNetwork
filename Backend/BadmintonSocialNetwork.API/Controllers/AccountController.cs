using BadmintonSocialNetwork.API.Attributes;
using BadmintonSocialNetwork.Repository.Interfaces;
using BadmintonSocialNetwork.Service.DTOs;
using BadmintonSocialNetwork.Service.Helpers;
using BadmintonSocialNetwork.Service.Services;
using BadmintonSocialNetwork.Service.Ultities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSocialNetwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly IMailService _mailService;

        public AccountController(IAccountService accountService, 
                                 IUnitOfWork unitOfWork, 
                                 IJwtTokenFactory jwtTokenFactory,
                                 IMailService mailService)
        {
            _accountService = accountService;
            _unitOfWork = unitOfWork;
            _jwtTokenFactory = jwtTokenFactory;
            _mailService = mailService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginModel)
        {
            try
            {
                var account = await _accountService.GetAccountByUserNameOrEmail(loginModel.UsernameOrEmail);
                if (account.Data == null || !PasswordHasher.VerifyPassword(loginModel.Password, account.Data.PasswordHash))
                {
                    return Unauthorized("Invalid username/email or password!");
                }

                if (!account.Data.IsConfirmedEmail)
                {
                    await _mailService.SendMailAsync(new MailContent
                    {
                        To = account.Data.Email,
                        Subject = "Confirm your email",
                        Body = $"<h1>Welcome to Badminton Social Network</h1><p>Please click " +
                        $"<a href='https://localhost:7018/api/Account/confirm-email/{account.Data.Id}'>here</a> to confirm your email</p>"
                    });
                    return Unauthorized("Please check your email to confirm your email!");
                }

                var token = await _jwtTokenFactory.CreateTokenAsync(account.Data);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("sign-up")]   
        public async Task<IActionResult> SignUp([FromBody] AccountCM accountCM)
        {
            try
            {
                var response = await _accountService.SignUp(accountCM);
                if (response.IsSuccess)
                {
                    return Ok(new { Message = response.Message, Data = response.Data } );
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                if (_jwtTokenFactory.ValidateRefreshToken(refreshToken, out var username))
                {
                    var account = await _accountService.GetAccountByUserNameOrEmail(username);
                    if (account.Data == null)
                    {
                        return Unauthorized("Invalid refresh token!");
                    }

                    var token = await _jwtTokenFactory.CreateTokenAsync(account.Data);
                    return Ok(token);
                }
                return Unauthorized("Invalid refresh token!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout([FromBody] string refreshToken)
        {
            try
            {
                _jwtTokenFactory.RevokeRefreshToken(refreshToken);
                return Ok("Logged out successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("send-otp-to-mail")]
        public async Task<IActionResult> SendOtpToMail([FromBody] string email)
        {
            try
            {
                var account = await _accountService.GetAccountByUserNameOrEmail(email);
                var verificationCode = new Random().Next(100000, 999999);
                var resetPasswordUrl = $"https://localhost:7018/api/Account/reset-password?email={email}&code={verificationCode}";
                //email content
                var bodyContent = $@"
<table width='100%' cellspacing='0' cellpadding='0' border='0' style='background-color:#f9f9f9; padding:20px;'>
    <tr>
        <td align='center'>
            <table width='600px' cellspacing='0' cellpadding='0' border='0' style='background-color:#ffffff; border-radius:8px; padding:20px;'>
                <tr>
                    <td align='center' style='padding-bottom:20px;'>
                        <img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' width='50' alt='Instagram Logo'>
                        <h2 style='font-family: Arial, sans-serif; color:#333;'>Facebad</h2>
                    </td>
                </tr>
                <tr>
                    <td style='font-family: Arial, sans-serif; font-size:16px; color:#333; padding:10px 20px;'>
                        <p>Hello <b>{account.Data.UserName}</b>!</p>
                        <p>Someone has attempted to change the email associated with your Facebad account.</p>
                        <p>If this was you, please use the following code to verify your identity:</p>
                        <p style='font-size:24px; font-weight:bold; color:#333; background-color:#f3f3f3; padding:10px; text-align:center; border-radius:5px;'>
                            {verificationCode}
                        </p>
                        <p>If this wasn't you, please <a href='{resetPasswordUrl}' style='color:#007bff; text-decoration:none;'>reset your password</a> to secure your account.</p>
                    </td>
                </tr>
                <tr>
                    <td align='center' style='padding-top:20px; font-size:12px; color:#777;'>
                        <p>from <b style='color:#d93025;'>Meta</b></p>
                        <p>© Facebad. Meta Platforms, Inc., 1601 Willow Road, Menlo Park, CA 94025</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>";

                var mailContent = new MailContent
                {
                    To = email,
                    Subject = "Confirm your email",
                    Body = bodyContent,
                };
                var response = await _mailService.SendMailAsync(mailContent);
                if (response)
                {
                    account.Data.EmailOtp = verificationCode;
                    await _unitOfWork.SaveAsync();
                    return Ok("Check your email to confirm email!");
                }
                return BadRequest("Send mail unsuccessfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("User")]
        [HttpPut]
        [Route("confirm-email-otp")]
        public async Task<IActionResult> ConfirmEmailOtp([FromBody] int otp)
        {
            try
            {
                var accountId = await _accountService.GetCurrentAccountFromToken();
                var response = await _accountService.ConfirmEmailOtp(accountId, otp);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.Message);
                }

                await _unitOfWork.SaveAsync();
                return Ok(new {Message = response.Message, Data = response.Data});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("User")]
        [HttpPut]
        [Route("change-email")]
        public async Task<IActionResult> ChangeEmail([FromBody] string newEmail)
        {
            try
            {
                var accountId = await _accountService.GetCurrentAccountFromToken();
                var response = await _accountService.ChangeEmail(accountId, newEmail);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.Message);
                }

                await _unitOfWork.SaveAsync();
                return Ok(new { Message = response.Message, Data = response.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("confirm-email/{accountId}")]
        public async Task<IActionResult> ConfirmEmail(int accountId)
        {
            try
            {
                var response = await _accountService.ConfirmEmail(accountId);
                if (response.IsSuccess)
                {
                    return Ok(new { Message = response.Message, Data = response.Data });
                }
                return BadRequest(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("Admin")]
        [HttpGet]
        [Route("get-accounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] DefaultSearch defaultSearch)
        {
            try
            {
                var accounts = await _accountService.GetAccounts(defaultSearch.PageIndex, defaultSearch.PageSize);
                return Ok(accounts);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("Admin")]
        [HttpGet]
        [Route("get-account-by-id")]
        public async Task<IActionResult> GetAccountById(int accountId)
        {
            try
            {
                var account = await _accountService.GetAccountById(accountId);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("Admin")]
        [HttpDelete]
        [Route("delete-account/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _accountService.DeleteAccount(id);
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("Admin")]
        [HttpPut]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email, string password, string confirmPassword)
        {
            try
            {
                var account = await _accountService.GetAccountByUserNameOrEmail(email);
                if (password.Equals(confirmPassword))
                {
                    return BadRequest("Password and confirm password are not matched!");
                }

                var response = await _accountService.ChangePassword(account.Data.Id, password);
                if (!response)
                {
                    return BadRequest("Change password unsuccessfully!");
                }

                await _unitOfWork.SaveAsync();
                return Ok("Change password successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("Admin", "User")]
        [HttpPut]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(string password, string confirmPassword)
        {
            try
            {
                var accountId = await _accountService.GetCurrentAccountFromToken();
                if (accountId == 0)
                {
                    return Unauthorized("Invalid token!");
                }

                if (password.Equals(confirmPassword))
                {
                    return BadRequest("Password and confirm password are not matched!");
                }

                var response = await _accountService.ChangePassword(accountId, password);

                if (!response)
                {
                    return BadRequest("Change password unsuccessfully!");
                }

                await _unitOfWork.SaveAsync();
                return Ok("Change password successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("Admin", "User")]
        [HttpPut]
        [Route("update-profile/{accountId}")]
        public async Task<IActionResult> UpdateProfile(int accountId, [FromBody] AccountUM accountUM)
        {
            try
            {
                var response = await _accountService.UpdateProfile(accountId, accountUM);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.Message);
                }

                await _unitOfWork.SaveAsync();
                return Ok(new { Message = response.Message, Data = response.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [RolesAuthorize("Admin", "User")]
        [HttpPut]
        [Route("update-avatar/{accountId}")]
        public async Task<IActionResult> UpdateAvatar(int accountId, IFormFile avatarFile)
        {
            try
            {
                var response = await _accountService.UpdateAvatar(accountId, avatarFile);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.Message);
                }

                await _unitOfWork.SaveAsync();
                return Ok(new { Message = response.Message, Data = response.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
