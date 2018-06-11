using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using PriceMonitoring.Helpers;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Models.DataModel;

namespace PriceMonitoring.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(IImageRepository imageRepository, 
            IPriceRepository priceRepository, 
            Product product)
        {
            Product = product;
            loadProductImages(imageRepository);
            loadProductPrices(priceRepository);
        }

        public Product Product { get; }
        public List<string> Images { get; } = new List<string>();
        public string Prices { get; set; }

        private void loadProductImages(IImageRepository imageRepository)
        {
            var productImages = imageRepository.FindSet(x => x.ProductId == Product.Id);
            foreach (var productImage in productImages)
                Images.Add(ByteArrayToBase64Convertor.ConvertToBase64(productImage.ImageSource));
        }

        private void loadProductPrices(IPriceRepository priceRepository)
        {
            var productPrices = priceRepository.FindSet(x => x.ProductId == Product.Id).ToList();
            if (productPrices.Last().RegistrationDate.Date < DateTime.Today)
                productPrices.Add(new Price
                {
                    ProductId = Product.Id,
                    RegistrationDate = DateTime.Today,
                    ProductPrice = productPrices.Last().ProductPrice
                });

            var prices = new List<ChartModel>();
            foreach (var productPrice in productPrices)
            {
                prices.Add(new ChartModel
                {
                    X = productPrice.RegistrationDate.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds,
                    Y = (double)productPrice.ProductPrice
                });
            }

            Prices = JsonConvert.SerializeObject(prices);
        }

        [DataContract]
        public class ChartModel
        {
            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "x")]
            public double? X = null;

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public double? Y = null;
        }
    }
}