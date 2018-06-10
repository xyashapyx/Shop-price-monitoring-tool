using System.Data.Entity;
using PriceMonitoring.Models.DataModel;

namespace PriceMonitoring.Models
{
    public class PriceMonitoringDBContext : DbContext
    {
        public PriceMonitoringDBContext() : base("PriceMonitoringDB")
        {
            //Disable initializer
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Price> Prices { get; set; }
    }
}