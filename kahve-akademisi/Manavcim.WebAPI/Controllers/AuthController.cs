using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Entities.Infrastructure;
using KahveAkademisi.Entities.RequestModels.AuthControllerModels;
using KahveAkademisi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private ILogger<AuthController> _logger;
        private IPasswordHasher<AppUser> _passwordHasher;
        private IConfiguration _config;
        private readonly IStringLocalizer<AuthController> _localizer;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IUnitOfWork _uow;


        private List<ErrorModel> errorContent;


        public AuthController(IPasswordHasher<AppUser> passwordHasher,
            ILogger<AuthController> logger,
            IConfiguration config,
            IStringLocalizer<AuthController> localizer,

            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
            _logger = logger;
            _config = config;
            _localizer = localizer;
            _userManager = userManager;
            _roleManager = roleManager;
            _uow = unitOfWork;
            _signInManager = signInManager;


            errorContent = new List<ErrorModel>();

        }

        #region CreateToken Denemesi

        //[AllowAnonymous]
        //[HttpPost("token")]
        //public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        //{
        //    try
        //    {
        //        IActionResult response = Unauthorized();
        //        var user = await _userManager.FindByNameAsync(model.UserName);

        //        if (user != null)
        //        {
        //            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
        //            {
        //                var tokenString = BuildToken(user);
        //                return Ok(new { token = tokenString });
        //            }
        //        }
        //        else
        //        {
        //            ErrorModel errorModel = new ErrorModel()
        //            {
        //                ErrorCode = 3000,
        //                ErrorMessage = "Telefon numarası veya şifre uyuşmuyor"
        //            };

        //            List<ErrorModel> errorContent = new List<ErrorModel>();
        //            errorContent.Add(errorModel);

        //            return BadRequest(new ApiResponsive()
        //            {
        //                IsSucces = false,
        //                ReturnObject = null,
        //                ErrorContent = errorContent
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError($"JWT oluşturuken hata oluştu: {ex}");

        //        ErrorModel errorModel = new ErrorModel()
        //        {
        //            ErrorCode = 3000,
        //            ErrorMessage = "Bir Hata oluştu: " + ex
        //        };

        //        List<ErrorModel> errorContent = new List<ErrorModel>();
        //        errorContent.Add(errorModel);

        //        return BadRequest(new ApiResponsive()
        //        {
        //            IsSucces = false,
        //            ReturnObject = null,
        //            ErrorContent = errorContent
        //        });
        //    }

        //    return BadRequest(new ApiResponsive()
        //    {
        //        IsSucces = false,
        //        ReturnObject = null,
        //        ErrorContent = null
        //    });
        //}
        #endregion

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
      
            try
            {
                if (!ModelState.IsValid)
                {

                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            ErrorModel errorModel = new ErrorModel()
                            {
                                ErrorCode = MessageNumber.KullaniciBilgileriYanlis.ToString(),
                                ErrorMessage = _localizer[modelError.ErrorMessage].Value
                            };

                            errorContent.Add(errorModel);
                        }
                    }


                    return Ok(new ApiResponsive()
                    {
                        IsSucces = false,
                        ReturnObject = null,
                        ErrorContent = errorContent
                    });
                }


                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user != null)
                    {
                        JwtPacket jwtPacket = await BuildToken(user);

                        return Ok(new ApiResponsive()
                        {
                            IsSucces = true,
                            ReturnObject = jwtPacket,
                            ErrorContent = null
                        });
                    }
                    else
                    {
                        ErrorModel errorModel = new ErrorModel()
                        {
                            ErrorCode = MessageNumber.BuTelefonNumarasiZatenKayitli.ToString(),
                            ErrorMessage = _localizer["BuTelefonNumarasiZatenKayitli"].Value
                        };

                        errorContent.Add(errorModel);

                        return Ok(new ApiResponsive()
                        {
                            IsSucces = false,
                            ReturnObject = null,
                            ErrorContent = errorContent
                        });
                    }
                }
                else
                {
                    ErrorModel errorModel2 = new ErrorModel()
                    {
                        ErrorCode = MessageNumber.KullaniciAdiVeSifreUyusmadi.ToString(),
                        ErrorMessage = _localizer["KullaniciAdiVeSifreUyusmadi"].Value
                    };

                    errorContent.Add(errorModel2);

                    return Ok(new ApiResponsive()
                    {
                        IsSucces = false,
                        ReturnObject = null,
                        ErrorContent = errorContent
                    });

                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error in {nameof(Login)} : {ex}");

                ErrorModel errorModel2 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.BeklenmedikHata.ToString(),
                    ErrorMessage = _localizer["BeklenmedikHata"].Value
                };

                errorContent.Add(errorModel2);

                return BadRequest(new ApiResponsive()
                {
                    IsSucces = false,
                    ReturnObject = null,
                    ErrorContent = errorContent
                });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel_Req model)
        {

            if (!ModelState.IsValid)
            {

            }

            return Ok();


        }

        [NonAction]
        public void sendSMS(string phoneNumber, string verifyCode)
        {
            const string accountSid = "ACd0ab5b5be37c63c0f73f7b7a8ad9172b";
            const string authToken = "92ddeac2598c37e34d6e1ab9c4027588";
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber("+9" + phoneNumber);
            var message = MessageResource.Create(
                to,
                from: new PhoneNumber("+18652349788"),
                body: _localizer["DogrulamaKoduSmsi", verifyCode]);
        }
        [NonAction]
        private async Task<JwtPacket> BuildToken(AppUser user)
        {

            try
            {

                var userRole = await _userManager.GetRolesAsync(user);


                var claims = new List<Claim>{
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               };

                for (int i = 0; i < userRole.Count; i++)
                {
                    claims.Add(new Claim("role", userRole[i]));
                }




                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                  _config["Tokens:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(30),
                  signingCredentials: creds);

                return new JwtPacket
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo.ToString(),
                    UserName = user.UserName,
                    FirstName=user.FirstName,
                    LastName=user.LastName
                };
            }
            catch (Exception ex)
            {

                _logger.LogError($"Token Oluşturuken Hata {nameof(BuildToken)}: " + ex);
                return null;
            }

        }

        public class JwtPacket
        {
            public string Token { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Expiration { get; set; }
        }
    }
}