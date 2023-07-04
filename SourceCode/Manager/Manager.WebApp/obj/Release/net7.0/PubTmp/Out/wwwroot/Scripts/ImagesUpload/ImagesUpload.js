

class MultipleUploader {

    #multipleUploader;
    #$imagesUploadInput;

    constructor(multiUploaderSelector) {
        this.#multipleUploader = document.querySelector(multiUploaderSelector);
        this.#$imagesUploadInput = document.createElement('input')
    }

    init({ maxUpload = 10, maxSize = 2, formSelector = 'form', filesInpName = 'images', className = 'images', imgUrls = "https://nld.mediacdn.vn/2020/5/29/doi-hoa-tim-6-15907313455782118586731.jpg" } = {}) {

        const form = document.querySelector(formSelector);

        if (!this.#multipleUploader) // check if the end user didnt write the multiple uploader div
            throw new Error('The multiple uploader element doesnt exist');

        if (!form) // check if there is no form with this selector
            throw new Error('We couldn\'t find a form with this selector: ' + formSelector);

        // ensure that the form has enctype attribute with the value multipart/form-data
        form.enctype = 'multipart/form-data'

        if (document.getElementById('max-upload-number'))
            document.getElementById('max-upload-number').innerHTML = `Upload up to ${maxUpload} files`;

        // create multiple file input and make it hidden
        this.#$imagesUploadInput.type = 'file';
        this.#$imagesUploadInput.name = `${filesInpName}[]`;
        this.#$imagesUploadInput.className = `${className}`;
        this.#$imagesUploadInput.multiple = true;
        this.#$imagesUploadInput.accept = "image/*";
        this.#$imagesUploadInput.style.setProperty('display', 'none', 'important');
        // create multiple file input and make it hidden

        // append the newly created input to the form with the help of the formSelector provided by the user
        document.querySelector(formSelector).append(this.#$imagesUploadInput);

        //if (imgUrls != "") {
        //    this.#multipleUploader.#preRenderImages(imgUrls)
        //    let imgUrlArray = imgUrls.split(';').filter(element => element);
        //    this.#multipleUploader.#addFilesToInput(imgUrlArray)
        //}

        this.#multipleUploader.addEventListener("click", (e) => {

            if (e.target.className === 'multiple-uploader' || e.target.className === 'mup-msg' || e.target.className === 'mup-main-msg')
                this.#$imagesUploadInput.click() // trigger the input file to upload images

        });

        const self = this;

        // preview the uploaded images
        this.#$imagesUploadInput.addEventListener("change", function () {

            if (this.files.length > 0) {
                self.#multipleUploader.querySelectorAll('.image-container').forEach(image => image.remove()); // clear the previous rendered images
                self.#multipleUploader.querySelector('.mup-msg').style.setProperty('display', 'none'); // hide the hint texts inside drop zone

                // if the length of uploaded images greater than the images uploaded by the user, the maximum uploaded will be considered
                const uploadedImagesCount = this.files.length > maxUpload ? maxUpload : this.files.length;
                const unAcceptableImagesIndices = [];

                for (let index = 0; index < uploadedImagesCount; index++) {

                    const imageSize = self.#bytesToSize(this.files[index].size);
                    const isImageSizeAcceptable = self.#checkImageSize(index, imageSize, maxSize, 'MB');


                    // appended the newly created image to the multiple uploader
                    //    self.#multipleUploader.innerHTML += `
                    //<div class="image-container" data-image-index="${index}" id="mup-image-${index}" data-acceptable-image="${+isImageSizeAcceptable}" >
                    //    <div class="image-size"> ${imageSize['size'] + ' ' + imageSize['unit']} </div>
                    //    ${!isImageSizeAcceptable ? `<div class="exceeded-size"> greater than ${maxSize} MB </div>` : ''}
                    //    <img src="${URL.createObjectURL(this.files[index])}"  class="image-preview" alt="" />
                    //</div>`;

                    self.#multipleUploader.innerHTML += `
                <div class="image-container" data-image-index="${index}" id="mup-image-${index}" data-acceptable-image="${+isImageSizeAcceptable}" >        
                    <img src="${URL.createObjectURL(this.files[index])}"  class="image-preview" alt="" />
                </div>`;


                    if (!isImageSizeAcceptable)
                        unAcceptableImagesIndices.push(index)

                }

                unAcceptableImagesIndices.forEach((index) => self.#removeFileFromInput(index, false))

            }

        });

        // event for deleting uploaded images
        document.addEventListener('click', function (e) {

            if (e.target.className === 'image-container') // clicked on remove pseudo element
            {
                const imageIndex = e.target.getAttribute(`data-image-index`)
                const imageIsAcceptable = e.target.getAttribute(`data-acceptable-image`)

                e.target.remove() // remove the html element from the dom

                if (+imageIsAcceptable)
                    self.#removeFileFromInput(imageIndex)

                if (document.querySelectorAll('.image-container').length === 0) // if there are no images
                    self.clear();


                self.#reorderFilesIndices(); // reorder images indices
            }

        });



        return this;

    }

    clear() {
        this.#multipleUploader.querySelectorAll('.image-container').forEach(image => image.remove());
        this.#multipleUploader.querySelectorAll('.mup-msg').forEach(msg => msg.style.setProperty('display', 'flex'));
        this.#$imagesUploadInput.value = [];
    }

    #removeFileFromInput(deletedIndex) {
        // remove the delete file from input
        const dt = new DataTransfer()

        for (const [index, file] of Object.entries(this.#$imagesUploadInput.files)) {
            if (index != deletedIndex)
                dt.items.add(file)
        }

        this.#$imagesUploadInput.files = dt.files
        // remove the delete file from input

    }

    #addFilesToInput(imgUrlArray) {
        // remove the delete file from input
        const dt = new DataTransfer()

        for (let i = 0; i < imgUrlArray.length; i++) {
            var xhr = new XMLHttpRequest();
            xhr.open("GET", imgUrlArray[i], true);
            xhr.responseType = "blob";

            xhr.onload = function () {
                if (xhr.status === 200) {
                    var blob = xhr.response;
                    var file = new File([blob], "image.jpg", { type: "image/jpeg" });
                    dt.items.add(file);
                }
            };

            xhr.send();
        }

        this.#$imagesUploadInput.files = dt.files
        // remove the delete file from input

    }

    #reorderFilesIndices() {
        document.querySelectorAll('.image-container').forEach((element, index) => {
            element.setAttribute('data-image-index', index.toString());
            element.setAttribute('id', `mup-image-${index}`)
        });
    }

    #checkImageSize(imageIndex, imageSize, maxSize) {
        return imageSize['unit'] !== 'MB' || (imageSize['unit'] === 'MB' && (imageSize['size'] <= maxSize)); // return true if acceptable
    }

    #bytesToSize(bytes) {
        const sizes = ['Bytes', 'KB', 'MB']

        const i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)), 10)

        if (i === 0)
            return { size: bytes, unit: sizes[i] }
        else
            return { size: (bytes / (1024 ** i)).toFixed(1), unit: sizes[i] }

    }

    #preRenderImages(imageUrls) {
        var imageUrlArray = imageUrls.split(';').filter(element => element);

        //self.#multipleUploader.querySelectorAll('.image-container').forEach(image => image.remove()); // clear the previous rendered images
        //self.#multipleUploader.querySelector('.mup-msg').style.setProperty('display', 'none'); // hide the hint texts inside drop zone

        //// if the length of uploaded images greater than the images uploaded by the user, the maximum uploaded will be considered
        //const uploadedImagesCount = this.files.length > maxUpload ? maxUpload : this.files.length;

        for (let index = 0; index < imageUrlArray.length; index++) {

            // appended the newly created image to the multiple uploader
            self.#multipleUploader.innerHTML += `
                <div class="image-container" data-image-index="${index}" id="mup-image-${index}">        
                    <img src="${imageUrlArray[i]}"  class="image-preview" alt="" />
                </div>`;

        }
    }


}


