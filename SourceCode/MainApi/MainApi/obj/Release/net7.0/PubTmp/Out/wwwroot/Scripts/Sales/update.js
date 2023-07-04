var SalesControl = {
    GetChart: function (type) {
        var fromDate = $("#FromDate").val();
        var toDate = $("#ToDate").val();
        var platForm = $("#Platform").val();

        var formData = [];
        formData.push({ name: "FromDate", value: fromDate });
        formData.push({ name: "ToDate", value: toDate });
        formData.push({ name: "ChartType", value: type });
        formData.push({ name: "Platform", value: platForm });

        $.aPost("/Sales/SalesChart", formData, function (result) {
            if (result.success) {
                var divParent = $("#render-saleschart");
                divParent.html(result.html);
            }
        }, "json", true);
    },
}

$("body").on("click", ".btn-saleschart-bar", function () {
    var chartType = 0;
    SalesControl.GetChart(chartType);
})

$("body").on("click", ".btn-saleschart-line", function () {
    var chartType = 1;
    SalesControl.GetChart(chartType);
})