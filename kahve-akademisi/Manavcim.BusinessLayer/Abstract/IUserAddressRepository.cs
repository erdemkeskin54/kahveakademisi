﻿using KahveAkademisi.Entities.DbModels;
using KahveAkademisi.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Abstract
{
    public interface IUserAddressRepository: IGenericRepository<UserAddress>
    {
        OperationResult GetUserAddress(int addressId);
    }
}
