using System.Linq.Expressions;
using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models;
using Microsoft.EntityFrameworkCore;

namespace Kiwi_review.UnitOfWork
{
    public abstract class UnitOfWorkBase<T> : IUnitOfWorkBase<T> where T : class
    {
        protected KiwiReviewContext KiwiReviewContext { get; set; }

        public UnitOfWorkBase(KiwiReviewContext kiwiReviewContext)
        {
            this.KiwiReviewContext = kiwiReviewContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.KiwiReviewContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.KiwiReviewContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Creat(T entity)
        {
            this.KiwiReviewContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.KiwiReviewContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.KiwiReviewContext.Set<T>().Remove(entity);
        }
    }
}