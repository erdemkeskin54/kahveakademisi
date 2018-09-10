using KahveAkademisi.Entities.ResponseModels;
using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.DbModels
{
    public class Product
    {
        public int Id { get; set; }
        public ProductStatus ProductStatus{ get; set; }
        public string ProductName { get; set; }
        public string MainImage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Discount { get; set; }
        public double DiscountAmount { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountFinishDate { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }


        public List<ProductCategory> ProductCategories { get; set; } = null;
        public List<ProductImageGallery> ProductImageGalleries { get; set; } = null;
        public List<ProductAmountType> ProductAmountTypes { get; set; } = null;

        public string CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        public AppUser CreateUser { get; set; } = null;

        public string UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        public AppUser UpdateUser { get; set; } = null;



        public Product()
        {
            ProductCategories = new List<ProductCategory>();
            ProductImageGalleries = new List<ProductImageGallery>();
        }

        public ProductRES ProductDTtoRES()
        {
            ProductRES productRES = new ProductRES()
            {
                Id = Id,
                ProductStatus=ProductStatus,
                ProductName = ProductName,
                Categories = ProductCategories!=null?ProductCategories.Select(x => x.CategoryToRES()).ToList():null,
                CreateDate=CreateDate,
                CreateUser=CreateUser!=null?CreateUser.AppUserDTtoRES():null,
                Discount=Discount,
                DiscountAmount=DiscountAmount,
                DiscountFinishDate=DiscountFinishDate,
                DiscountStartDate=DiscountStartDate,
                MainImage=MainImage,
                ProductImageGalleries= ProductImageGalleries!=null?ProductImageGalleries.Select(x=>x.ProductImageGalleryDTtoRES()).ToList():null,
                UpdateDate=UpdateDate,
                UpdateUser= UpdateUser!=null?UpdateUser.AppUserDTtoRES():null,
                ProductAmountTypes=ProductAmountTypes.Select(x=>x.ProductAmountTypeDTtoRES()).ToList(),
                LongDescription=LongDescription,
                ShortDescription=ShortDescription,    
            };

            return productRES;
        }
    
        public ProductRES ProductDTtoRES2()
        {
            ProductRES productRES = new ProductRES()
            {
                Id = Id,
                ProductStatus = ProductStatus,
                ProductName = ProductName,
                Categories = ProductCategories != null ? ProductCategories.Select(x => x.CategoryToRES()).ToList() : null,
                CreateDate = CreateDate,
                CreateUser = CreateUser != null ? CreateUser.AppUserDTtoRES() : null,
                Discount = Discount,
                DiscountAmount = DiscountAmount,
                DiscountFinishDate = DiscountFinishDate,
                DiscountStartDate = DiscountStartDate,
                MainImage = MainImage,
                ProductImageGalleries = ProductImageGalleries != null ? ProductImageGalleries.Select(x => x.ProductImageGalleryDTtoRES()).ToList() : null,
                UpdateDate = UpdateDate,
                UpdateUser = UpdateUser != null ? UpdateUser.AppUserDTtoRES() : null,
                LongDescription = LongDescription,
                ShortDescription = ShortDescription,
            };

            return productRES;
        }
    }
}
