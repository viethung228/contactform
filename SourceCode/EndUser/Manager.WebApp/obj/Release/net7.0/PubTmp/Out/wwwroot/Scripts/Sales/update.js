$(function () {
    var idx = 0;
    $(".chart-item").each(function () {
        var chartItemCtn = $(this);
        chartItemCtn.areaLoading();

        var frmData = $("#ReportSearchForm").serializeArray();
        var reportType = chartItemCtn.data("type");
        frmData.push({ name: "ReportType", value: reportType });

        if (idx == 0 || idx == -1) {
            $.aPost("/Sales/GetChartReportData", frmData, function (result) {
                idx++;
                if (result) {
                    if (result.data) {
                        SalesControl.DrawChart(chartItemCtn, result.data);

                        chartItemCtn.areaLoading();
                    }
                }
            });
        }

        idx++;
    });
});