using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.Entities.ResponseModels.Client
{
    public class ProductImageGalleryRES
    {
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public AppUserRES CreateUser { get; set; }
        public AppUserRES UpdateUser { get; set; }
        
    }
}
