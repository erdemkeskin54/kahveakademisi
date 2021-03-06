﻿using KahveAkademisi.Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Abstract
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Category GetByName(string name);
    }
}
