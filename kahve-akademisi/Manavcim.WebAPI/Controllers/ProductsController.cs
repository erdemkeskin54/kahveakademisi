using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    [Route("api/Products")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Client,Admin")]
    public class ProductsController : Controller
    {
        public IUnitOfWork _uow;
        public readonly ILogger _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<ProductsController> _localizer;
        private ApiResponsive apiResponsive;

        private List<ErrorModel> errorModels;

        public ProductsController(IUnitOfWork uow,
                        ILoggerFactory loggerFactory,
                        UserManager<AppUser> userManager,
                        SignInManager<AppUser> signInManager,
                        RoleManager<IdentityRole> roleManager,
                        IStringLocalizer<ProductsController> localizer)
        {
            _uow = uow;
            _logger = loggerFactory.CreateLogger("ProductsController");
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _localizer = localizer;

            errorModels = new List<ErrorModel>();
        }

        #region client product function
    
        [HttpGet("getallproducts")]
        public IActionResult GetAllProducts()
        {
            errorModels = new List<ErrorModel>();
            OperationResult operationResult_products = _uow.Products.GetAllProducts();
            if (operationResult_products.IsSuccess)
            {
                List<Product> products = (List<Product>)operationResult_products.ReturnObject;

                List<ProductRES> productRESs = products.Select(x => x.ProductDTtoRES()).ToList();
             
                apiResponsive = new ApiResponsive
                {
                    IsSucces = true,
                    ErrorContent = null,
                    ReturnObject = productRESs
                };

                return Ok(apiResponsive);
            }
            else
            {

                ErrorModel errorModel2 = new ErrorModel()
                {
                    ErrorCode = MessageNumber.UrunGetirilemedi.ToString(),
                    ErrorMessage = _localizer["UrunGetirilemedi"]
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

        [HttpGet("getproductdetail")]
        public IActionResult GetProductDetail(int id)
        {
            if (id != 0)
            {
                errorModels = new List<ErrorModel>();
                OperationResult operationResult_products = _uow.Products.GetProductDetail(id);
                if (operationResult_products.IsSuccess)
                {
                    Product product = (Product)operationResult_products.ReturnObject;

                    ProductRES productRES = product.ProductDTtoRES();

                    apiResponsive = new ApiResponsive
                    {
                        IsSucces = true,
                        ErrorContent = null,
                        ReturnObject = productRES
                    };

                    return Ok(apiResponsive);
                }
                else
                {

                    ErrorModel errorModel2 = new ErrorModel()
                    {
                        ErrorCode = MessageNumber.UrunGetirilemedi.ToString(),
                        ErrorMessage = _localizer["UrunGetirilemedi"]
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

        #endregion
        #region admin product function


        #endregion
    }
}