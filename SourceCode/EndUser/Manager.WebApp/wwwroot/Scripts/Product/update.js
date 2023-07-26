Dropzone.autoDiscover = false;
Dropzone.autoProcessQueue = false;

var myImagesDropzone = null;

function DropzoneImagesInit() {
    // Get the template HTML and remove it from the doument
    var previewNode = document.querySelector("#dropzoneImageTemplate");
    previewNode.id = "";

    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    myImagesDropzone = new Dropzone("#dropzoneImages", { // Make the whole body a dropzone
        url: `/Product/UploadMedia?type=${$("#MediaTypeImage").val()}`, // Set the url
        //thumbnailWidth: 120,
        //thumbnailHeight: 60,
        acceptedFiles: "image/*, video/*",
        timeout: 1800000,
        parallelUploads: 100,
        previewTemplate: previewTemplate,
        autoQueue: false, // Make sure the files aren't queued until manually added
        previewsContainer: "#dropzonImagePreviews", // Define the container to display the previews
        clickable: ".dropzone-image-area", // Define the element that should be used as click trigger to select files.
        init: function () {
            this.on("sending", function (file, xhr, formData) {
                var desEl = $(file.previewElement).find(".dropzone-preview-item-description");
                if (desEl) {
                    formData.append("description", desEl.val());
                }

                SubmitOnProgress();
            });

            this.on("success", function (file, response) {
                var mediaFiles = [];
                var jsonAtts = $("#AllMediaFiles").val();
                if (jsonAtts != undefined && jsonAtts.length > 0) {
                    mediaFiles = JSON.parse(jsonAtts);
                }

                if (mediaFiles == null) {
                    mediaFiles = [];
                }

                if (response) {
                    if (response.data) {
                        mediaFiles.push(response.data);
                    }
                }

                $("#AllMediaFiles").val(JSON.stringify(mediaFiles));

                SubmitEndProgress();
            });
        }
    });

    //Hide the total progress bar when nothing's uploading anymore
    myImagesDropzone.on("queuecomplete", function (progress, response) {
        document.querySelector("#total-image-upload-progress").style.opacity = "0";
        $(".image-upload-process").addClass("hidden");

        var uploadDone = true;

        if (uploadDone) {
            $("#frmProduct").submit();
        }

        return false;
    });
}

DropzoneImagesInit();

$("body").on("click", ".btn-remove-media", function () {
    var el = $(this);
    var id = el.data("id");
    var jsonAtts = $("#AllMediaFiles").val();
    if (jsonAtts != undefined && jsonAtts.length > 0) {
        mediaFiles = JSON.parse(jsonAtts);
    }

    if (mediaFiles == null) {
        mediaFiles = [];
    }
    mediaFiles = mediaFiles.filter(x => x.Id != id);
    $("#AllMediaFiles").val(JSON.stringify(mediaFiles));

    el.closest(".media-item").remove();
});

$(function () {
    const parentCategories = JSON.parse($("#parent-categories").val());
    const childrenCategoires = JSON.parse($("#children-categories").val());
    const volumes = JSON.parse($("#volumes").val())

    let categorySelect = $('.category-selection');
    categorySelect.on("change", function () {
        let selectedValue = categorySelect.find(":selected").val();

        if (selectedValue > 0) {
            for (e = 0; e < parentCategories.length; e++) {
                if (selectedValue == parentCategories[e].Id) {
                    //Property
                    resetPropertyForm();
                    generatePropertyForm(selectedValue, parentCategories);

                    //if (parentCategories[e].ChildrenCategories != null) {
                    //    $('.add-sub-category-btn').removeClass('hidden');
                    //    $('.sub-category').addClass('hidden');
                    //    resetCategorySelect(parentCategories[e].ChildrenCategories);
                    //} else {
                    //    $('.add-sub-category-btn').addClass('hidden');
                    //    $('.sub-category').addClass('hidden');
                    //}
                }
            }
        }
    })

    $('.add-sub-category-btn').click(function () {
        $('.sub-category').removeClass('hidden');
    })

    let subCategorySelect = $(".sub-category-selection");

    subCategorySelect.on("change", function () {
        let selectedValue = subCategorySelect.find(":selected").val();

        if (selectedValue < 0) {
            $(".additional-category").removeClass("hidden");
        }
        else {
            $(".additional-category").addClass("hidden");
            for (e = 0; e < childrenCategoires.length; e++) {
                if (selectedValue == childrenCategoires[e].Id) {
                    ressetPropertyForm();
                    generatePropertyForm(childrenCategoires[e].ParentId, parentCategories);
                }
            }
        }
    })

    let brandSelect = $(".company-selection");

    brandSelect.on("change", function () {
        let selectedValue = brandSelect.find(":selected").val();

        if (selectedValue < 0) {
            $(".additional-company").removeClass("hidden");
        }
        else {
            $(".additional-company").addClass("hidden");
        }
    })

    let originSelect = $(".origin-selection");

    originSelect.on("change", function () {
        let selectedValue = brandSelect.find(":selected").val();

        if (selectedValue < 0) {
            $(".additional-origin").removeClass("hidden");
        }
        else {
            $(".additional-origin").addClass("hidden");
        }
    })
})

