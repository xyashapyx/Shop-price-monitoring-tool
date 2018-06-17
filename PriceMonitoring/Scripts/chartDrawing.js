function drawPriceGraph(points) {

    var chart = new CanvasJS.Chart("chartContainer", {
        animationEnabled: true,
        title: {
            text: "Price dynamic"
        },
        axisX: {
            valueFormatString: "DD MMM"
        },
        axisY: {
            title: "Price in hrn",
            prefix: "₴",
            includeZero: false
        },
        data: [{
            type: "spline",
            xValueType: "dateTime",
            xValueFormatString: "DD MMM",
            yValueFormatString: "$#.###",
            dataPoints: JSON.parse(points)
        }]
    });
    chart.render();
}
