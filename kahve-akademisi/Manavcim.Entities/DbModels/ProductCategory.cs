using KahveAkademisi.Entities.ResponseModels;
using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KahveAkademisi.Entities.DbModels
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        public Category Category { get; set; } = null;
        public Product Product { get; set; } = null;

        public CategoryRES CategoryToRES()
        {
            CategoryRES categoryRES = new CategoryRES()
            {
                Id = Category.Id,
                CategoryName = Category.CategoryName
            };
            return categoryRES;
        }

    }
}
