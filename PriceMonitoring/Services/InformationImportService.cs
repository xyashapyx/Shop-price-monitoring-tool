using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Inerfaces.Services;
using PriceMonitoring.Models.DataModel;
using PriceMonitoring.Models.JsonModel;
using Image = PriceMonitoring.Models.DataModel.Image;
using Product = PriceMonitoring.Models.JsonModel.Product;

namespace PriceMonitoring.Services
{
    public class InformationImportService: IInformationImportService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IPriceRepository _priceRepository;
        private const string _url = "https://api.eldorado.ua/v1/goods?get_catalog_goods&ids=1111111,1346629,1240198,1299929,1332705,1333031,1284321,1332653,1334909,1339503,1220875,1226268,1313593,1302161,1329109,1345053,1271338,1302847,1303767,1316163,1298693,1315429,1330163,1332479,1313219,1316165,1223382,1316611,1315343,1331959,1294577,1330299,1339421,1298563,1218564,1250720,1290768,1235736,1271324,1313215,1263090,1310357,1258784,1332701,1302881,1313015,1290948,1258180,1315457,1235740,1258770,1272620,1307049,1315449,1315459,1274280,1286390,1302481,1219522,1315453,1339679,1276722,1314553,1305095,1292740,1312333,1232300,1331077,1261262,1315957,1298743,1334083,1271398,1295699,1247294,1256098,1261290,1294533,1222922,1246242,1269428,1276524,1290398,1258794,1270236,1286566,1309397,1224316,1240226,1251662,1277348,1281914,1284972,1286434,1293122,1294535";
        private const string _rootUrl = "https://i.eldorado.ua/";

        public InformationImportService(IProductRepository productRepository,
            IImageRepository imageRepository,
            IPriceRepository priceRepository)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _priceRepository = priceRepository;
        }

        public async Task ImportProducts()
        {
            var products = await getProducts();
            saveProducts(products);
            savePrices(products);
        }

        private void saveProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                if (_productRepository.Get(product.Id) != null)
                    continue;
                _productRepository.Add(new Models.DataModel.Product
                {
                    AddingDate = product.CreatedAt,
                    Description = product.Description,
                    Id = product.Id,
                    IsActive = product.IsActive,
                    Name = product.Name,
                    Title = product.Title
                });
                 saveProductImages(product.Images, product.Id);
            }            
            _productRepository.Save();
            _imageRepository.Save();
        }

        private void saveProductImages(IEnumerable<Models.JsonModel.Image> images, int productId)
        {
            foreach (var image in images)
            {
                _imageRepository.Add(new Image
                {
                    ProductId = productId,
                    ImageSource = readImageBytes(image.Href)
                });
            }
        }

        private void savePrices(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                var prices = _priceRepository.FindSet(x => x.ProductId == product.Id);
                if (!prices.Any() || prices.Last().ProductPrice != product.Price)
                    _priceRepository.Add(new Price
                    {
                        ProductId = product.Id,
                        ProductPrice = product.Price,
                        RegistrationDate = DateTime.Now
                    });
            }
            _priceRepository.Save();
        }

        private async Task<IEnumerable<Product>> getProducts()
        {
            var jsonString = await getJsonFromUrl(_url);
            return JsonConvert.DeserializeObject<Json>(jsonString).Data;
        }

        private async Task<string> getJsonFromUrl(string url)
        {
            string jsonResponse = string.Empty;
            ServicePointManager
                    .ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                jsonResponse = await reader.ReadToEndAsync();
            }
            return jsonResponse;
        }

        private byte[] readImageBytes(string imageName)
        {
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(_rootUrl + imageName);
            return imageBytes;
        }
    }
}