using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Inerfaces.Services;
using PriceMonitoring.ViewModels;

namespace PriceMonitoring.Controllers
{
    public class ProductsController : Controller
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

        // GET: Products
        [OutputCache(Duration = 300)]
        public async Task<ActionResult> Index()
        {
            await _informationImportService.ImportProducts();
            var productViewModels = new List<ProductViewModel>();
            foreach (var product in _productRepository.GetAll().Take(10))
                productViewModels.Add(new ProductViewModel(_imageRepository, _priceRepository, product));
            return View(productViewModels);
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = _productRepository.Get((int)id);
            var productViewModel = new ProductViewModel(_imageRepository, _priceRepository, product);
            if (productViewModel == null)
            {
                return HttpNotFound();
            }
            return View(productViewModel);
        }  
    }
}
