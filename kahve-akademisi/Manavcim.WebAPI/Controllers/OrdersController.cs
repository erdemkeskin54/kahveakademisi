using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Entities.Infrastructure;
using KahveAkademisi.Entities.RequestModels.AuthControllerModels;
using KahveAkademisi.Entities.RequestModels.OrdersControllerModels;
using KahveAkademisi.Entities.ResponseModels.Client;
using KahveAkademisi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Client,Admin")]
    public class OrdersController : Controller
    {
        public IUnitOfWork _uow;
        public readonly ILogger _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<OrdersController> _localizer;
        private ApiResponsive apiResponsive;

        private List<ErrorModel> errorModels;

        public OrdersController(IUnitOfWork uow,
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


        #region client product function

        [HttpGet("getuserorders")]
        public async Task<IActionResult> GetUserOrdersAsync(int pageNumber, int pageSize)
        {
            errorModels = new List<ErrorModel>();

            if (pageNumber != 0 && pageSize != 0)
            {
                var currentUser = User.Identity.Name;
                if (currentUser != null)
                {
                    AppUser appUser = await _userManager.FindByNameAsync(currentUser);
                    if (appUser != null)
                    {
                        OperationResult operationResult_orders = _uow.Orders.GetUserOrders(currentUser, pageNumber, pageSize);
                        if (operationResult_orders.IsSuccess)
                        {
                            List<Order> orders = (List<Order>)operationResult_orders.ReturnObject;
                            List<OrderRES> orderRESs = orders.Select(x => x.OrderDTtoRES()).ToList();

                            return Ok(new ApiResponsive()
                            {
                                ErrorContent = null,
                                IsSucces = true,
                                ReturnObject = orderRESs
                            });
                        }
                        else
                        {
                            ErrorModel errorModel = new ErrorModel()
                            {
                                ErrorCode = MessageNumber.SiparislerGetirilemedi.ToString(),
                                ErrorMessage = _localizer["SiparislerGetirilemedi"]
                            };

                            errorModels.Add(errorModel);

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
            else
            {
                ErrorModel errorModel3 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.ParametrelerHatali.ToString(),
                    ErrorMessage = _localizer["ParametrelerHatali"]
                };

                errorModels.Add(errorModel3);

                apiResponsive = new ApiResponsive()
                {
                    IsSucces = false,
                    ReturnObject = null,
                    ErrorContent = errorModels
                };

                return BadRequest(apiResponsive);
            }
        }

        [HttpGet("getuserorderscount")]
        public async Task<IActionResult> GetUserOrdersCountAsync()
        {
            errorModels = new List<ErrorModel>();
            var currentUser = User.Identity.Name;
            if (currentUser != null)
            {
                AppUser appUser = await _userManager.FindByNameAsync(currentUser);
                if (appUser != null)
                {
                    OperationResult operationResult_ordersCount = _uow.Orders.GetUserOrdersCount(appUser.UserName);
                    if (operationResult_ordersCount.IsSuccess)
                    {
                        apiResponsive = new ApiResponsive()
                        {
                            ErrorContent = null,
                            IsSucces = true,
                            ReturnObject = operationResult_ordersCount.ReturnObject
                        };

                        return Ok(apiResponsive);
                    }
                    else
                    {
                        ErrorModel errorModel3 = new ErrorModel()
                        {
                            ErrorCode = MessageNumber.SiparisSayisiGetirilemedi.ToString(),
                            ErrorMessage = _localizer["SiparisSayisiGetirilemedi"]
                        };

                        errorModels.Add(errorModel3);

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


        #endregion

    }
}