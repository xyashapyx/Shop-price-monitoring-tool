using System.Collections.Generic;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Inerfaces.Services;
using PriceMonitoring.Models.DataModel;
using PriceMonitoring.Services;

namespace Tests.Services
{
    [TestFixture]
    public class InformationImportServiceTests
    {
        private readonly IPriceRepository _priceRepository = Substitute.For<IPriceRepository>();
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IImageRepository _imageRepository = Substitute.For<IImageRepository>();
        private readonly IDataReader _dataReader = Substitute.For<IDataReader>();
        private IInformationImportService _informationImportService;
        private const int _firstProductId = 1350351;
        private const string _jsonExample = "{\"data\":[{\"Id\":1350351,\"Images\":[{\"Href\":\"goods_images/1038946/6181753-1527864409.png\"},{\"Href\":\"goods_images/1038946/6181755-1527864409.png\"},{\"Href\":\"goods_images/1038946/6181757-1527864409.png\"},{\"Href\":\"goods_images/1038946/6181759-1527864410.png\"},{\"Href\":\"goods_images/1038946/6181761-1527864410.png\"}],\"CreatedAt\":\"2018-05-25T18:51:51\",\"IsActive\":true,\"Name\":\"smartfon-huawei-y7-2018-ldn-l21-blue\",\"Description\":\"Диагональ: 5.99'' / Разрешение дисплея: 1440х720 / Разрешение основной камеры: 13+2Мп / Разрешение фронтальной камеры: 8 Мп / Оперативная память: 3072Мб / Встроенная память: 32Гб / Поддержка 3G: да / Поддержка 4G (LTE): да / Встроенный GPS: да / Количество SIM-карт: 2 / Тип SIM-карты: nano-SIM / ОС: Android\",\"Price\":5999.00,\"Title\":\"Смартфон HUAWEI Y7 Prime 2018 3/32GB Blue (51092JHB)\"},{\"Id\":1111111,\"Images\":[{\"Href\":\"/goods/905/905924.jpg\"}],\"CreatedAt\":\"2014-02-15T01:17:16\",\"IsActive\":true,\"Name\":\"offer_1251985\",\"Description\":\"Совместимость с принтерами HP: Officejet 6213/7213/7313/7413/H470/H470b/H470wbt/K7103; Deskjet\t460c/460cb/460wbt/5743/6543/6543d/6623/6843/6843d/9803/9803d; Photosmart 2573/2613/2713/8153/8453/8753/C3183/Pro B8353; PSC 1513/1513s/1613/2353.\",\"Price\":950.00,\"Title\":\"Картридж HP 131 Black (C8765HE)\"},{\"Id\":1346629,\"Images\":[{\"Href\":\"goods_images/1038946/6173317-1526542720.jpg\"},{\"Href\":\"goods_images/1038946/6173319-1526542720.jpg\"},{\"Href\":\"goods_images/1038946/6173321-1526542720.jpg\"},{\"Href\":\"goods_images/1038946/6173323-1526542720.jpg\"},{\"Href\":\"goods_images/1038946/6173325-1526542720.jpg\"},{\"Href\":\"goods_images/1038946/6173329-1526542720.jpg\"},{\"Href\":\"goods_images/1038946/6173331-1526542720.jpg\"},{\"Href\":\"goods_images/1038946/6173337-1526542720.jpg\"},{\"Href\":\"goods_images/1038946/6173367-1526543013.png\"},{\"Href\":\"goods_images/1038946/6177361-1527055653.jpg\"},{\"Href\":\"goods_images/1038946/6177363-1527055930.jpg\"},{\"Href\":\"goods_images/1038946/6177365-1527056393.jpg\"},{\"Href\":\"goods_images/1038946/6177367-1527056539.jpg\"},{\"Href\":\"goods_images/1038946/6177369-1527056817.jpg\"},{\"Href\":\"goods_images/1038946/6177371-1527057191.jpg\"},{\"Href\":\"goods_images/1038946/6177373-1527057333.jpg\"},{\"Href\":\"goods_images/1038946/6177377-1527058398.png\"},{\"Href\":\"goods_images/1038946/6177379-1527058466.JPG\"},{\"Href\":\"goods_images/1038946/6177381-1527058557.jpg\"},{\"Href\":\"goods_images/1038946/6177383-1527058774.jpg\"},{\"Href\":\"goods_images/1038946/6177393-1527059485.png\"},{\"Href\":\"goods_images/1038946/6177395-1527059485.png\"},{\"Href\":\"goods_images/1038946/6177397-1527059485.png\"},{\"Href\":\"goods_images/1038946/6177399-1527059485.png\"},{\"Href\":\"goods_images/1038946/6177401-1527059485.png\"},{\"Href\":\"goods_images/1038946/6177403-1527059485.png\"},{\"Href\":\"goods_images/1038946/6177405-1527060185.jpg\"}],\"CreatedAt\":\"2018-05-16T10:51:43\",\"IsActive\":true,\"Name\":\"smartfon-huawei-y7-prime-2018-black-51092-jha-\",\"Description\":\"Диагональ: 5.99'' / Матрица: IPS / Разрешение дисплея: 1440х720 / Частота процессора: 1.4 ГГц / Разрешение основной камеры: 13 + 2 Мп / Разрешение фронтальной камеры: 8 Мп / Оперативная память: 3072 Мб / Встроенная память: 32 Гб / Расширение памяти: MicroSD до 256 Гб / Поддержка 3G: да / Поддержка 4G (LTE): да / Встроенный GPS: да / Количество SIM-карт: 2 / Тип SIM-карты: nano-SIM / ОС: Android / Поддержка NFC: нет / USB Подключение: microUSB\",\"Price\":5999.00,\"Title\":\"Смартфон HUAWEI Y7 Prime 2018 Black (51092JHA)\"}]}";

