using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Abstract
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IOrdersRepository Orders { get; }
        IUserAddressRepository UserAddress { get; }
        ICartRepository Carts { get; }
        IProductAmountTypeRepository ProductAmountTypes { get; }
        IOrderDeteilRepository OrderDetails { get; }
        IPaymentResultRepository PaymentResults { get; }

        int SaveChanges();
    }
}
