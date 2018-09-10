using KahveAkademisi.Entities.ResponseModels;
using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KahveAkademisi.Entities.DbModels
{
    public class ProductImageGallery
    {
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        public AppUser CreateUser { get; set; } = null;

        public string UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        public AppUser UpdateUser { get; set; }
        public Product Product { get; set; } = null;

        public ProductImageGalleryRES ProductImageGalleryDTtoRES()
        {
            ProductImageGalleryRES productImageGalleryRES = new ProductImageGalleryRES()
            {
                Id = Id,
                CreateDate=CreateDate,
                CreateUser=CreateUser.AppUserDTtoRES(),
                ImageTitle=ImageTitle,
                ImageUrl=ImageUrl,
                UpdateDate=UpdateDate,
                UpdateUser=UpdateUser.AppUserDTtoRES(),
                
            };

            return productImageGalleryRES;
        }

    }
}
