using KahveAkademisi.Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.ResponseModels.Client
{
    public class ProductRES
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string MainImage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Discount { get; set; }
        public double DiscountAmount { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountFinishDate { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public AppUserRES CreateUser { get; set; }
        public AppUserRES UpdateUser { get; set; }


        public List<CategoryRES> Categories { get; set; }
        public List<ProductImageGalleryRES> ProductImageGalleries { get; set; }
        public List<ProductAmountTypeRES> ProductAmountTypes { get; set; }

    }
}
