using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Abstract
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        OperationResult AddRange(List<Cart> carts);
        OperationResult GetMyCarts(string userName);
        OperationResult GetMyCartsWithUserId(string userId);
    }
}
