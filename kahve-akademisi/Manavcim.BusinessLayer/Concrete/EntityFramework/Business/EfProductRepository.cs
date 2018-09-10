using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Entities.ResponseModels;
using KahveAkademisi.Entities.ResponseModels.Client;
using KahveAkademisi.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework.Business
{
    public class EfProductRepository : EfGenericRepository<Product>, IProductRepository
    {
        public EfProductRepository(KahveAkademisiContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public KahveAkademisiContext manavcimContext
        {
            get { return _context as KahveAkademisiContext; }
        }

        public OperationResult GetAllProducts()
        {
            try
            {
                IQueryable<Product> productsQueryble = manavcimContext.Products.Where(x => x.ProductStatus == ProductStatus.Enabled || x.ProductStatus == ProductStatus.InRequest).Take(5)
                    .Include(x => x.ProductCategories).ThenInclude(x => x.Category)
                    .Include(x => x.ProductImageGalleries).ThenInclude(x => x.CreateUser)
                    .Include(x => x.ProductAmountTypes)
                    .Include(x => x.CreateUser)
                    .Include(x => x.UpdateUser);

                List<Product> products = productsQueryble.ToList();
                return OperationResult.Success(products);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAllProducts)} : {ex}");
                return OperationResult.Error(ex);
            }
        }
        public OperationResult GetProductDetail(int id)
        {
            try
            {
                IQueryable<Product> productQueryble = manavcimContext.Products.Where(x => x.Id == id).Take(5)
                    .Include(x => x.ProductCategories).ThenInclude(x => x.Category)
                    .Include(x => x.ProductImageGalleries).ThenInclude(x => x.CreateUser)
                    .Include(x => x.ProductAmountTypes)
                    .Include(x => x.CreateUser)
                    .Include(x => x.UpdateUser);


                Product product = productQueryble.FirstOrDefault();
                return OperationResult.Success(product);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetProductDetail)} : {ex}");
                return OperationResult.Error(ex);
            }
        }

        public List<ProductRES> GetTop5Products()
        {
            return manavcimContext.Products
                 .OrderByDescending(i => i.Id)
                 .Take(5).Include(x => x.ProductCategories).ThenInclude(x => x.Category).Include(x => x.CreateUser).Include(x => x.ProductImageGalleries).Select(x => x.ProductDTtoRES()).ToList();
        }
    }
}
