using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VenueBookingSystem.Data
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);  // 通过ID获取实体
        IEnumerable<T> GetAll();  // 获取所有实体
        IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate);  // 根据条件查找实体
        void Add(T entity);  // 添加实体
        void Update(T entity);  // 更新实体
        void Delete(T entity);  // 删除实体
    }
}
