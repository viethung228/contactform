Dropzone.autoDiscover = false;
Dropzone.autoProcessQueue = false;

// Get the template HTML and remove it from the doument
var previewNode = document.querySelector("#dropzoneTemplate");
previewNode.id = "";
var previewTemplate = previewNode.parentNode.innerHTML;
previewNode.parentNode.removeChild(previewNode);

var myDropzone = new Dropzone(document.body, { // Make the whole body a dropzone
    url: "/House/UploadMedia", // Set the url
    //thumbnailWidth: 120,
    //thumbnailHeight: 60,
    parallelUploads: 100,
    previewTemplate: previewTemplate,
    autoQueue: false, // Make sure the files aren't queued until manually added
    previewsContainer: "#previews", // Define the container to display the previews
    clickable: ".fileinput-button", // Define the element that should be used as click trigger to select files.
    init: function () {
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
        });
    }
});

//Hide the total progress bar when nothing's uploading anymore
myDropzone.on("queuecomplete", function (progress, response) {
    document.querySelector("#total-progress").style.opacity = "0";
    $(".fileupload-process").addClass("hidden");

    $("#frmHouse").submit();
    return false;
});

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

    el.closest("tr").remove();
});


function RemoveImageEvent(btn) {
    var currentControl = $(btn);
    var id = currentControl.data("id");

    var params = $.extend({}, doAjax_params_default);
    params['url'] = "/House/RemoveImage";
    params['requestType'] = "POST";
    params['data'] = { Id: id, Url: currentControl.data("url") };
    params['dataType'] = "json";

    params['successCallbackFunction'] = function (data) {
        if (data) {
            if (data.success) {
                $("#House_img_" + id).remove();
            } else {
                bootbox.alert(data.errorMessage);
            }
        }
    };
    doAjax(params);
}

$("#frmHouse").on("submit", function () {
    var ctrl = $(".btn-house-submit");
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
            ctrl.buttonLoading();
            hideLoading();
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

    //$.aPost("/House/Edit", data, function (result) {
    //    ctrl.buttonLoading();
    //    hideLoading();

    //    if (result.success) {
    //        if (result.message) {
    //            $.showSuccessMessage(LanguageDic["LB_NOTIFICATION"], result.message, function () {
    //                return false;
    //            });
    //        }
    //    } else {
    //        if (result.message) {
    //            $.showErrorMessage(LanguageDic["LB_NOTIFICATION"], result.message, function () {
    //                return false;
    //            });
    //        }
    //    }
    //}, 'json', true);

    return false;
});

$("body").on("click", ".btn-house-submit", function () {
    var ctrl = $(this);
    var isValid = true;

    isValid = $("#frmHouse").valid();

    if (!isValid) {
        var firstError = $("#frmHouse").find(".input-validation-error").first();
        GoToElement(firstError);
        firstError.focus();

        return false;
    }
   
    if (isValid) {
        var newFilesUpload = myDropzone.getFilesWithStatus(Dropzone.ADDED);
        if (newFilesUpload.length > 0) {
            myDropzone.enqueueFiles(newFilesUpload);
        } else {
            $("#frmHouse").submit();

            return false;
        }
    }
});