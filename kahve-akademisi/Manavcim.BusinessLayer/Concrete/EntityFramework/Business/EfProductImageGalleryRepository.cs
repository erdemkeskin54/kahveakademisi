using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework.Business
{
    public class EfProductImageGalleryRepository : EfGenericRepository<ProductImageGallery>, IProductImageGalleryRepository
    {
        public EfProductImageGalleryRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }

        public KahveAkademisiContext manavcimContext
        {
            get { return _context as KahveAkademisiContext; }
        }

    }
}
