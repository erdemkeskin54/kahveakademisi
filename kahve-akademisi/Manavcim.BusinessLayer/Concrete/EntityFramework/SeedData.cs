using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Entities.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework
{
    public class SeedData
    {

        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        private List<Product> products;
        private List<Category> categories;
        private List<ProductCategory> productCategories;
        private List<AppUser> appUsers;
        private List<UserAddress> userAdresses;

        double[] kglar = new double[] { 0.5, 1, 1, 5, 2, 2.5, 3, 3.5, 4, 4.5, 5 };
        double[] adetler = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        double[] kasalar = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        double[] cuvallar = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int[] resimNolar = new int[] { 1, 2, 3 };
        List<ProductSize> ProductSizes = new List<ProductSize>();
        

        public SeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

            products = new List<Product>();
            categories = new List<Category>();
            productCategories = new List<ProductCategory>();
            appUsers = new List<AppUser>();
            userAdresses = new List<UserAddress>();
        }



        public async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {

            ProductSizes.Add(ProductSize.Small);
            ProductSizes.Add(ProductSize.Medium);
            ProductSizes.Add(ProductSize.Large);


            var context = app.ApplicationServices.GetRequiredService<KahveAkademisiContext>();
            context.Database.Migrate();

            if (!context.Products.Any())
            {

                await KullaniciEkle(context, 20);
                KullaniciyaAdresEkle(context, 3);

                await UrunleriEkle(context, 50);
                await UrunSatisTipiAsync(context);
                KategoriEkle(context, 20);
                UrunlereKategoriEkle(context, 2);
                await UruneGaleryEkleAsync(context, 3);
                SiparisEkle(context, 5, 3);
                await SepetEkleAsync(context, 10,3);
            }
        }


        private async Task KullaniciEkle(KahveAkademisiContext context, int adet)
        {

            AppUser user = new AppUser
            {
                FirstName = "Abdurrahman",
                LastName = "JO",
                UserName = "05533607714",
                Language = Language.Turkish,
                CreateDate = DateTime.Now,
                Email = FakeData.NetworkData.GetEmail(),
                Tckn = "29455801802"
                
            };

            var clientRole = new IdentityRole("Client");
            await _roleManager.CreateAsync(clientRole);
            var result = await _userManager.CreateAsync(user, "411811c");
            var roleResult = await _userManager.AddToRoleAsync(user, "Client");


            var user2 = await _userManager.FindByNameAsync("05379226233");
            if (user2 == null)
            {
                if (!(await _roleManager.RoleExistsAsync("Admin")))
                {
                    var adminRole = new IdentityRole("Admin");
                    await _roleManager.CreateAsync(adminRole);


                    await _roleManager.AddClaimAsync(adminRole, new Claim("IsAdmin", "true"));

                }

                user = new AppUser()
                {
                    UserName = "05379226233",
                    FirstName = "Mert",
                    LastName = "İĞDİR",
                    Language = Language.English,
                    Email = "admin@admin.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.Now,
                    Tckn = "29455801802"
                };

                var userResult = await _userManager.CreateAsync(user, "P@ssw0rd!");

                var roleResult2 = await _userManager.AddToRoleAsync(user, "Admin");

                var clamResult = await _userManager.AddClaimAsync(user, new Claim("SuperUser", "true"));

                if (!userResult.Succeeded || !roleResult.Succeeded || !clamResult.Succeeded)
                {
                    throw new InvalidOperationException("kullanıcı yaratmada sorun var!");
                }

            }



            for (int i = 0; i < adet; i++)
            {
                AppUser user3 = new AppUser
                {
                    FirstName = FakeData.NameData.GetFirstName(),
                    LastName = FakeData.NameData.GetSurname(),
                    UserName = FakeData.PhoneNumberData.GetPhoneNumber().DeleteTre(),
                    Language = Language.Turkish,
                    CreateDate = DateTime.Now
                };

                var result3 = await _userManager.CreateAsync(user3, "411811c");
                var roleResult3 = await _userManager.AddToRoleAsync(user3, "Client");
            }

            appUsers = context.Users.Where(x => x.UserName != "05379226233").ToList();

        }
        private async Task UrunleriEkle(KahveAkademisiContext context, int adet)
        {

            AppUser admin = await _userManager.FindByNameAsync("05379226233");

            for (int i = 0; i < adet; i++)
            {
                bool indirim = FakeData.BooleanData.GetBoolean();
                DateTime dateTime = FakeData.DateTimeData.GetDatetime(DateTime.Now, new DateTime(2018, 10, 10, 10, 10, 10));

                Product product = new Product()
                {
                    CreateDate = FakeData.DateTimeData.GetDatetime(DateTime.Now, new DateTime(2018, 10, 10, 10, 10, 10)),
                    CreateUserId = admin.Id,
                    Discount = indirim,
                    DiscountAmount = indirim == false ? 0 : FakeData.NumberData.GetNumber(1, 99),
                    DiscountFinishDate = FakeData.DateTimeData.GetDatetime(DateTime.Now, new DateTime(2018, 10, 10, 10, 10, 10)),
                    DiscountStartDate = FakeData.DateTimeData.GetDatetime(DateTime.Now, dateTime),              
                    MainImage = "http://192.168.1.100:2176/images/product-image-"+ resimNolar[FakeData.NumberData.GetNumber(0, resimNolar.Length - 1)] + ".jpg",
                    ProductName = FakeData.NameData.GetFirstName(),
                    UpdateDate = DateTime.Now,
                    UpdateUserId = admin.Id,
                    ProductStatus =i%2==0 ? ProductStatus.Enabled : ProductStatus.InRequest,
                    LongDescription=FakeData.TextData.GetSentences(5),
                    ShortDescription=FakeData.TextData.GetSentences(1),
                   
                };

                context.Products.Add(product);
            }
            context.SaveChanges();
            products = context.Products.Include(x => x.ProductAmountTypes).ToList();

        }
        private async Task UrunSatisTipiAsync(KahveAkademisiContext context)
        {
            AppUser admin = await _userManager.FindByNameAsync("05379226233");

            for (int i = 0; i < products.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double adet=0;
                    AmountType amountType = new AmountType();
                    switch (j+1)
                    {
                        case 1:
                            amountType = AmountType.Piece;
                            adet= adetler[FakeData.NumberData.GetNumber(0, adetler.Count() - 1)];                           
                            break;
                        case 2:
                            amountType = AmountType.Weight;
                            adet = kglar[FakeData.NumberData.GetNumber(0, kglar.Count() - 1)];
                            break;
                        case 3:
                            amountType = AmountType.Box;
                            adet = kglar[FakeData.NumberData.GetNumber(0, kasalar.Count() - 1)];
                            break;
                    }

                    ProductAmountType productAmountType = new ProductAmountType()
                    {
                        AmountType = amountType,
                        CreateUserId = admin.Id,
                        Product = products[i],
                        UpdateUserId = admin.Id,
                        Price = FakeData.NumberData.GetNumber(1, 15),
                        Choice=amountType==AmountType.Piece ? true : false,
                        Weight=FakeData.NumberData.GetNumber(1,20),
                        Stock = FakeData.NumberData.GetNumber(5, 100)
                    };

                    context.ProductAmountTypes.Add(productAmountType);
                }
            }

            context.SaveChanges();
            products = context.Products.Include(x => x.ProductAmountTypes).ToList();

        }
        private void KategoriEkle(KahveAkademisiContext context, int adet)
        {
            for (int i = 0; i < adet; i++)
            {

                Category category = new Category()
                {
                    CategoryName = FakeData.NameData.GetFirstName()
                };

                categories.Add(category);
            }


            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
        private void UrunlereKategoriEkle(KahveAkademisiContext context, int kacKategori)
        {
            for (int i = 0; i < products.Count; i++)
            {
                for (int j = 0; j < kacKategori; j++)
                {
                    ProductCategory productCategory = new ProductCategory()
                    {
                        Product = products[i],
                        Category = categories[FakeData.NumberData.GetNumber(0, categories.Count - 1)]
                    };
                    productCategories.Add(productCategory);
                }

            }


            context.ProductCategory.AddRange(productCategories);
            context.SaveChanges();
        }
        private async Task UruneGaleryEkleAsync(KahveAkademisiContext context, int kacResim)
        {
            AppUser admin = await _userManager.FindByNameAsync("05379226233");

            for (int i = 0; i < products.Count; i++)
            {
                for (int j = 0; j < kacResim; j++)
                {
                    ProductImageGallery productImageGallery = new ProductImageGallery()
                    {
                        Product = products[i],
                        CreateDate = DateTime.Now,
                        CreateUserId = admin.Id,
                        ImageTitle = FakeData.NameData.GetFullName(),
                        ImageUrl = j==0 ? products[i].MainImage : "http://192.168.1.100:2176/images/product-image-gallery-" + j + ".jpg",
                        UpdateDate = DateTime.Now,
                        UpdateUserId = admin.Id
                    };

                    context.ProductImageGalleries.Add(productImageGallery);
                }
            }

            context.SaveChanges();

        }
        private void KullaniciyaAdresEkle(KahveAkademisiContext context, int adet)
        {
            for (int i = 0; i < appUsers.Count; i++)
            {
                for (int j = 0; j < adet; j++)
                {
                    UserAddress userAdress = new UserAddress()
                    {
                        Adress = FakeData.PlaceData.GetAddress(),
                        AdressTitle = FakeData.NameData.GetFullName(),
                        UserId = appUsers[i].Id,
                        PhoneNumber = FakeData.PhoneNumberData.GetPhoneNumber(),
                        XAxis = FakeData.NumberData.GetNumber(10, 50).ToString(),
                        YAxis = FakeData.NumberData.GetNumber(10, 50).ToString(),
                        City="Sakarya",
                        District="Adapazarı",
                        Neighborhood="Yukarı Kirazca",
                        Zip="54100"
                    };

                    context.UserAdresses.Add(userAdress);
                }
            }

            context.SaveChanges();
            userAdresses = context.UserAdresses.ToList();
        }
        private void SiparisEkle(KahveAkademisiContext context, int adet, int kacUrun)
        {
            for (int i = 0; i < appUsers.Count; i++)
            {
                for (int t = 0; t < adet; t++)
                {

                    double genelToplam = 0;

                    Order order = new Order()
                    {
                        UserId = appUsers[i].Id,
                        OrderDate = DateTime.Now,
                        OrderStatus = t % 2 == 0 ? OrderStatus.Approved : OrderStatus.Canceled ,
                        UserAdress = userAdresses.Where(x => x.UserId == appUsers[i].Id).FirstOrDefault(),
                    };
           
                    for (int j = 0; j < kacUrun; j++)
                    {
                        int productIndex = FakeData.NumberData.GetNumber(0, products.Count - 1);                    
                        int quantity = FakeData.NumberData.GetNumber(1, 30);
                        ProductAmountType productAmountType = products[productIndex].ProductAmountTypes[FakeData.NumberData.GetNumber(0, products[productIndex].ProductAmountTypes.Count - 1)];
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            Order = order,
                            OrderStatus = j%2==0 ? OrderStatus.Active : OrderStatus.Canceled,
                            ProductAmountType= productAmountType,
                            Quantity = quantity,
                            TotalPrice = productAmountType.Price*quantity
                        };


                        genelToplam = orderDetail.TotalPrice + genelToplam;

                        order.OrderDetails.Add(orderDetail);
                    }

                    order.TotalPrice = genelToplam;

                    context.Orders.Add(order);

                }
            }

            context.SaveChanges();

        }
        private async Task SepetEkleAsync(KahveAkademisiContext context,int kullaniciAdet,int urunAdet)
        {
            Cart cart = null;
            Product product = null;
            AppUser appUser =await _userManager.FindByNameAsync("05533607714");

            
            for (int i = 0; i < urunAdet; i++)
            {
                product = products[FakeData.NumberData.GetNumber(0, products.Count - 1)];


                cart = new Cart() {
                    User = appUser,
                    AddToCartDate = DateTime.Now,
                    ProductAmountType = product.ProductAmountTypes[FakeData.NumberData.GetNumber(0, product.ProductAmountTypes.Count-1)],
                    Quantity=3,                
                };

                context.Carts.Add(cart);
            }

            context.SaveChanges();
        }

    }
}