function generatePropertyForm(selectedValue, parentCategories) {
    if (selectedValue !== null && parentCategories !== null && parentCategories.length > 0) {
        for (i = 0; i < parentCategories.length; i++) {
            if (selectedValue == parentCategories[i].Id) {
                let properties = parentCategories[i].Properties;
                let categoryId = parentCategories[i].Id;
                if (properties !== null && properties.length > 0) {
                    $(".property-manage").removeClass('hidden');
                    for (j = 0; j < properties.length; j++) {
                        let propertyRow = $(".property-wrapper").find(".property-set").first().clone();
                        propertyRow.removeClass('hidden');

                        propertyRow.find('.property-name').removeClass('hidden');
                        propertyRow.find('.property-name').val(properties[j].Name);

                        propertyRow.find('.property-id').attr('name', `Properties[${j}].PropertyId`);
                        propertyRow.find('.property-id').val(properties[j].Id);

                        if (properties[j].Type === 'Text') {
                            propertyRow.find('.property-value-input').removeClass('hidden');

                            propertyRow.find('.property-value-input').attr('name', `Properties[${j}].Value`)
                        } else if (properties[j].Type === 'Dropdown') {
                            propertyRow.find('.property-value-input').addClass('hidden');
                            propertyRow.find('.property-value-select').removeClass('hidden');

                            propertyRow.find('.property-value-select').addClass(`${properties[j].Name}`)
                            propertyRow.find('.property-value-select').attr('name', `Properties[${j}].Value`)

                            let options = properties[j].Options;
                            if (options !== null && options.length > 0) {
                                for (m = 0; m < options.length; m++) {
                                    propertyRow.find(`.property-value-select`).append($('<option/>').attr("value", options[m].Text).text(options[m].Text));
                                }
                            }
                        }
                        $(".property-wrapper").append(propertyRow);
                    }
                }
            }
        }
    }
}

function resetPropertyForm() {
    $(".property-manage").addClass('hidden');
    let propertySets = $(".property-wrapper").find(".property-set");
    //reset first property-set
    let firstItem = $(".property-set").first();
    firstItem.addClass('hidden');

    firstItem.find(".property-name")[0].Value = "";
    firstItem.find(".property-name")[0].classList.add("hidden");

    firstItem.find(".property-value-input")[0].Value = "";
    firstItem.find(".property-value-input")[0].classList.add("hidden");
    firstItem.find(".property-value-input")[0].name = '';

    firstItem.find(".property-value-select")[0].classList.add("hidden");
    firstItem.find(".property-value-select")[0].name = '';

    for (i = 1; i < propertySets.length; i++) {
        propertySets[i].remove();
    }
}

