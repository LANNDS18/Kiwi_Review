using System;
using System.Linq;
using System.Linq.Expressions;

namespace Kiwi_review.Interfaces.IUnitOfWork
{
    public interface IUnitOfWorkBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Creat(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}