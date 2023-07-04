parentEngine = new Bloodhound({
    identify: function (o) { return o.categoryid; },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
    dupDetector: function (a, b) { return a.Id === b.Id; },
    remote: {
        url: '/Seller/GetParentSuggestion?query=%QUERY',
        wildcard: '%QUERY',
        cache: true,
        replace: function (url, uriEncodedQuery) {
            var sellerType = $("#SellerType").val();
            var urlTmp = url.replace("%QUERY", uriEncodedQuery);
            var returnUrl = urlTmp.concat(`&sellertype=${sellerType}`)
            return returnUrl
        },
    }
});

var TypeAheadControl = {
    ParentRptTypeAhead: function (el) {
        var parentTemp = Handlebars.compile($("#parent-template").html());
        el.typeahead().bind('typeahead:close', function () {
            //$('#Parent').typeahead('val', '')
            $("#ParentId").trigger('propertychange.typeahead')
        });

        el.typeahead({
            highlight: true,
            minLength: 0,
            cache: false,
        },
        {
            limit: 10,
            source: function (query, sync, async) {
                
                if (query === '') {
                    console.log("parent1")
                    parentEngine.search(query, sync, async);
                    //$("#IsSelectParent").val(0);
                }
                else {
                    console.log("parent2")
                    parentEngine.search(query, sync, async);
                }
            },
            displayKey: 'Name',
            templates: {
                suggestion: parentTemp
            }
        })
        .on('typeahead:selected', function (evt, item) {
            //$("#IsSelectParent").val(1);
            $("#Parent").attr("value", item.Id);
            $("#Parent").typeahead('val', item.Name);
        });
    }
}

function GetParentList(sellerType) {
    $.aGet(`/Seller/GetParentList?sellerType=${sellerType}`, null, function (result) {
        $(".select-superior").html("");
        if (result) {
            if (result.html) {
                $(".select-superior").html(result.html);
                $("#ParentId").selectpicker();
            }

        }
    }, "json", true);
};

$("#SellerType").change(function () {
    var el = $(this);
    if (el.val() != 1) {
        console.log(el.val())
        $(".parent-seller").removeClass("hidden");
        $("#ParentId").typeahead('destroy');
        TypeAheadControl.ParentRptTypeAhead($("#ParentId"));

        GetParentList(el.val());
    }
    else {
        $(".parent-seller").addClass("hidden");
    }
})

$(function () {
    if ($("#SellerType").val() > 1) {
        $(".parent-seller").removeClass("hidden");
        $("#ParentId").selectpicker();
    }
})