        [SetUp]
        public void SetUp()
        {
            _informationImportService = new InformationImportService(_productRepository, _imageRepository, _priceRepository, _dataReader);
        }

        [Test]
        public void ImportProducts_SavingProductAndItsImages()
        {
            //arrange
            _productRepository.ClearReceivedCalls();
            _productRepository.ClearSubstitute();
            _imageRepository.ClearReceivedCalls();
            _dataReader.GetJsonFromUrl(Arg.Any<string>()).Returns(_jsonExample);

            //act
            _informationImportService.ImportProducts();

            //assert
            _productRepository.Received(1).Add(Arg.Is<Product>(x => x.Id == _firstProductId));
            _productRepository.Received(3).Add(Arg.Any<Product>());
            _imageRepository.Received(33).Add(Arg.Any<Image>());
        }

        [Test]
        public void ImportProducts_SavingProductOnlyIfItIsNew()
        {
            //arrange
            _productRepository.ClearReceivedCalls();
            _imageRepository.ClearReceivedCalls();
            _dataReader.GetJsonFromUrl(Arg.Any<string>()).Returns(_jsonExample);
            _productRepository.Get(_firstProductId).Returns(new Product());

            //act
            _informationImportService.ImportProducts();

            //assert
            _productRepository.Received(0).Add(Arg.Is<Product>(x => x.Id == _firstProductId));
            _productRepository.Received(2).Add(Arg.Any<Product>());
            _imageRepository.Received(28).Add(Arg.Any<Image>());
        }


        [Test]
        public void ImportProducts_SavingImagesOnlyForNewProducts()
        {
            //arrange
            _productRepository.ClearReceivedCalls();
            _imageRepository.ClearReceivedCalls();
            _dataReader.GetJsonFromUrl(Arg.Any<string>()).Returns(_jsonExample);
            _productRepository.Get(_firstProductId).Returns(new Product());

            //act
            _informationImportService.ImportProducts();

            //assert
            _imageRepository.Received(0).Add(Arg.Is<Image>(x=>x.ProductId == _firstProductId));
        }

        [TestCase(true, 0)]
        [TestCase(false, 2)]
        public void ImportProducts_SavingProductPrices(bool samePriceExists, int recievedAddCalls)
        {
            //arrange
            _productRepository.ClearReceivedCalls();
            _priceRepository.ClearReceivedCalls();
            _priceRepository.ClearSubstitute();
            var productPrice = 5999.0m;
            _dataReader.GetJsonFromUrl(Arg.Any<string>()).Returns(_jsonExample);
            if (samePriceExists)
                _priceRepository.FindSet(x => true).ReturnsForAnyArgs(new List<Price> {new Price {ProductPrice = productPrice } });

            //act
            _informationImportService.ImportProducts();

            //assert
            _priceRepository.Received(recievedAddCalls).Add(Arg.Is<Price>(x=>x.ProductPrice == productPrice));
        }

        [Test]
        public void ImportProducts_AllDataWasSaved()
        {
            //arrange
            _productRepository.ClearReceivedCalls();
            _priceRepository.ClearReceivedCalls();
            _imageRepository.ClearReceivedCalls();
            _dataReader.GetJsonFromUrl(Arg.Any<string>()).Returns(_jsonExample);

            //act
            _informationImportService.ImportProducts();

            //assert
            _productRepository.Received(1).Save();
            _priceRepository.Received(1).Save();
            _imageRepository.Received(1).Save();
        }
    }
}
