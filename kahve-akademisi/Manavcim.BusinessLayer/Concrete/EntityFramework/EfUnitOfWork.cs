using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.BusinessLayer.Concrete.EntityFramework.Business;
using Microsoft.Extensions.Logging;
using System;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework
{
    public class EfUnitOfWork : IUnitOfWork
    {

        private readonly KahveAkademisiContext dbContext;
        private ILoggerFactory loggerFactory;
        public EfUnitOfWork(KahveAkademisiContext _dbContext, ILoggerFactory _loggerFactory)
        {
            dbContext = _dbContext ?? throw new ArgumentNullException("dbcontext can not be null");
            loggerFactory = _loggerFactory;
        }

        private IProductRepository _products;
        private ICategoryRepository _categories;
        private IOrdersRepository _orders;
        private IUserAddressRepository _userAdress;
        private ICartRepository _carts;
        private IProductAmountTypeRepository _productAmountTypes;
        private IOrderDeteilRepository _orderDetails;
        private IPaymentResultRepository _paymentResults;
     


        public IProductRepository Products
        {
            get
            {
                return _products ?? (_products = new EfProductRepository(dbContext,loggerFactory));
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                return _categories ?? (_categories = new EfCategoryRepository(dbContext,loggerFactory));
            }
        }


        public IOrdersRepository Orders
        {
            get
            {
                return _orders ?? (_orders = new EfOrdersRepository(dbContext, loggerFactory));
            }
        }

        public IUserAddressRepository UserAddress
        {
            get
            {
                return _userAdress ?? (_userAdress = new EfUserAddressRepository(dbContext, loggerFactory));
            }
        }

        public ICartRepository Carts
        {
            get
            {
                return _carts ?? (_carts = new EfCartRepository(dbContext, loggerFactory));
            }
        }

        public IProductAmountTypeRepository ProductAmountTypes
        {
            get
            {
                return _productAmountTypes ?? (_productAmountTypes = new EfProductAmountTypeRepository(dbContext, loggerFactory));
            }
        }

        public IOrderDeteilRepository OrderDetails
        {
            get
            {
                return _orderDetails ?? (_orderDetails = new EfOrderDetailRepository(dbContext, loggerFactory));
            }
        }

        public IPaymentResultRepository PaymentResults
        {
            get
            {
                return _paymentResults ?? (_paymentResults = new EfPaymentResultRepository(dbContext, loggerFactory));
            }
        }
        
        public int SaveChanges()
        {
            try
            {
                return dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
