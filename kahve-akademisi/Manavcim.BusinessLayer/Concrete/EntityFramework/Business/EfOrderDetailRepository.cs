using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework.Business
{
    public class EfOrderDetailRepository : EfGenericRepository<OrderDetail>, IOrderDeteilRepository
    {
        public EfOrderDetailRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }


        public KahveAkademisiContext manavcimContext
        {
            get { return _context as KahveAkademisiContext; }
        }

        public OperationResult AddRange(List<OrderDetail> orderDetails)
        {
            try
            {
                manavcimContext.OrderDetails.AddRange(orderDetails);
                manavcimContext.SaveChanges();

                return OperationResult.Success(orderDetails);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error in {nameof(AddRange)} : {ex}");

                return OperationResult.Error(ex);
            }
        }


    }
}
