
$(function () {
    var canvasJson = $(".data-template").val();

    if (canvasJson) {
        imgEditor.setCanvasJSON(canvasJson);
    }
})

$(function () {

    $("body").on("click", ".btn-submit", SpamProtection(function () {

        const templateForm = $("#template-form");

        var templateJson = imgEditor.getCanvasJSON();
        var thumbBaseStr = imgEditor.canvas.toDataURL("png");

        if (templateJson) {
            var templateJsonStr = JSON.stringify(templateJson);
            $(".data-template").val(templateJsonStr);
        }

        $(".data-thumbstr").val(thumbBaseStr);

        if (templateForm.valid()) {
            $(this).buttonLoading();

            //alert("meo")

            templateForm.submit();
        }

    }, 300))
})