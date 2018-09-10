using KahveAkademisi.BusinessLayer.Abstract;
using KahveAkademisi.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework
{
    public class EfGenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly ILogger _logger;

        public EfGenericRepository(DbContext context,
                                   ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("EfGenericRepository");
        }

        public OperationResult Add(T t)
        {
            try
            {
               var result=  _context.Set<T>().Add(t);
                _context.SaveChanges();
                return OperationResult.Success(result.Entity);
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public async Task<OperationResult> AddAsynAsync(T t)
        {
            try
            {
                _context.Set<T>().Add(t);
                await _context.SaveChangesAsync();
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public OperationResult Delete(T entity)
        {
            try
            {
                var t = _context.Set<T>().Remove(entity);
                if (t == null)
                    return OperationResult.Error("Bu nesne bulunamadı");

                _context.SaveChanges();
                return OperationResult.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(Delete)}" + " " + ex.Message);
                return OperationResult.Error(ex);
            }
        }

        public async Task<OperationResult> DeleteAsyn(T entity)
        {
            try
            {
                var t = _context.Set<T>().Remove(entity);
                if (t == null)
                    return OperationResult.Error("Bu nesne bulunamadı");

                await _context.SaveChangesAsync();
                return OperationResult.Success(entity);
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }


        public OperationResult Find(Expression<Func<T, bool>> match)
        {
            try
            {
                return OperationResult.Success(_context.Set<T>().SingleOrDefault(match));
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }

        }

        public OperationResult FindAll(Expression<Func<T, bool>> match)
        {
            try
            {
                return OperationResult.Success(_context.Set<T>().Where(match).ToList());
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public async Task<OperationResult> FindAllAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                return OperationResult.Success(await _context.Set<T>().Where(match).ToListAsync());
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public async Task<OperationResult> FindAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                return OperationResult.Success(await _context.Set<T>().SingleOrDefaultAsync(match));
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public OperationResult FindBy(Expression<Func<T, bool>> predicate)
        {

            try
            {
                IQueryable<T> query = _context.Set<T>().Where(predicate);
                return OperationResult.Success(query);
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }

        }

        public async Task<OperationResult> FindByAsyn(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return OperationResult.Success(await _context.Set<T>().Where(predicate).ToListAsync());
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }

        }

        public OperationResult Get(int id)
        {
            try
            {
                return OperationResult.Success(_context.Set<T>().Find(id));
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public OperationResult GetAll()
        {
            try
            {
                return OperationResult.Success(_context.Set<T>().ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAll)}" + " " +ex.Message);

                return OperationResult.Error(ex);
            }
        }

        public virtual async Task<OperationResult> GetAllAsyn()
        {
            //HATA VAR
            try
            {
                return OperationResult.Success(await _context.Set<T>().ToListAsync());
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public OperationResult GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            try
            {
                IQueryable<T> queryable = (IQueryable<T>)GetAll();
                foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                {

                    queryable = queryable.Include<T, object>(includeProperty);
                }

                return OperationResult.Success(queryable);
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public async Task<OperationResult> GetAsync(int id)
        {
            try
            {
                return OperationResult.Success(await _context.Set<T>().FindAsync(id));
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public OperationResult Update(T t, object key)
        {
            try
            {
                if (t == null)
                    return null;
                T exist = _context.Set<T>().Find(key);
                if (exist != null)
                {                   
                    
                    _context.Entry(exist).CurrentValues.SetValues(t);
                    _context.SaveChanges();
                }

                return OperationResult.Success(exist);
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }

        public async Task<OperationResult> UpdateAsyn(T t, object key)
        {
            try
            {
                if (t == null)
                    return null;
                T exist = await _context.Set<T>().FindAsync(key);
                if (exist != null)
                {
                    _context.Entry(exist).CurrentValues.SetValues(t);
                    await _context.SaveChangesAsync();
                }

                return OperationResult.Success(exist);
            }
            catch (Exception ex)
            {
                return OperationResult.Error(ex);
            }
        }



        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
