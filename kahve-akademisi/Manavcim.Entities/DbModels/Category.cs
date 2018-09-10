using KahveAkademisi.Entities.ResponseModels;
using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.Entities.DbModels
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public  List<ProductCategory> ProductCategories { get; set; }

        public CategoryRES CategoryDTtoRES()
        {
            CategoryRES categoryRES = new CategoryRES()
            {
                Id = Id,
                CategoryName=CategoryName,

            };

            return categoryRES;
        }
    }
}
