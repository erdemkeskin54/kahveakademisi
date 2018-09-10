using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Entities.Infrastructure;
using KahveAkademisi.Entities.ResponseModels.Client;
using KahveAkademisi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Addresses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Client,Admin")]
    public class AddressesController : Controller
    {
        public IUnitOfWork _uow;
        public readonly ILogger _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<OrdersController> _localizer;
        private ApiResponsive apiResponsive;

        private List<ErrorModel> errorModels;

        public AddressesController(IUnitOfWork uow,
                                ILoggerFactory loggerFactory,
                                UserManager<AppUser> userManager,
                                SignInManager<AppUser> signInManager,
                                RoleManager<IdentityRole> roleManager,
                                IStringLocalizer<OrdersController> localizer)
        {
            _uow = uow;
            _logger = loggerFactory.CreateLogger("OrdersController");
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _localizer = localizer;

            errorModels = new List<ErrorModel>();
        }



        [HttpGet("getuseraddresses")]
        public async Task<IActionResult> GetUserAddressesAsync()
        {
            try
            {
                var currentUser = User.Identity.Name;
                if (currentUser != null)
                {
                    AppUser appUser = await _userManager.FindByNameAsync(currentUser);
                    if (appUser != null)
                    {
                        OperationResult operationResult_addresses = _uow.UserAddress.FindAll(x => x.UserId == appUser.Id);
                        if (operationResult_addresses.IsSuccess)
                        {

                            List<UserAddress> userAddresses = (List<UserAddress>)operationResult_addresses.ReturnObject;
                            List<UserAdressRES> userAdressRESs = userAddresses.Select(x => x.UserAdressDTtoRES()).ToList();

                            apiResponsive = new ApiResponsive()
                            {
                                IsSucces = true,
                                ErrorContent = null,
                                ReturnObject = userAdressRESs
                            };

                            return Ok(apiResponsive);
                        }



                        ErrorModel errorModel1 = new ErrorModel()
                        {
                            ErrorCode = MessageNumber.AdresGetirirkenHata.ToString(),
                            ErrorMessage = _localizer["AdresGetirirkenHata"]
                        };

                        errorModels.Add(errorModel1);

                        apiResponsive = new ApiResponsive()
                        {
                            IsSucces = false,
                            ReturnObject = null,
                            ErrorContent = errorModels
                        };

                        return BadRequest(apiResponsive);

                    }
                }


                ErrorModel errorModel2 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.KullaniciBulunamadi.ToString(),
                    ErrorMessage = _localizer["KullaniciBulunamadi"]
                };

                errorModels.Add(errorModel2);

                apiResponsive = new ApiResponsive()
                {
                    IsSucces = false,
                    ReturnObject = null,
                    ErrorContent = errorModels
                };

                return BadRequest(apiResponsive);

            }
            catch (Exception ex)
            {

                _logger.LogError($"Error in {nameof(GetUserAddressesAsync)} : {ex}");

                ErrorModel errorModel2 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.BeklenmedikHata.ToString(),
                    ErrorMessage = _localizer["BeklenmedikHata"]
                };

                errorModels.Add(errorModel2);

                apiResponsive = new ApiResponsive()
                {
                    IsSucces = false,
                    ReturnObject = null,
                    ErrorContent = errorModels
                };

                return BadRequest(apiResponsive);

            }
        }


    }
}