function generatePriceByVolumeForm(pricingByVolume, volumes) {
    if (pricingByVolume) {
        resetPriceNotByVolumeForm();

        //generate price-by-volume form
        $('.price-by-volume').removeClass('hidden');
        let table = document.getElementById("price-by-volume-table");
        let count = $("#price-by-volume-table > tbody > tr").length;
        for (i = 0; i < volumes.length; i++) {
            count++
            var row = table.insertRow(count);
            var cell1 = row.insertCell(0);
            var cell2 = row.insertCell(1);
            var cell3 = row.insertCell(2);
            cell1.innerHTML = volumes[i].Value + volumes[i].Unit;
            cell1.className = "text-center";
            cell2.innerHTML = `<input type="text" class="form-control text-right" name="Prices[${i}].Price" placeholder="Enter Price"/>` + `<input type="text" class="form-control text-right hidden" name="Prices[${i}].VolumeId" value="${volumes[i].Id}"/>`;
            cell3.innerHTML = `<input type="text" class="form-control text-right" name="Prices[${i}].OriginalPrice" placeholder="Enter Original Price"/>`
        }
    } else {
        resetPricingByVolumeForm();

        //generate price-not-by-volume form
        //let priceRow = $(".price-not-by-volume").find(".price-not-by-volume-set").first().clone();
        //priceRow.removeClass('hidden');
        //$(".price-not-by-volume").append(priceRow);

        $(".price-not-by-volume").removeClass('hidden');
        $(".price-not-by-volume").find(".price-not-by-volume-set-price-value").first().attr('name', 'Prices[0].Price');
        $(".price-not-by-volume").find(".price-not-by-volume-set-original-price-value").first().attr('name', 'Prices[0].OriginalPrice');

        $(".price-not-by-volume").find(".price-not-by-volume-set-price-value").addClass('price');
        $(".price-not-by-volume").find(".price-not-by-volume-set-original-price-value").addClass('original-price');
    }
}

function resetPriceNotByVolumeForm() {
    //let propertySets = $(".price-not-by-volume").find(".price-not-by-volume-set");

    //for (i = 1; i < propertySets.length; i++) {
    //    propertySets[i].remove();
    //}
    $(".price-not-by-volume").addClass('hidden');
    $(".price-not-by-volume").find(".price-not-by-volume-set-price-value").first().val('');
    $(".price-not-by-volume").find(".price-not-by-volume-set-price-value").first().attr('name', '');
    $(".price-not-by-volume").find(".price-not-by-volume-set-original-price-value").first().val('');
    $(".price-not-by-volume").find(".price-not-by-volume-set-original-price-value").first().attr('name', '');

    $(".price-not-by-volume").find(".price-not-by-volume-set-price-value").removeClass('price');
    $(".price-not-by-volume").find(".price-not-by-volume-set-original-price-value").removeClass('original-price');

    $(".price-not-by-volume").find(".invalid-price").addClass('hidden');
    $(".price-not-by-volume").find(".invalid-original-price").addClass('hidden');
}

function resetPricingByVolumeForm() {
    $("#price-by-volume-table").find("tr:not(:first)").remove();
    $(".price-by-volume").addClass('hidden');
}
function resetCategorySelect(childrenCategories) {
    var subCategorySelect = $('.sub-category-selection');
    subCategorySelect.empty();
    subCategorySelect.append($("<option disabled selected></option>")
        .attr("value", -2).text('-- Select Category --'));
    subCategorySelect.append($("<option></option>")
        .attr("value", -1).text('Other'));
    for (i = 0; i < childrenCategories.length; i++) {
        subCategorySelect.append($("<option></option>")
            .attr("value", childrenCategories[i].Id).text(childrenCategories[i].Name));
    };
};

$("#frmProduct").on("submit", function () {
    var ctrl = $(".btn-product-submit");
    ctrl.buttonLoading();
    showLoading();

    $.ajax({
        url: $(this).attr("action"),
        type: 'POST',
        dataType: "JSON",
        data: new FormData(this),
        processData: false,
        contentType: false,
        success: function (result) {
            //ctrl.buttonLoading();
            //hideLoading();
            if (result.success) {
                if (result.message) {
                    $.showSuccessMessage(LanguageDic["LB_NOTIFICATION"], result.message, function () {
                        if (!result.backlink) {
                            location.reload();
                        } else {
                            location.href = result.backlink;
                        }

                        return false;
                    });
                }
            } else {
                if (result.message) {
                    $.showErrorMessage(LanguageDic["LB_NOTIFICATION"], result.message, function () {
                        return false;
                    });
                }
            }
        },
        error: function () {
            ctrl.buttonLoading();
            hideLoading();
        }
    });

    return false;
});

