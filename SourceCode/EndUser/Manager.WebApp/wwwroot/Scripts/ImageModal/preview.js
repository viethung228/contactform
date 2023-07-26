function PreviewImages() {
    $('div.thumnail-images-box > img').remove();
    if ($("#file-upload")[0].files.length > 0) {

        for (var i = 0; i < $("#file-upload")[0].files.length; i++) {

            let file = $("#file-upload")[0].files[i];

            if (!file.type.match('image.*')) {
                return;
            }
            var reader = new FileReader();
            reader.onload = function (e) {
                let src = e.target.result;
                console.log(src)
                let img = $(`<img src="${src}" style="width:150px; height:150px" id="preview-image" class="file-upload-btn thumbImg img-full-center pointer"/>`);
                $('.thumnail-images-box').append(img);
            }
            //read img
            reader.readAsDataURL(file)

        }
        $("#preview-upload-btn").addClass('hidden');

    } else {
        //$("#preview-image").addClass('hidden');

        //$('div.thumnail-images-box > img').remove();
        $("#preview-upload-btn").removeClass('hidden');
    }

}
function PreviewImage() {
    if (document.getElementById("file-upload").files.length > 0) {
        var oFReader = new FileReader();
        oFReader.readAsDataURL(document.getElementById("file-upload").files[0]);

        oFReader.onload = function (oFREvent) {
            document.getElementById("preview-image").src = oFREvent.target.result;
            document.getElementById("preview-image").classList.remove('hidden');
            document.getElementById("preview-upload-btn").classList.add('hidden');
        }
    } else {
        document.getElementById("preview-image").classList.add('hidden');
        document.getElementById("preview-upload-btn").classList.remove('hidden');
    }

}