using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Models;
using PriceMonitoring.Models.DataModel;

namespace PriceMonitoring.Repository
{
    public class ProductRepository: IProductRepository
    {
        private readonly PriceMonitoringDBContext _priceMonitoringDbContext;

        public ProductRepository(PriceMonitoringDBContext priceMonitoringDbContext)
        {
            _priceMonitoringDbContext = priceMonitoringDbContext;
        }

        public IEnumerable<Product> GetAll()
        {
            return _priceMonitoringDbContext.Products.ToList();
        }

        public Product Get(int id)
        {
            return _priceMonitoringDbContext.Products.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Product entity)
        {
            _priceMonitoringDbContext.Products.Add(entity);
        }

        public void AddRange(IEnumerable<Product> entities)
        {
            _priceMonitoringDbContext.Products.AddRange(entities);
        }

        public void Delete(Product entity)
        {
            _priceMonitoringDbContext.Products.Remove(entity);
        }

        public void Clear()
        {
            _priceMonitoringDbContext.Products.RemoveRange(GetAll());
        }

        public void Modify(int id, Product entity)
        {
            var product = Get(id);
            entity.Id = product.Id;
            _priceMonitoringDbContext.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Product> FindSet(Func<Product, bool> predicate)
        {
            return _priceMonitoringDbContext.Products.Where(predicate);
        }

        public void Save()
        {
            _priceMonitoringDbContext.SaveChanges();
        }
    }
}