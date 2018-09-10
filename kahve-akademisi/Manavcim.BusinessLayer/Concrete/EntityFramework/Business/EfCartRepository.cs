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
    public class EfCartRepository : EfGenericRepository<Cart>, ICartRepository
    {
        public EfCartRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public KahveAkademisiContext manavcimContext
        {
            get { return _context as KahveAkademisiContext; }
        }

        public OperationResult AddRange(List<Cart> carts)
        {
            try
            {
                manavcimContext.Carts.AddRange(carts);
                manavcimContext.SaveChanges();

                return OperationResult.Success(carts);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error in {nameof(AddRange)} : {ex}");

                return OperationResult.Error(ex);
            }
        }

        public OperationResult GetMyCarts(string userName)
        {
            try
            {
                IQueryable<Cart> cartsIquery = manavcimContext.Carts.Where(x => x.User.UserName == userName)
                    .Include(x => x.ProductAmountType).ThenInclude(x => x.Product).ThenInclude(x => x.ProductCategories).ThenInclude(x=>x.Category);
                List<Cart> carts = cartsIquery.ToList();

                return OperationResult.Success(carts);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error in {nameof(GetMyCarts)} : {ex}");

                return OperationResult.Error(ex);
            }
        }
        public OperationResult GetMyCartsWithUserId(string userId)
        {
            try
            {
                IQueryable<Cart> cartsIquery = manavcimContext.Carts.Where(x => x.User.Id == userId)
                    .Include(x => x.ProductAmountType).ThenInclude(x => x.Product).ThenInclude(x => x.ProductCategories).ThenInclude(x => x.Category);
                List<Cart> carts = cartsIquery.ToList();

                return OperationResult.Success(carts);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error in {nameof(GetMyCarts)} : {ex}");

                return OperationResult.Error(ex);
            }
        }
    }
}
