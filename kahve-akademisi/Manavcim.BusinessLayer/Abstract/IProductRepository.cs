using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Entities.ResponseModels;
using KahveAkademisi.Entities.ResponseModels.Client;
using KahveAkademisi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Abstract
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        List<ProductRES> GetTop5Products();
        OperationResult GetAllProducts();
        OperationResult GetProductDetail(int id);
    }
}
