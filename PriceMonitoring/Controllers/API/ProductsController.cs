using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Inerfaces.Services;
using PriceMonitoring.Models;
using PriceMonitoring.Models.DataModel;
using PriceMonitoring.ViewModels;

namespace PriceMonitoring.Controllers.API
{
    public class ProductsController : ApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IInformationImportService _informationImportService;

        public ProductsController(IProductRepository productRepository,
            IImageRepository imageRepository,
            IPriceRepository priceRepository,
            IInformationImportService informationImportService)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _priceRepository = priceRepository;
            _informationImportService = informationImportService;
        }
        // GET: api/Products
        [OutputCache(Duration = 300)]
        public IEnumerable<ProductViewModel> GetProducts()
        {
            //_informationImportService.ImportProducts();
            var productViewModels = new List<ProductViewModel>();
            foreach (var product in _productRepository.GetAll().Take(10))
                productViewModels.Add(new ProductViewModel(_imageRepository, _priceRepository, product));
            return productViewModels;
        }

        //GET: api/Products/5
        public ProductViewModel GetProduct(int id)
        {
            var product = _productRepository.Get(id);
            if (product == null)
                return new ProductViewModel(_imageRepository, _priceRepository, new Product());

            var productViewModel = new ProductViewModel(_imageRepository, _priceRepository, product);
            return productViewModel;
        }
    }
}