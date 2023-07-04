CKEDITOR.disableAutoInline = true;

//CKEDITOR.config.protectedSource.push(/<i[\s\S]*?\>/g); //allows beginning <i> tag
//CKEDITOR.config.protectedSource.push(/<\/i[\s\S]*?\>/g); //allows ending </i> tag
//CKEDITOR.config.allowedContent = true;
//CKEDITOR.dtd.$removeEmpty.i = 0;

//CKEDITOR.config.extraPlugins = "ckeditorfa";

CKEDITOR.plugins.add('hcard', {
    requires: 'widget',

    init: function (editor) {
        editor.widgets.add('hcard', {
            allowedContent: 'span(!h-card); a[href](!customer-short-code); span(!p-tel)',
            requiredContent: 'span(h-card)',
            pathName: 'hcard',

            upcast: function (el) {
                return el.name == 'span' && el.hasClass('h-card');
            }
        });

        // This feature does not have a button, so it needs to be registered manually.
        editor.addFeature(editor.widgets.registered.hcard);

        // Handle dropping a contact by transforming the contact object into HTML.
        // Note: All pasted and dropped content is handled in one event - editor#paste.
        editor.on('paste', function (evt) {
            var contact = evt.data.dataTransfer.getData('info');
            if (!contact) {
                return;
            }

            evt.data.dataValue =
                `<span class="h-card">` +
                `<span href="javascript:;" title="${contact.Name}" class="customer-short-code">[*${contact.Code}*]</span>` +
                `</span>`;
        });
    }
});

CKEDITOR.on('instanceReady', function () {
    // When an item in the contact list is dragged, copy its data into the drag and drop data transfer.
    // This data is later read by the editor#paste listener in the hcard plugin defined above.
    CKEDITOR.document.getById('CustomerDragFields').on('dragstart', function (evt) {
        // The target may be some element inside the draggable div (e.g. the image), so get the div.h-card.
        var target = evt.data.getTarget().getAscendant('div', true);

        // Initialization of the CKEditor 4 data transfer facade is a necessary step to extend and unify native
        // browser capabilities. For instance, Internet Explorer does not support any other data type than 'text' and 'URL'.
        // Note: evt is an instance of CKEDITOR.dom.event, not a native event.
        CKEDITOR.plugins.clipboard.initDragDataTransfer(evt);

        var dataTransfer = evt.data.dataTransfer;

        // Pass an object with contact details. Based on it, the editor#paste listener in the hcard plugin
        // will create the HTML code to be inserted into the editor. You could set 'text/html' here as well, but:
        // * It is a more elegant and logical solution that this logic is kept in the hcard plugin.
        // * You do not know now where the content will be dropped and the HTML to be inserted
        // might vary depending on the drop target.
        dataTransfer.setData('info', JSON.parse(target.data('info')));

        // You need to set some normal data types to backup values for two reasons:
        // * In some browsers this is necessary to enable drag and drop into text in the editor.
        // * The content may be dropped in another place than the editor.
        dataTransfer.setData('text/html', target.getText());

        // You can still access and use the native dataTransfer - e.g. to set the drag image.
        // Note: IEs do not support this method... :(.
        //if (dataTransfer.$.setDragImage) {
        //    dataTransfer.$.setDragImage(target.findOne('img').$, 0, 0);
        //}
    });
});

$("body").on("click", ".btn-save-form", SpamProtection(function () {
    if ($("#frmUpdateForm").valid()) {
        var btn = $(this);
        btn.buttonLoading();

        JZFormBuilder.SetNameForNestedObject($(".form-dropzone"), "column-dropitem", "Components");
        JZFormBuilder.SetNameForNestedObject($(".form-dropzone"), "column-dropzone", "Columns");
        JZFormBuilder.SetNameForNestedObject($(".form-dropzone"), "jz-custom-form-form-dropitem", "Components");

        setTimeout(function () {
            $("#frmUpdateForm").submit();
        }, 500);
    }
    return false;
}, 300));

$(".btn-preview").on("click", function () {
    var btn = $(this);
    btn.buttonLoading();
    var modalBody = $("#previewModal .modal-body");

    for (instance in CKEDITOR.instances) {
        CKEDITOR.instances[instance].updateElement();
    }

    JZFormBuilder.SetNameForNestedObject($(".form-dropzone"), "column-dropitem", "Components");
    JZFormBuilder.SetNameForNestedObject($(".form-dropzone"), "column-dropzone", "Columns");
    JZFormBuilder.SetNameForNestedObject($(".form-dropzone"), "jz-custom-form-form-dropitem", "Components");

    setTimeout(function () {

        var frmData = $("#frmUpdateForm").serialize();
        $.aPost(`/EmailForm/Preview`, frmData, function (result) {
            modalBody.html(result);
            $("#previewModal").modal("show");

            btn.buttonLoading();
        }, "html", false);
    }, 300);
});

$(function () {
    JZFormBuilder.Init("form-dropzone");
       
    var componentEl = $("#CurrentComponents");

    if (componentEl.val()) {
        var components = JSON.parse(componentEl.val());
        JZFormBuilder.RenderMultipleComponents(components);
        JZFormBuilder.SetColumnDropZone();
        JZFormBuilder.ParagraphDragable();
    }    
});

$(function () {
    $(".guide-link").each(function () {
        let link = $(this);
        link.click(function (e) {
            e.preventDefault();
            let content = link.parent().siblings(".guide-modal-content").html();
            $("#guider").find(".modal-body-content").html(content)
            $("#guider").modal("show")
        })
    })
})

function loadFile(el) {
    var file = $(el).parent().siblings(".jz-custom-form-file");
    file.trigger("click");

    file.change(function (e) {
        var fileName = e.target.files[0].name;
        var myFile = $(this);
        myFile.siblings('.jz-custom-form-file-name').val(fileName);

        var reader = new FileReader();
        reader.onload = function (e) {
            // get loaded data and render thumbnail.                        
            myFile.parent().siblings().children('.jz-custom-form-preview').attr("src", e.target.result)
            myFile.siblings('.file-value').val(e.target.result)
            //show remove button and preview img
            myFile.parent().siblings(".jz-custom-form-file-wrapper").removeClass("hidden");
        };
        // read the image file as a data URL.
        reader.readAsDataURL(this.files[0]);
    })
}

function removePreview(el) {
    var removeBtn = $(el);

    removeBtn.siblings("img").attr("src", "");
    removeBtn.parent(".jz-custom-form-file-wrapper").addClass("hidden");
    removeBtn.parent().siblings().children(".jz-custom-form-file").val(null);
    removeBtn.parent().siblings().children(".jz-custom-form-file-name").val(null);
}

function removeGuidanceItem(el) {
    var item = $(el).closest(".guidance-dropitem");
    item.remove();
}

function copyGuidanceItem(el) {
    var item = $(el).closest(".guidance-dropitem");
    var clone = item.clone();
    //$(".guidance-dropzone").append(clone);
    clone.insertAfter(item)
}