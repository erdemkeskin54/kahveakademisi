using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Entities.Infrastructure;
using KahveAkademisi.Entities.RequestModels.CheckoutControllerModels;
using KahveAkademisi.Utility;
using KahveAkademisi.WebAPI.Models.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Checkout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Client,Admin")]
    public class CheckoutController : Controller
    {
        public IUnitOfWork _uow;
        public readonly ILogger _logger;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<CheckoutController> _localizer;
        private IHttpContextAccessor _accessor;
        private ApiResponsive apiResponsive;

        private List<ErrorModel> errorModels;

        private IHubContext<MyHub, ITypedHubClient> _hubContext;


        public CheckoutController(IUnitOfWork uow,
                        ILoggerFactory loggerFactory,
                        UserManager<AppUser> userManager,
                        SignInManager<AppUser> signInManager,
                        RoleManager<IdentityRole> roleManager,
                        IStringLocalizer<CheckoutController> localizer,
                        IHttpContextAccessor accessor,
                        IHubContext<MyHub, ITypedHubClient> hubContext)
        {
            _uow = uow;
            _logger = loggerFactory.CreateLogger("CheckoutController");
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _accessor = accessor;
            _hubContext = hubContext;

            errorModels = new List<ErrorModel>();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutModel_req checkoutModel)
        {
            try
            {
                var currentUser = User.Identity.Name;
                if (currentUser == null)
                {
                    ErrorModel errorModel3 = new ErrorModel()
                    {
                        ErrorCode = MessageNumber.KullaniciBulunamadi.GetHashCode().ToString(),
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

                AppUser appUser = await _userManager.FindByNameAsync(currentUser);
                if (appUser != null)
                {
                    if (!ModelState.IsValid)
                    {
                        ErrorModel errorModel3 = new ErrorModel()
                        {
                            ErrorCode = MessageNumber.ParametrelerHatali.GetHashCode().ToString(),
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

                    if (checkoutModel.PaymentMethod == PaymentMethod.CreditCard)
                    {
                        if (checkoutModel.OldAddressId != 0)
                        {
                            OperationResult operationResult_address = _uow.UserAddress.GetUserAddress(checkoutModel.OldAddressId);
                            if (operationResult_address.IsSuccess && operationResult_address.ReturnObject != null)
                            {
                                Order order = new Order()
                                {
                                    UserId = appUser.Id,
                                    OrderDate = DateTime.Now,
                                    OrderStatus = OrderStatus.NotApproved,
                                    UserAddressId = checkoutModel.OldAddressId,
                                    TotalPrice = 0
                                };

                                OperationResult operationResult_addOrder = _uow.Orders.Add(order);
                                if (operationResult_addOrder.IsSuccess)
                                {
                                    OperationResult operationResult_carts = _uow.Carts.GetMyCarts(currentUser);
                                    if (operationResult_carts.IsSuccess)
                                    {
                                        List<Cart> myCart = (List<Cart>)operationResult_carts.ReturnObject;
                                        if (myCart.Count != 0)
                                        {
                                            Order order2 = (Order)operationResult_addOrder.ReturnObject;

                                            ThreedsInitialize payment = sendIyzico(appUser, myCart, checkoutModel, (UserAddress)operationResult_address.ReturnObject, order2.Id.ToString(), _accessor.HttpContext.Connection.RemoteIpAddress.ToString());
                                            if (payment.Status == "success")
                                            {

                                                apiResponsive = new ApiResponsive()
                                                {
                                                    ErrorContent = null,
                                                    IsSucces = true,
                                                    ReturnObject = payment.HtmlContent
                                                };

                                                return Ok(apiResponsive);
                                            }
                                            else
                                            {
                                                _uow.Orders.Delete(order2);

                                                ErrorModel errorModel3 = new ErrorModel()
                                                {
                                                    ErrorCode = MessageNumber.OdemeBasarisiz.GetHashCode().ToString(),
                                                    ErrorMessage = _localizer["OdemeBasarisiz"]
                                                };

                                                errorModels.Add(errorModel3);

                                                ErrorModel errorModel = new ErrorModel()
                                                {
                                                    ErrorCode = payment.ErrorCode,
                                                    ErrorMessage = payment.ErrorMessage

                                                };

                                                errorModels.Add(errorModel);

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

                                        return Ok(apiResponsive);
                                    }
                                }
                                else
                                {
                                    ErrorModel errorModel3 = new ErrorModel()
                                    {
                                        ErrorCode = MessageNumber.SiparisEklenemedi.ToString(),
                                        ErrorMessage = _localizer["SiparisEklenemedi"]
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
                                    ErrorCode = MessageNumber.AdresBulunamadi.GetHashCode().ToString(),
                                    ErrorMessage = _localizer["AdresBulunamadi"]
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
                            UserAddress userAddress = new UserAddress()
                            {
                                UserId = appUser.Id,
                                Adress = checkoutModel.Address,
                                AdressTitle = checkoutModel.AddressTitle,
                                City = checkoutModel.City,
                                District = checkoutModel.District,
                                Neighborhood = checkoutModel.Neighborhood,
                                Zip = checkoutModel.Zip

                            };
                            OperationResult operationResult_addAddress = _uow.UserAddress.Add(userAddress);
                            if (operationResult_addAddress.IsSuccess)
                            {
                                UserAddress userAddress2 = (UserAddress)operationResult_addAddress.ReturnObject;
                                OperationResult operationResult_carts = _uow.Carts.GetMyCarts(currentUser);
                                if (operationResult_carts.IsSuccess)
                                {
                                    List<Cart> myCart = (List<Cart>)operationResult_carts.ReturnObject;
                                    if (myCart.Count != 0)
                                    {
                                        Order order = new Order()
                                        {
                                            UserId = appUser.Id,
                                            OrderDate = DateTime.Now,
                                            OrderStatus = OrderStatus.NotApproved,
                                            UserAddressId = userAddress2.Id,
                                            TotalPrice = 0
                                        };
                                        OperationResult operationResult_addOrder = _uow.Orders.Add(order);
                                        if (operationResult_addOrder.IsSuccess)
                                        {
                                            Order order2 = (Order)operationResult_addOrder.ReturnObject;
                                            ThreedsInitialize payment = sendIyzico(appUser, myCart, checkoutModel, userAddress, order2.Id.ToString(), _accessor.HttpContext.Connection.RemoteIpAddress.ToString());
                                            if (payment.Status == "success")
                                            {

                                                apiResponsive = new ApiResponsive()
                                                {
                                                    ErrorContent = null,
                                                    IsSucces = true,
                                                    ReturnObject = payment.HtmlContent
                                                };

                                                return Ok(apiResponsive);
                                            }
                                            else
                                            {
                                                _uow.Orders.Delete(order2);
                                                _uow.UserAddress.Delete(userAddress2);


                                                ErrorModel errorModel3 = new ErrorModel()
                                                {
                                                    ErrorCode = MessageNumber.OdemeBasarisiz.GetHashCode().ToString(),
                                                    ErrorMessage = _localizer["OdemeBasarisiz"]
                                                };

                                                errorModels.Add(errorModel3);

                                                ErrorModel errorModel = new ErrorModel()
                                                {
                                                    ErrorCode = payment.ErrorCode,
                                                    ErrorMessage = payment.ErrorMessage

                                                };

                                                errorModels.Add(errorModel);

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

                                            _uow.UserAddress.Delete(userAddress2);

                                            ErrorModel errorModel3 = new ErrorModel()
                                            {
                                                ErrorCode = MessageNumber.SiparisEklenemedi.GetHashCode().ToString(),
                                                ErrorMessage = _localizer["SiparisEklenemedi"]
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
                                        _uow.UserAddress.Delete(userAddress2);

                                        ErrorModel errorModel3 = new ErrorModel()
                                        {
                                            ErrorCode = MessageNumber.SepetinizBos.GetHashCode().ToString(),
                                            ErrorMessage = _localizer["SepetinizBos"]
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

                                    _uow.UserAddress.Delete(userAddress2);

                                    ErrorModel errorModel3 = new ErrorModel()
                                    {
                                        ErrorCode = MessageNumber.SepetGetirilemedi.GetHashCode().ToString(),
                                        ErrorMessage = _localizer["SepetGetirilemedi"]
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
                                    ErrorCode = MessageNumber.AdresEklenemedi.GetHashCode().ToString(),
                                    ErrorMessage = _localizer["AdresEklenemedi"]
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
                    }
                    else if (checkoutModel.PaymentMethod == PaymentMethod.PaymentAtTheDoor)
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
                        apiResponsive = new ApiResponsive()
                        {
                            IsSucces = true,
                            ErrorContent = null,
                            ReturnObject = null
                        };

                        return Ok(apiResponsive);
                    }
                }
                else
                {
                    ErrorModel errorModel3 = new ErrorModel()
                    {
                        ErrorCode = MessageNumber.KullaniciBulunamadi.GetHashCode().ToString(),
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

                _logger.LogError($"Error in {nameof(CheckoutController)} : {ex}");
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

        [HttpPost("callback")]
        [AllowAnonymous]
        public void Callback(string status, string paymentId, string conversationData, string conversationId, string mdStatus)
        {
            try
            {
                if (status == "success")
                {
                    if (mdStatus == "1")
                    {
                        Options options = new Options();
                        options.ApiKey = "sandbox-OPMmAWhaJiQntmjPPVrolCuJ8XLWITGd";
                        options.SecretKey = "sandbox-wPykKvg4r9BZUGAlvFP08eOf90iYY9oS";
                        options.BaseUrl = "https://sandbox-api.iyzipay.com";

                        CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest();
                        request.Locale = Locale.TR.ToString();
                        request.ConversationId = conversationId;
                        request.PaymentId = paymentId;
                        request.ConversationData = conversationData;

                        ThreedsPayment threedsPayment = ThreedsPayment.Create(request, options);

                        if(threedsPayment.Status=="success")
                        {
                            OperationResult operationResult_order = _uow.Orders.Get(int.Parse(conversationId));
                            if (operationResult_order.IsSuccess)
                            {
                                Order order = (Order)operationResult_order.ReturnObject;
                                if (order != null)
                                {
                                    OperationResult operationResult_carts = _uow.Carts.GetMyCartsWithUserId(order.UserId);
                                    if (operationResult_carts.IsSuccess)
                                    {
                                        List<Cart> carts = (List<Cart>)operationResult_carts.ReturnObject;
                                        if (carts != null)
                                        {
                                            List<OrderDetail> orderDetails = new List<OrderDetail>();
                                            foreach (var cart in carts)
                                            {
                                                OrderDetail orderDetail2 = new OrderDetail()
                                                {
                                                    Order = order,
                                                    OrderStatus = OrderStatus.Approved,
                                                    Quantity = cart.Quantity,
                                                    ProductAmountTypeId = cart.ProductAmountType.Id,
                                                    TotalPrice = cart.ProductAmountType.Price * cart.Quantity,
                                                };

                                                orderDetails.Add(orderDetail2);
                                            }

                                            OperationResult operationResult_orderDetails = _uow.OrderDetails.AddRange(orderDetails);
                                            if (operationResult_orderDetails.IsSuccess)
                                            {
                                                _hubContext.Clients.All.checkoutNav();

                                                foreach (var cart in carts)
                                                {
                                                    _uow.Carts.Delete(cart);
                                                }


                                                order.OrderStatus = OrderStatus.Approved;
                                                _uow.SaveChanges();

                                                PaymentResult paymentResult = new PaymentResult()
                                                {
                                                    AuthCode = threedsPayment.AuthCode,
                                                    BasketId= threedsPayment.BasketId,
                                                    BinNumber= threedsPayment.BinNumber,
                                                    CardAssociation= threedsPayment.CardAssociation,
                                                    CardFamily= threedsPayment.CardFamily,
                                                    CardToken= threedsPayment.CardToken,
                                                    CardType= threedsPayment.CardType,
                                                    CardUserKey= threedsPayment.CardUserKey,
                                                    ConnectorName= threedsPayment.ConnectorName,
                                                    IyziCommissionRateAmount= threedsPayment.IyziCommissionRateAmount,
                                                    Currency= threedsPayment.Currency,
                                                    FraudStatus= threedsPayment.FraudStatus,
                                                    Installment = threedsPayment.Installment,
                                                    IyziCommissionFee = threedsPayment.IyziCommissionFee,
                                                    MerchantCommissionRate = threedsPayment.IyziCommissionFee,
                                                    MerchantCommissionRateAmount = threedsPayment.MerchantCommissionRateAmount,
                                                    PaidPrice = threedsPayment.PaidPrice,
                                                    PaymentId = threedsPayment.PaymentId,
                                                    PaymentStatus = threedsPayment.PaymentStatus,
                                                    Phase=threedsPayment.Phase,
                                                    Price= threedsPayment.Price,
                                                    PaymentItems=threedsPayment.PaymentItems.Select(x=> new Entities.DbModels.PaymentItem() {

                                                        BlockageRate=x.BlockageRate,
                                                        BlockageRateAmountMerchant=x.BlockageRateAmountMerchant,
                                                        BlockageRateAmountSubMerchant=x.BlockageRateAmountSubMerchant,
                                                        BlockageResolvedDate=x.BlockageResolvedDate,
                                                        ItemId=x.ItemId,
                                                        IyziCommissionFee=x.IyziCommissionFee,
                                                        ConvertedPayout=new Entities.DbModels.ConvertedPayout()
                                                        {
                                                            BlockageRateAmountMerchant=x.ConvertedPayout.BlockageRateAmountMerchant,
                                                            BlockageRateAmountSubMerchant=x.ConvertedPayout.BlockageRateAmountSubMerchant,
                                                            Currency=x.ConvertedPayout.Currency,
                                                            IyziCommissionFee=x.ConvertedPayout.IyziCommissionFee,
                                                            IyziCommissionRateAmount=x.ConvertedPayout.IyziCommissionRateAmount,
                                                            IyziConversionRate=x.ConvertedPayout.IyziConversionRate,
                                                            IyziConversionRateAmount=x.ConvertedPayout.IyziConversionRateAmount,
                                                            MerchantPayoutAmount=x.ConvertedPayout.MerchantPayoutAmount,
                                                            PaidPrice=x.ConvertedPayout.PaidPrice,
                                                            SubMerchantPayoutAmount=x.ConvertedPayout.SubMerchantPayoutAmount
                                                        },
                                                        IyziCommissionRateAmount=x.IyziCommissionRateAmount,
                                                        MerchantCommissionRate=x.MerchantCommissionRate,
                                                        MerchantCommissionRateAmount=x.MerchantCommissionRateAmount,
                                                        MerchantPayoutAmount=x.MerchantPayoutAmount,
                                                        PaidPrice=x.PaidPrice,
                                                        PaymentTransactionId=x.PaymentTransactionId,
                                                        Price=x.Price,
                                                        SubMerchantKey=x.SubMerchantKey,
                                                        SubMerchantPayoutAmount=x.SubMerchantPayoutAmount,
                                                        SubMerchantPayoutRate=x.SubMerchantPayoutRate,
                                                        SubMerchantPrice=x.SubMerchantPrice,
                                                        TransactionStatus=x.TransactionStatus
                                                    }).ToList(),
                                                    OrderId=order.Id,
                                                    UserId=order.UserId
                                                    
                                                };

                                                _uow.PaymentResults.Add(paymentResult);
                                    
                                            }
                                            else
                                            {

                                            }

                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {

                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }



                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

                throw;
            }

      

        }

        [NonAction]
        private ThreedsInitialize sendIyzico(AppUser appUser, List<Cart> carts, CheckoutModel_req checkoutModel_Req, UserAddress userAddress, string orderId, string ip)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-OPMmAWhaJiQntmjPPVrolCuJ8XLWITGd";
            options.SecretKey = "sandbox-wPykKvg4r9BZUGAlvFP08eOf90iYY9oS";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            List<BasketItem> basketItems = new List<BasketItem>();
            double totalPrice = 0;


            CreatePaymentRequest request = new CreatePaymentRequest();

            for (int i = 0; i < carts.Count; i++)
            {
                BasketItem basket = new BasketItem();
                basket.Id = carts[i].ProductAmountType.Id.ToString();
                basket.Name = carts[i].ProductAmountType.Product.ProductName;
                basket.ItemType = BasketItemType.PHYSICAL.ToString();
                basket.Price = carts[i].ProductAmountType.Price.ToString();
                basket.Category1 = carts[i].ProductAmountType.Product.ProductCategories[0].Category.CategoryName;
                basketItems.Add(basket);

                totalPrice = totalPrice + carts[i].ProductAmountType.Price;
            }
            request.BasketItems = basketItems;

            request.Locale = Locale.TR.ToString();
            request.ConversationId = orderId;
            request.Price = totalPrice.ToString();
            request.PaidPrice = totalPrice.ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = checkoutModel_Req.Installment;
            request.BasketId = orderId;
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "http://192.168.1.100:2176/api/checkout/callback";

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = checkoutModel_Req.FullName;
            paymentCard.CardNumber = checkoutModel_Req.CardNumber;
            paymentCard.ExpireMonth = checkoutModel_Req.Mounth;
            paymentCard.ExpireYear = "20" + checkoutModel_Req.Year;
            paymentCard.Cvc = checkoutModel_Req.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = appUser.Id;
            buyer.Name = appUser.FirstName;
            buyer.Surname = appUser.LastName;
            buyer.GsmNumber = appUser.UserName;
            buyer.Email = appUser.Email;
            buyer.IdentityNumber = appUser.Tckn;
            buyer.RegistrationAddress = checkoutModel_Req.Address;
            buyer.City = checkoutModel_Req.City;
            buyer.Country = checkoutModel_Req.District;
            buyer.Ip = ip;
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = appUser.FirstName + " " + appUser.LastName;
            shippingAddress.City = userAddress.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = userAddress.Adress;
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = appUser.FirstName + " " + appUser.LastName;
            billingAddress.City = userAddress.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = userAddress.Adress;
            request.BillingAddress = billingAddress;

            ThreedsInitialize threedsInitialize = ThreedsInitialize.Create(request, options);

            return threedsInitialize;

        }
    }
}
