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
    public class EfUserAddressRepository : EfGenericRepository<UserAddress>, IUserAddressRepository
    {
        public EfUserAddressRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {

        }

        public KahveAkademisiContext manavcimContext
        {
            get { return _context as KahveAkademisiContext; }
        }

        public OperationResult GetUserAddress(int addressId)
        {
            try
            {
                IQueryable<UserAddress> addressQueryble = manavcimContext.UserAdresses.Where(x => x.Id == addressId);

                UserAddress userAddress = addressQueryble.FirstOrDefault();
                return OperationResult.Success(userAddress);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetUserAddress)} : {ex}");
                return OperationResult.Error(ex);
            }
        }
    }
}
