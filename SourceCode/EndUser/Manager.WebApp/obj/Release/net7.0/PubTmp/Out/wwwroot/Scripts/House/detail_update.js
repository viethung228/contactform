Dropzone.autoDiscover = false;
Dropzone.autoProcessQueue = false;

var myImagesDropzone = null;
var myConstructImagesDropzone = null;
var myAttachmentsDropzone = null;

function DropzoneImagesInit() {
    // Get the template HTML and remove it from the doument
    var previewNode = document.querySelector("#dropzoneImageTemplate");
    previewNode.id = "";

    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    myImagesDropzone = new Dropzone("#dropzoneImages", { // Make the whole body a dropzone
        url: `/House/UploadMedia?type=${$("#MediaTypeImage").val()}`, // Set the url
        //thumbnailWidth: 120,
        //thumbnailHeight: 60,
        acceptedFiles: "image/*",
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
            });
        }
    });

    //Hide the total progress bar when nothing's uploading anymore
    myImagesDropzone.on("queuecomplete", function (progress, response) {
        document.querySelector("#total-image-upload-progress").style.opacity = "0";
        $(".image-upload-process").addClass("hidden");

        var newConstructImagesUpload = myConstructImagesDropzone.getFilesWithStatus(Dropzone.ADDED);
        var newAttachmentsUpload = myAttachmentsDropzone.getFilesWithStatus(Dropzone.ADDED);
        var uploadDone = false;
        if (newConstructImagesUpload.length > 0) {
            myConstructImagesDropzone.enqueueFiles(newConstructImagesUpload);
        }
        else {
            if (newAttachmentsUpload.length > 0) {
                myAttachmentsDropzone.enqueueFiles(newAttachmentsUpload);
            } else {
                uploadDone = true;
            }
        } 

        if (uploadDone) {
            console.log("done")
            $("#frmHouseDetail").submit();
        }
        
        return false;
    });
}

function DropzoneAttachmentsInit() {
    // Get the template HTML and remove it from the doument
    var previewNode = document.querySelector("#dropzoneAttachmentTemplate");
    previewNode.id = "";

    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    myAttachmentsDropzone = new Dropzone("#dropzoneAttachments", { // Make the whole body a dropzone
        url: `/House/UploadMedia?type=${$("#MediaTypeOther").val()}`, // Set the url
        //thumbnailWidth: 120,
        //thumbnailHeight: 60,
        acceptedFiles: "application/pdf,.doc,.docx,.xls,.xlsx,.csv,.tsv,.ppt,.pptx,.pages,.odt,.rtf",
        parallelUploads: 100,
        previewTemplate: previewTemplate,
        autoQueue: false, // Make sure the files aren't queued until manually added
        previewsContainer: "#dropzonAttachmentPreviews", // Define the container to display the previews
        //clickable: ".dropzone-attachment-area", // Define the element that should be used as click trigger to select files.
        clickable: ".attachment-button", // Define the element that should be used as click trigger to select files.
        init: function () {
            this.on("sending", function (file, xhr, formData) {
                var desEl = $(file.previewElement).find(".dropzone-preview-item-description");
                if (desEl) {
                    formData.append("description", desEl.val());
                }
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
            });
        }
    });

    //Hide the total progress bar when nothing's uploading anymore
    myAttachmentsDropzone.on("queuecomplete", function (progress, response) {
        document.querySelector("#total-image-upload-progress").style.opacity = "0";
        $(".image-upload-process").addClass("hidden");

        $("#frmHouseDetail").submit();
        return false;
    });
}

function DropzoneConstructImagesInit() {
    // Get the template HTML and remove it from the doument
    var previewNode = document.querySelector("#dropzoneConstructImageTemplate");

    var previewTemplate = previewNode.parentNode.innerHTML;
    previewNode.parentNode.removeChild(previewNode);

    myConstructImagesDropzone = new Dropzone("#dropzoneConstructImages", { // Make the whole body a dropzone
        url: `/House/UploadMedia?type=${$("#MediaTypeConstructImage").val()}`, // Set the url
        //thumbnailWidth: 120,
        //thumbnailHeight: 60,
        acceptedFiles: "image/*",
        parallelUploads: 100,
        previewTemplate: previewTemplate,
        autoQueue: false, // Make sure the files aren't queued until manually added
        previewsContainer: "#dropzonConstructImagePreviews", // Define the container to display the previews
        clickable: ".dropzone-construct-image-area", // Define the element that should be used as click trigger to select files.
        init: function () {
            this.on("sending", function (file, xhr, formData) {
                var desEl = $(file.previewElement).find(".dropzone-preview-item-description");
                if (desEl) {
                    formData.append("description", desEl.val());
                }
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
            });
        }
    });

    //Hide the total progress bar when nothing's uploading anymore
    myConstructImagesDropzone.on("queuecomplete", function (progress, response) {
        document.querySelector("#total-image-upload-progress").style.opacity = "0";
        $(".image-upload-process").addClass("hidden");

        var newAttachmentsUpload = myAttachmentsDropzone.getFilesWithStatus(Dropzone.ADDED);
        var newConstructImagesUpload = myConstructImagesDropzone.getFilesWithStatus(Dropzone.ADDED);
        var uploadDone = false;
        if (newConstructImagesUpload.length > 0) {
            myConstructImagesDropzone.enqueueFiles(newConstructImagesUpload);
        } else {
            uploadDone = true;
        }

        if (uploadDone) {
            $("#frmHouseDetail").submit();
        }

        return false;
    });
}

DropzoneImagesInit();
DropzoneConstructImagesInit();
DropzoneAttachmentsInit();

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


$("body").on("click", ".btn-update-house-media", function () {
    var ctrl = $(".btn-house-submit");
    ctrl.buttonLoading();
    showLoading();
    var isValid = true;

    isValid = $("#frmHouseDetail").valid();

    if (!isValid) {
        var firstError = $("#frmHouseDetail").find(".input-validation-error").first();
        GoToElement(firstError);
        firstError.focus();

        return false;
    }

    if (isValid) {
        var newImagesUpload = myImagesDropzone.getFilesWithStatus(Dropzone.ADDED);
        var newConstructImagesUpload = myConstructImagesDropzone.getFilesWithStatus(Dropzone.ADDED);
        var newAttachmentsUpload = myAttachmentsDropzone.getFilesWithStatus(Dropzone.ADDED);

        var uploadDone = false;
        if (newImagesUpload.length > 0) {
            myImagesDropzone.enqueueFiles(newImagesUpload);
        } else {
            if (newConstructImagesUpload.length > 0) {
                myConstructImagesDropzone.enqueueFiles(newConstructImagesUpload);
            }
            else {
                if (newAttachmentsUpload.length > 0) {
                    myAttachmentsDropzone.enqueueFiles(newAttachmentsUpload);
                } else {
                    uploadDone = true;
                }
            }
        }

        if (uploadDone) {
            $("#frmHouseDetail").submit();

            return false;
        }
    }

});

$("#frmHouseDetail").on("submit", function () {
    var ctrl = $(".btn-update-house-media");
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

    return false;
});
