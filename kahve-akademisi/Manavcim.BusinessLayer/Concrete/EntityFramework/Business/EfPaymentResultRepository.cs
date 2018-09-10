using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework.Business
{
    public class EfPaymentResultRepository : EfGenericRepository<PaymentResult>, IPaymentResultRepository
    {
        public EfPaymentResultRepository(DbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
        }
    }
}
