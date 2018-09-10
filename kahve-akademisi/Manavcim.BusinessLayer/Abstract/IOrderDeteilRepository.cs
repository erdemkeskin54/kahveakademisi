using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Abstract
{
    public interface IOrderDeteilRepository: IGenericRepository<OrderDetail>
    {
        OperationResult AddRange(List<OrderDetail> orderDetails);
    }
}
