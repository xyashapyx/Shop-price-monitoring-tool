var ViewModel = function () {
    var self = this;
    self.Products = ko.observableArray();
    self.error = ko.observable();
    self.Detail = ko.observable();

    var productsUri = '/api/Products/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: data
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllProducts() {
        ajaxHelper(productsUri, 'GET').done(function (data) {
            self.Products(data);
        });
    }

    self.getProductDetail = function (item) {
        ajaxHelper(productsUri + item.Product.Id, 'GET').done(function (data) {
            self.Detail(data);
            drawPriceGraph(data.Prices);
        });
    }

    // Fetch the initial data.
    getAllProducts();
};

ko.applyBindings(new ViewModel());