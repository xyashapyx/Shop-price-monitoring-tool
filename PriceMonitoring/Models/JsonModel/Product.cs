using System;
using System.Collections.Generic;

namespace PriceMonitoring.Models.JsonModel
{
    public class Product
    {
        public int Id { get; set; }
        public List<Image> Images { get; set; }
        public DateTime CreatedAt => Created_at;
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Description => desc_small;
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Is_active
        {
            private get => string.Empty;
            set => IsActive = value == "1";
        }
        public DateTime Created_at { private get; set; }
        public string desc_small { private get; set; }
    }
}