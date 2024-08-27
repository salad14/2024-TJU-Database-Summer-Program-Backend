using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VenueBookingSystem.Data
{
    public interface IRepository<T> where T : class
    {
        T GetById(object id);  // 通过ID获取实体，支持不同类型的主键
        IEnumerable<T> GetAll();  // 获取所有实体
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);  // 根据条件查找实体
        void Add(T entity);  // 添加实体
        void Update(T entity);  // 更新实体
        void Delete(T entity);  // 删除实体
    }
}
