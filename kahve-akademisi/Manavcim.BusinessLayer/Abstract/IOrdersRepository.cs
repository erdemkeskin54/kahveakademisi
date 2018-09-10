using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Abstract
{
    public interface IOrdersRepository: IGenericRepository<Order>
    {
        OperationResult GetUserOrders(string userName,int pageNumber,int pageSize);
        OperationResult GetUserOrdersCount(string userName);

    }
}
