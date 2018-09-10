using KahveAkademisi.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KahveAkademisi.BusinessLayer.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        OperationResult Add(T t);
        Task<OperationResult> AddAsynAsync(T t);
        int Count();
        Task<int> CountAsync();
        OperationResult Delete(T entity);
        Task<OperationResult> DeleteAsyn(T entity);
        void Dispose();
        OperationResult Find(Expression<Func<T, bool>> match);
        OperationResult FindAll(Expression<Func<T, bool>> match);
        Task<OperationResult> FindAllAsync(Expression<Func<T, bool>> match);
        Task<OperationResult> FindAsync(Expression<Func<T, bool>> match);
        OperationResult FindBy(Expression<Func<T, bool>> predicate);
        Task<OperationResult> FindByAsyn(Expression<Func<T, bool>> predicate);
        OperationResult Get(int id);
        OperationResult GetAll();
        Task<OperationResult> GetAllAsyn();
        OperationResult GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<OperationResult> GetAsync(int id);
        void Save();
        Task<int> SaveAsync();
        OperationResult Update(T t, object key);
        Task<OperationResult> UpdateAsyn(T t, object key);
    }
}
