using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Entities.Infrastructure;
using KahveAkademisi.Entities.RequestModels.CartControllerModels;
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
    [Route("api/Cart")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Client,Admin")]
    public class CartController : Controller
    {
        public IUnitOfWork _uow;
        public readonly ILogger _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<ProductsController> _localizer;
        private ApiResponsive apiResponsive;

        private List<ErrorModel> errorModels;

        public CartController(IUnitOfWork uow,
                        ILoggerFactory loggerFactory,
                        UserManager<AppUser> userManager,
                        SignInManager<AppUser> signInManager,
                        RoleManager<IdentityRole> roleManager,
                        IStringLocalizer<ProductsController> localizer)
        {
            _uow = uow;
            _logger = loggerFactory.CreateLogger("CartController");
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _localizer = localizer;

            errorModels = new List<ErrorModel>();
        }


        [HttpPost("addtocart")]
        public async Task<IActionResult> AddToCartAsync([FromBody] List<CartModel_req> carts)
        {
            try
            {
                var currentUser = User.Identity.Name;
                Cart newCart = null;
                if (currentUser != null)
                {

                    AppUser appUser = await _userManager.FindByNameAsync(currentUser);

                    OperationResult carts_operationResult = _uow.Carts.GetMyCarts(currentUser);
                    if (carts_operationResult.IsSuccess)
                    {
                        List<Cart> cartsModel = (List<Cart>)carts_operationResult.ReturnObject;
                        List<Cart> addCarts = new List<Cart>();


                        foreach (var cart in cartsModel)
                        {
                            for (int i = 0; i < carts.Count; i++)
                            {
                                if (carts[i].Id == cart.ProductAmountType.Id)
                                {
                                    cart.Quantity = cart.Quantity + carts[i].Quantity;
                                    carts.Remove(carts[i]);
                                }
                            }
                        }
                        _uow.SaveChanges();

                        foreach (var cart in carts)
                        {
                            OperationResult productAmountType_operationResult = _uow.ProductAmountTypes.Get(cart.Id);
                            if (productAmountType_operationResult.IsSuccess && productAmountType_operationResult.ReturnObject != null)
                            {
                                newCart = new Cart()
                                {
                                    AddToCartDate = DateTime.Now,
                                    Quantity = cart.Quantity,
                                    User = appUser,
                                    ProductAmountType = (ProductAmountType)productAmountType_operationResult.ReturnObject,
                                };

                                addCarts.Add(newCart);
                            }
                            else
                            {
                                //hata
                            }

                        }

                        _uow.Carts.AddRange(addCarts);

                        apiResponsive = new ApiResponsive()
                        {
                            ErrorContent = null,
                            IsSucces = true,
                            ReturnObject = null
                        };
                        return Ok(apiResponsive);
                    }
                    else
                    {
                        ErrorModel errorModel3 = new ErrorModel()
                        {
                            ErrorCode = MessageNumber.SepetGetirilemedi.ToString(),
                            ErrorMessage = _localizer["SepetGetirilemedi"]
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
                else
                {
                    ErrorModel errorModel3 = new ErrorModel()
                    {
                        ErrorCode = MessageNumber.KullaniciBulunamadi.ToString(),
                        ErrorMessage = _localizer["KullaniciBulunamadi"]
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
            catch (Exception ex)
            {
                ErrorModel errorModel3 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.BeklenmedikHata.ToString(),
                    ErrorMessage = _localizer["BeklenmedikHata"]
                };

                errorModels.Add(errorModel3);

                _logger.LogError($"Error in {nameof(AddToCartAsync)} : {ex}");


                apiResponsive = new ApiResponsive()
                {
                    IsSucces = false,
                    ReturnObject = null,
                    ErrorContent = errorModels
                };

                return BadRequest(apiResponsive);
            }




        }

        [HttpGet("getcart")]
        public IActionResult GetCart()
        {
            try
            {
                var currentUser = User.Identity.Name;
                if (currentUser != null)
                {
                    OperationResult operationResult_carts = _uow.Carts.GetMyCarts(currentUser);
                    if (operationResult_carts.IsSuccess)
                    {
                        List<Cart> carts = (List<Cart>)operationResult_carts.ReturnObject;

                        apiResponsive = new ApiResponsive()
                        {
                            ErrorContent = null,
                            IsSucces = true,
                            ReturnObject = carts.Select(x => x.CartDTtoRES())
                        };

                        return Ok(apiResponsive);
                    }
                }

                ErrorModel errorModel3 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.KullaniciBulunamadi.ToString(),
                    ErrorMessage = _localizer["KullaniciBulunamadi"]
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
            catch (Exception ex)
            {
                ErrorModel errorModel3 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.BeklenmedikHata.ToString(),
                    ErrorMessage = _localizer["BeklenmedikHata"]
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

        [HttpPost("deletetocart")]
        public async Task<IActionResult> DeleteToCart([FromBody]int cartId)
        {
            try
            {
                var currentUser = User.Identity.Name;
                if (currentUser == null)
                {
                    ErrorModel errorModel3 = new ErrorModel()
                    {
                        ErrorCode = MessageNumber.KullaniciBulunamadi.ToString(),
                        ErrorMessage = _localizer["KullaniciBulunamadi"]
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

                if (cartId != 0)
                {
                    AppUser appUser = await _userManager.FindByNameAsync(currentUser);
                    if (appUser != null)
                    {
                        OperationResult operationResult_deleteCart = _uow.Carts.Find(x => x.Id == cartId & x.User.Id==appUser.Id);
                        if (operationResult_deleteCart.IsSuccess)
                        {
                            Cart cart = (Cart)operationResult_deleteCart.ReturnObject;
                            if (cart != null)
                            {
                                OperationResult operationResult_deletedCart = _uow.Carts.Delete(cart);
                                if (operationResult_deletedCart.IsSuccess)
                                {
                                    apiResponsive = new ApiResponsive()
                                    {
                                        IsSucces = true,
                                        ErrorContent = null,
                                        ReturnObject = null
                                    };

                                    return Ok(apiResponsive);
                                }
                                else
                                {
                                    ErrorModel errorModel3 = new ErrorModel()
                                    {
                                        ErrorCode = MessageNumber.SepetSilinemedi.ToString(),
                                        ErrorMessage = _localizer["SepetSilinemedi"]
                                    };
                                    errorModels.Add(errorModel3);
                                    apiResponsive = new ApiResponsive()
                                    {
                                        IsSucces = false,
                                        ReturnObject = null,
                                        ErrorContent = errorModels
                                    };

                                    return Ok(apiResponsive);
                                }
                            }
                            else
                            {
                                ErrorModel errorModel3 = new ErrorModel()
                                {
                                    ErrorCode = MessageNumber.SepetBulunamadi.ToString(),
                                    ErrorMessage = _localizer["SepetBulunamadi"]
                                };
                                errorModels.Add(errorModel3);
                                apiResponsive = new ApiResponsive()
                                {
                                    IsSucces = false,
                                    ReturnObject = null,
                                    ErrorContent = errorModels
                                };

                                return Ok(apiResponsive);
                            }
                        }
                        else
                        {
                            ErrorModel errorModel3 = new ErrorModel()
                            {
                                ErrorCode = MessageNumber.SepetBulunamadi.ToString(),
                                ErrorMessage = _localizer["SepetBulunamadi"]
                            };
                            errorModels.Add(errorModel3);
                            apiResponsive = new ApiResponsive()
                            {
                                IsSucces = false,
                                ReturnObject = null,
                                ErrorContent = errorModels
                            };

                            return Ok(apiResponsive);
                        }
                    }
                    else
                    {
                        ErrorModel errorModel3 = new ErrorModel()
                        {
                            ErrorCode = MessageNumber.KullaniciBulunamadi.ToString(),
                            ErrorMessage = _localizer["KullaniciBulunamadi"]
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
            catch
            {
                ErrorModel errorModel3 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.BeklenmedikHata.ToString(),
                    ErrorMessage = _localizer["BeklenmedikHata"]
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

        [HttpPost("updatetocart")]
        public async Task<IActionResult> UpdateToCart([FromBody]List<CartModel_req> carts)
        {

            try
            {
                var currentUser = User.Identity.Name;
                if (currentUser == null)
                {
                    ErrorModel errorModel3 = new ErrorModel()
                    {
                        ErrorCode = MessageNumber.KullaniciBulunamadi.ToString(),
                        ErrorMessage = _localizer["KullaniciBulunamadi"]
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

                AppUser appUser =await _userManager.FindByNameAsync(currentUser);

                if (appUser != null)
                {
                    OperationResult operationResult_cartsModel = _uow.Carts.GetMyCarts(appUser.UserName);
                    if (operationResult_cartsModel.IsSuccess)
                    {
                        List<Cart> cartsModel = (List<Cart>)operationResult_cartsModel.ReturnObject;
                        foreach (var cart in carts)
                        {
                            foreach (var cartModel in cartsModel)
                            {
                                if(cart.Id==cartModel.Id)
                                {
                                    cartModel.Quantity = cart.Quantity;
                                }
                            }
                        }

                        _uow.SaveChanges();


                        apiResponsive = new ApiResponsive()
                        {
                            IsSucces = true,
                            ReturnObject = null,
                            ErrorContent = null
                        };

                        return Ok(apiResponsive);

                    }
                    else
                    {
                        ErrorModel errorModel3 = new ErrorModel()
                        {
                            ErrorCode = MessageNumber.SepetGetirilemedi.ToString(),
                            ErrorMessage = _localizer["SepetGetirilemedi"]
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
                else
                {
                    ErrorModel errorModel3 = new ErrorModel()
                    {
                        ErrorCode = MessageNumber.KullaniciBulunamadi.ToString(),
                        ErrorMessage = _localizer["KullaniciBulunamadi"]
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
            catch
            {
                ErrorModel errorModel3 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.BeklenmedikHata.ToString(),
                    ErrorMessage = _localizer["BeklenmedikHata"]
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
}