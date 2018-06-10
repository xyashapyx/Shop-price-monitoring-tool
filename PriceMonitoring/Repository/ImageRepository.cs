using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Models;
using PriceMonitoring.Models.DataModel;

namespace PriceMonitoring.Repository
{
    public class ImageRepository: IImageRepository
    {
        private readonly PriceMonitoringDBContext _priceMonitoringDbContext;

        public ImageRepository(PriceMonitoringDBContext priceMonitoringDbContext)
        {
            _priceMonitoringDbContext = priceMonitoringDbContext;
        }

        public IEnumerable<Image> GetAll()
        {
            return _priceMonitoringDbContext.Images.ToList();
        }

        public Image Get(int id)
        {
            return _priceMonitoringDbContext.Images.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Image entity)
        {
            _priceMonitoringDbContext.Images.Add(entity);
        }

        public void AddRange(IEnumerable<Image> entities)
        {
            _priceMonitoringDbContext.Images.AddRange(entities);
        }

        public void Delete(Image entity)
        {
            _priceMonitoringDbContext.Images.Remove(entity);
        }

        public void Clear()
        {
            _priceMonitoringDbContext.Images.RemoveRange(GetAll());
        }

        public void Modify(int id, Image entity)
        {
            var image = Get(id);
            entity.Id = image.Id;
            _priceMonitoringDbContext.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Image> FindSet(Func<Image, bool> predicate)
        {
            return _priceMonitoringDbContext.Images.Where(predicate);
        }

        public void Save()
        {
            _priceMonitoringDbContext.SaveChanges();
        }
    }
}