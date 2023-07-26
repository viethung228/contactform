$("#TemplateType").change(function () {
    var type = $(this).val();

    var thumbSrc = $(`#email-template-thumb-img-${type}`).val();

    $("#TemplateThumbnail").find("img").attr("src", `${thumbSrc}`);
});

$(".btn-preview-thumbnail").on("click", function () {
    var modalTitle = $("#thumbnailModal .modal-title");
    var modalBody = $("#thumbnailModal .modal-body");

    var type = $("#TemplateType").val();
    modalBody.areaLoading();

    var thumbItem = $(`#email-template-thumb-img-${type}`);
    $.aGet(`/EmailTemplate/GetThumbnail`, { type: type }, function (result) {
        modalBody.html(result);
        modalTitle.html(thumbItem.data("name"));

        $("#thumbnailModal").modal("show");
    }, "html", false);    
});

var isFullscreen = false;
function requestFullScreen(element) {
    if (!isFullscreen) {
        // Supports most browsers and their versions.
        var requestMethod = element.requestFullScreen || element.webkitRequestFullScreen || element.mozRequestFullScreen || element.msRequestFullScreen;

        if (requestMethod) { // Native full screen.
            requestMethod.call(element);
        } else if (typeof window.ActiveXObject !== "undefined") { // Older IE.
            var wscript = new ActiveXObject("WScript.Shell");
            if (wscript !== null) {
                wscript.SendKeys("{F11}");
            }
        }

        isFullscreen = true;
        $("#section-sales-chart-filter").addClass("hidden");
    } else {
        var requestMethod = document.cancelFullScreen || document.webkitCancelFullScreen || document.mozCancelFullScreen || document.exitFullscreen || document.webkitExitFullscreen;
        if (requestMethod) { // cancel full screen.
            requestMethod.call(document);
        } else if (typeof window.ActiveXObject !== "undefined") { // Older IE.
            var wscript = new ActiveXObject("WScript.Shell");
            if (wscript !== null) {
                wscript.SendKeys("{F11}");
            }
        }

        isFullscreen = false;
        $("#section-sales-chart-filter").removeClass("hidden");
    }
}

$("body").on("click", ".btn-full-screen", function () {
    requestFullScreen(document.getElementById("section-saleschart"));
});

$(document).keyup(function (e) {
    if (e.keyCode == 70) {
        requestFullScreen(document.getElementById("section-saleschart"));
        return false;
    }
});

$(".btn-preview").on("click", function () {
    var btn = $(this);
    btn.buttonLoading();
    var modalBody = $("#previewModal .modal-body");

    $.aGet(`/EmailForm/Preview`, { id: $("#EmailFormId").val() }, function (result) {
        modalBody.html(result);
        $("#previewModal").modal("show");

        btn.buttonLoading();
    }, "html", false);
});

$(function () {
    $("#TemplateType").change();
});