$("body").on("click", ".btn-product-submit", function () {
    SubmitProduct();
});

function SubmitProduct() {
    var isValid = true;

    if (!$("#frmProduct").valid()) {
        var firstError = $("#frmProduct").find(".input-validation-error").first();
        $("#TabBaseInfo").click();

        FocusToErrorElement(firstError);
        firstError.focus();

        isValid = false;
        return false;
    }
    else {
        //Price
        var prices = document.getElementsByClassName("price");
        if ($('#pricing-by-volume').is(':checked') == true) {
            for (var i = 0; i < prices.length - 1; i++) {
                let price = prices[i].value;
                if ((price == "") || isNaN(price) || parseFloat(price) < 0) {
                    prices[i].closest('.price-set').querySelectorAll('.invalid-price')[0].classList.remove('hidden');
                    isValid = false;
                } else {
                    prices[i].closest('.price-set').querySelectorAll('.invalid-price')[0].classList.add('hidden');
                }
            }
        } else {
            for (var i = 0; i < prices.length; i++) {
                let price = prices[i].value;
                if ((price == "") || isNaN(price) || parseFloat(price) < 0) {
                    prices[i].closest('.price-set').querySelectorAll('.invalid-price')[0].classList.remove('hidden');
                    isValid = false;
                } else {
                    prices[i].closest('.price-set').querySelectorAll('.invalid-price')[0].classList.add('hidden');
                }
            }
        }

        var originalPrices = document.getElementsByClassName("original-price");
        if ($('#pricing-by-volume').is(':checked') == true) {
            for (var i = 0; i < originalPrices.length - 1; i++) {
                let originalPrice = originalPrices[i].value;
                if ((originalPrice == "") || isNaN(originalPrice) || parseFloat(originalPrice) <= 0) {
                    originalPrices[i].closest('.original-price-set').querySelectorAll('.invalid-original-price')[0].classList.remove('hidden');
                    isValid = false;
                } else {
                    originalPrices[i].closest('.original-price-set').querySelectorAll('.invalid-original-price')[0].classList.add('hidden');
                }
            }
        } else {
            for (var i = 0; i < originalPrices.length; i++) {
                let originalPrice = originalPrices[i].value;
                if ((originalPrice == "") || isNaN(originalPrice) || parseFloat(originalPrice) <= 0) {
                    originalPrices[i].closest('.original-price-set').querySelectorAll('.invalid-original-price')[0].classList.remove('hidden');
                    isValid = false;
                } else {
                    originalPrices[i].closest('.original-price-set').querySelectorAll('.invalid-original-price')[0].classList.add('hidden');
                }
            }
        }

        if (isValid) {
            DoUploadFilesAndSubmit();

            return false;
        }
    }
}

function DoUploadFilesAndSubmit() {
    var newImagesUpload = myImagesDropzone.getFilesWithStatus(Dropzone.ADDED);

    var uploadDone = false;
    if (newImagesUpload.length > 0) {
        myImagesDropzone.enqueueFiles(newImagesUpload);
    } else {
        uploadDone = true;
    }

    if (uploadDone) {
        $("#frmProduct").submit();

        return false;
    }
}

function SubmitOnProgress() {
    var ctrl = $(".btn-house-submit");
    ctrl.buttonLoading();
    showLoading();
}

function SubmitEndProgress() {
    //var ctrl = $(".btn-house-submit");
    //ctrl.buttonLoading();
    //hideLoading();
}

function PricingByVolumeCheck() {
    const volumes = JSON.parse($("#volumes").val())
    //Price
    generatePriceByVolumeForm($('#pricing-by-volume').is(':checked'), volumes);
}
