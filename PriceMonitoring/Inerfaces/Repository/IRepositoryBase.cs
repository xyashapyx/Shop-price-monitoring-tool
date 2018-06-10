using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PriceMonitoring.Inerfaces.Repository
{
    public interface IRepositoryBase<TModel>
    {
        IEnumerable<TModel> GetAll();
        TModel Get(int id);
        void Add(TModel entity);
        void AddRange(IEnumerable<TModel> entities);
        void Delete(TModel entity);
        void Clear();
        void Modify(int id, TModel entity);
        IEnumerable<TModel> FindSet(Func<TModel, bool> predicate);
        void Save();
    }
}
