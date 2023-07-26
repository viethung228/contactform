productEngine = new Bloodhound({
    identify: function (o) { return o.categoryid; },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    dupDetector: function (a, b) { return a.Id === b.Id; },
    remote: {
        url: '/Filter/GetProductNameSuggestion?query=%QUERY',
        wildcard: '%QUERY',
        cache: true,
        replace: function (url, uriEncodedQuery) {
            var exceptIds = $("#ExceptProducts").val();
            //var exceptIds = "";
            return url.replace("%QUERY", uriEncodedQuery) + '&excepts=' + encodeURIComponent(exceptIds)
        },
    }
});

answerEngine = new Bloodhound({
    identify: function (o) { return o.categoryid; },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    dupDetector: function (a, b) { return a.Id === b.Id; },
    remote: {
        url: '/Filter/GetAnswerSuggestion?query=%QUERY',
        wildcard: '%QUERY',
        cache: true,
        replace: function (url, uriEncodedQuery) {
            return url.replace("%QUERY", uriEncodedQuery)
        },
    }
});

eventEngine = new Bloodhound({
    identify: function (o) { return o.categoryid; },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    dupDetector: function (a, b) { return a.Id === b.Id; },
    remote: {
        url: '/Filter/GetEventSuggestion?query=%QUERY',
        wildcard: '%QUERY',
        cache: true,
        replace: function (url, uriEncodedQuery) {
            return url.replace("%QUERY", uriEncodedQuery)
        },
    }
});

function FilterInputCounter() {
    var total = 0;
    $(".filter-item").each(function () {
        total++;
        $(this).find(".add-idx").html(total);
    });
}

$(document).on("click", ".btn-remove-filter", function () {
    $(this).closest(".filter-item").remove();
    FilterInputCounter();
    var randomCode = $(this).data("code");

    AllFilters = AllFilters.filter(function (el) { return el.RandomCode != randomCode; });

    $("#UserFilters").val(JSON.stringify(AllFilters));

})

function ValidateSpecialFilters() {
    if ($(".logged-in-done").length > 0) {
        $("#myModal").modal("hide");
        $.showErrorMessage('Điều kiện "Người vừa đăng nhập, đăng kí thành công" không thể đi kèm với điều kiện khác', '')
        return false;
    }

    if ($(".answer-question-done").length > 0) {
        $("#myModal").modal("hide");
        $.showErrorMessage('Điều kiện "khách hàng vừa trả lời xong câu hỏi" không thể đi kèm với điều kiện khác', '')
        return false;
    }

    if ($(".seleted-alluser").length > 0) {
        $("#myModal").modal("hide");
        $.showErrorMessage('Điều kiện "Chọn tất cả user" không thể đi kèm với điều kiện khác', '')
        return false;
    }

    return true;
}

var TypeAheadControl = {
    ProductRptTypeAhead: function (el) {
        var productTemp = Handlebars.compile($("#product-template").html());
        el.typeahead({
            highlight: true,
            minLength: 0,
            cache: false,
        },
        {
            limit: 10,
            source: function (query, sync, async) {
                if (query === '') {
                    productEngine.search(query, sync, async);
                }
                else {
                    productEngine.search(query, sync, async);
                }
            },
            displayKey: 'Name',
            templates: {
                suggestion: productTemp
            }
        })
        .on('typeahead:selected', function (evt, item) {
            $("#ProductName").attr("value", item.ProductName);
            $("#ProductName").typeahead('val', item.ProductName);
        });

    },
    AnswerRptTypeAhead: function (el) {
        var answerTemp = Handlebars.compile($("#answer-template").html());
        el.typeahead({
            highlight: true,
            minLength: 0,
            cache: false,
        },
        {
            limit: 3,
            source: function (query, sync, async) {
                if (query === '') {
                    answerEngine.search(query, sync, async);
                }
                else {
                    answerEngine.search(query, sync, async);
                }
            },
            displayKey: 'Name',
            templates: {
                suggestion: answerTemp
            }
        })
        .on('typeahead:selected', function (evt, item) {
            $("#Answer").attr("value", item.Label);
            $("#Answer").typeahead('val', item.Label);
        });
    },
    EventRptTypeAhead: function (el) {
        var eventTemp = Handlebars.compile($("#event-template").html());
        el.typeahead({
            highlight: true,
            minLength: 0,
            cache: false,
        },
        {
            limit: 10,
            source: function (query, sync, async) {
                if (query === '') {
                    $("#EventId").val("");
                    eventEngine.search(query, sync, async);
                }
                else {
                    $("#EventId").val("");
                    eventEngine.search(query, sync, async);
                }
            },
            displayKey: 'Name',
            templates: {
                suggestion: eventTemp
            }
        })
        .on('typeahead:selected', function (evt, item) {
            $("#Event").attr("value", item.Name);
            $("#EventId").val(item.Id);
            $("#Event").typeahead('val', item.Name);
            $(".vld-event").addClass("hidden");
        });
    },
};


