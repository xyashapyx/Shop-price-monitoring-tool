using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Models;
using PriceMonitoring.Models.DataModel;

namespace PriceMonitoring.Repository
{
    public class PriceRepository: IPriceRepository
    {
        private readonly PriceMonitoringDBContext _priceMonitoringDbContext;

        public PriceRepository(PriceMonitoringDBContext priceMonitoringDbContext)
        {
            _priceMonitoringDbContext = priceMonitoringDbContext;
        }

        public IEnumerable<Price> GetAll()
        {
            return _priceMonitoringDbContext.Prices.ToList();
        }

        public Price Get(int id)
        {
            return _priceMonitoringDbContext.Prices.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Price entity)
        {
            _priceMonitoringDbContext.Prices.Add(entity);
        }

        public void AddRange(IEnumerable<Price> entities)
        {
            _priceMonitoringDbContext.Prices.AddRange(entities);
        }

        public void Delete(Price entity)
        {
            _priceMonitoringDbContext.Prices.Remove(entity);
        }

        public void Clear()
        {
            _priceMonitoringDbContext.Prices.RemoveRange(GetAll());
        }

        public void Modify(int id, Price entity)
        {
            var price = Get(id);
            entity.Id = price.Id;
            _priceMonitoringDbContext.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Price> FindSet(Func<Price, bool> predicate)
        {
            return _priceMonitoringDbContext.Prices.Where(predicate);
        }

        public void Save()
        {
            _priceMonitoringDbContext.SaveChanges();
        }
    }
}