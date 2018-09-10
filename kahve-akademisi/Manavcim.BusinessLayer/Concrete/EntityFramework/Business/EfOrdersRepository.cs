using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework.Business
{
    public class EfOrdersRepository : EfGenericRepository<Order>, IOrdersRepository
    {
        public EfOrdersRepository(KahveAkademisiContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {

        }

        public KahveAkademisiContext manavcimContext
        {
            get { return _context as KahveAkademisiContext; }
        }

        public OperationResult GetUserOrders(string userName,int pageNumber,int pageSize)
        {
            try
            {
                IQueryable<Order> order = manavcimContext.Orders.Where(x => x.AppUser.UserName == userName).Skip((pageNumber-1)*pageSize).Take(pageSize)
                    .Include(x => x.OrderDetails).ThenInclude(x => x.ProductAmountType).ThenInclude(x => x.Product)
                    .Include(x => x.UserAdress);
                List<Order> orders = order.ToList();

                return OperationResult.Success(orders);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error in {nameof(GetUserOrders)} : {ex}");

                return OperationResult.Error(ex);
            }
        }

        public OperationResult GetUserOrdersCount(string userName)
        {
            try
            {
                IQueryable<Order> orders = manavcimContext.Orders.Where(x => x.AppUser.UserName == userName);
                int ordersCount = orders.Count();

                return OperationResult.Success(ordersCount);
            }
            catch (Exception ex)
            {


                _logger.LogError($"Error in {nameof(GetUserOrdersCount)} : {ex}");
                return OperationResult.Error(ex);
            }
        }
    }
}
