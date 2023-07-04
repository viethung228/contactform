var CustomerSearchTool = {
    SelectedItems: [],
    SearchLoaded: false,
    SelectMultiple: true,
    SearchPageIdx: 1,    
    SearchCallbackFunc: null,
    SearchCallbackFuncName: "",
    Search: function (idx = 1, more = false) {
        var ctn = $("#modalSearchCustomerContent");
        CustomerSearchTool.SearchPageIdx = idx;

        //if (!more) {
        //    if (!CustomerSearchTool.SearchLoaded) {
        //        ctn.html("");
        //        ctn.areaLoading();
        //    }
        //}
        ctn.find(".search-result-container").html("");
        ctn.find(".search-result-container").areaLoading();

        var sIncludePack = $("#frmCustomerSearch").find(".search-include-pack");
        var include_pack = true;
        if (sIncludePack) {
            include_pack = sIncludePack.val();
            if (include_pack == null || include_pack === "") {
                include_pack = true;
            }
        }

        var frmData = $("#frmCustomerSearch").serializeArray();
        frmData.push({ name: "Page", value: idx });
        frmData.push({ name: "IncludePack", value: include_pack });
        frmData.push({ name: "CallbackFunction", value: CustomerSearchTool.SearchCallbackFuncName });

        $.aPost("/Customer/Search", frmData, function (result) {
            if (result) {
                if (!CustomerSearchTool.SearchLoaded) {
                    ctn.html(result);
                } else {
                    if (!more) {
                        ctn.html(result);
                    } else {
                        ctn.find("#CustomerSearchResults").append(result);
                    }
                }

                $("#modalSearchCustomer").find(".selectpicker").selectpicker().removeClass("selectpicker");

                CustomerSearchTool.SearchModalBindEvents();

                CustomerSearchTool.SearchLoaded = true;

                $("#modalSearchCustomer").modal("show");
            }
        }, "html", false);
    },
    SearchModalShow: function () {
        if (!CustomerSearchTool.SearchLoaded) {
            CustomerSearchTool.Search();
        }
        else {
            $("#modalSearchCustomer").modal("show");
        }
    },
    SearchModalHide: function () {
        $("#modalSearchCustomer").modal("hide");
    },
    SearchModalBindEvents: function () {
        //const ctn = document.querySelector('#modalSearchCustomer .search-result-container');
        //if (ctn) {
        //    ctn.addEventListener('scroll',
        //        SpamProtection(function () {
        //            if (ctn.offsetHeight + ctn.scrollTop >= ctn.scrollHeight) {
        //                var page = CustomerSearchTool.SearchPageIdx + 1;
        //                CustomerSearchTool.Search(page, true);
        //            }
        //        }, 500)
        //    );
        //}

        $(".search-customer-item-cbx.new-item").on("click", function () {            
            CustomerSearchTool.ItemCheckedEvent($(this));            
        });
    },    
    Detail: function (link) {
        $.aGet(link, null, function (result) {
            if (result) {
                $("#modalCustomerDetailContent").html(result);
                CustomerSearchTool.DetailModalShow();
            }
        }, "html", true);

        CustomerSearchTool.SearchModalHide();
    },
    DetailModalShow: function () {
        $("#modalCustomerDetail").modal("show");
    },
    DetailModalHide: function () {
        $("#modalCustomerDetail").modal("hide");
    },
    SearchAllowSelect: function (callbackFuncName) {
        if (!CustomerSearchTool.SelectedItems || CustomerSearchTool.SelectedItems.length === 0) {
            $.showErrorMessage(LanguageDic["LB_NOTIFICATION"], "少なくとも1つの材料を選択してください", function () {
                ModalDisplayBack("modalSearchCustomer");
            });
        }

        if (callbackFuncName !== "") {           
            window[callbackFuncName](CustomerSearchTool.SelectedItems);
            $(".search-customer-item-cbx").prop("checked", false);

            CustomerSearchTool.ClearSelectedItems();

            if (!CustomerSearchTool.SelectedItems || CustomerSearchTool.SelectedItems.length === 0) {
                ModalDisplayBack("modalSearchCustomer");
            }
        } else {
            alert("Please define CallbackFunctionName");
        }
    },    
    ItemCheckedEvent: function (el) {
        if (!CustomerSearchTool.SelectMultiple) {
            CustomerSearchTool.ClearSelectedItems();
        }

        var hasCheckedItems = (CustomerSearchTool.SelectedItems.length > 0);
        var id = el.data("id");
        var info = el.data("info");
        let itemData = {
            id: id,
            info: info
        };

        if (el.is(":checked")) {
            if (hasCheckedItems) {
                var matched = false;
                for (var key in CustomerSearchTool.SelectedItems) {
                    if (CustomerSearchTool.SelectedItems[key]["id"] == id) {
                        matched = true;

                        break;
                    }
                }
                if (!matched) {
                    CustomerSearchTool.SelectedItems.push(itemData);
                }
            }
            else {
                CustomerSearchTool.SelectedItems.push(itemData);
            }
        } else {
            if (hasCheckedItems) {
                for (var key in CustomerSearchTool.SelectedItems) {
                    if (CustomerSearchTool.SelectedItems[key]["id"] == id) {
                        CustomerSearchTool.SelectedItems.splice(key, 1);
                        break;
                    }
                }
            }
        }

        el.removeClass("new-item");
    },
    ClearSelectedItems: function () {
        CustomerSearchTool.SelectedItems = [];
    }
}

MySiteGlobal.bindClickedEvents(".btn-search-customer", function () {
    CustomerSearchTool.SearchModalShow();
});

MySiteGlobal.bindClickedEvents(".btn-search-customer-show", function () {
    CustomerSearchTool.DetailModalHide();

    CustomerSearchTool.SearchModalShow();
});

MySiteGlobal.bindClickedEvents(".btn-filter-customer", function () {
    CustomerSearchTool.Search(1);
});