Dropzone.autoDiscover = false;
Dropzone.autoProcessQueue = false;

//var myVideosDropzone = null;
var myAttachmentsDropzone = null;

var dropZonesArr = [];
var uploadQueueDone = 0;
var dzUploadQueue = 0;


$("body").on("click", ".btn-course-submit", function () {
    SubmitCourse();
})

function SubmitCourse() {
    var isValid = true;
    isValid = $("#frmCourse").valid();

    if (!isValid) {
        var firstError = $("#frmCourse").find(".input-validation-error").first();
        GoToElement(firstError);
        firstError.focus();

        isValid = false;
        return false;
    }

    if (isValid) {
        $(".attachment-queue").each(function (e) {
            var el = $(this)
            if (el.val() == 0) {
                queueDone = false;
                //Finish upload queue
                dropZonesArr.forEach(function (el) {
                    if (el.element.id.includes("dropzoneAttachments")) {
                        
                        var newAttachmentsUpload = el.getFilesWithStatus(Dropzone.ADDED);

                        if (newAttachmentsUpload.length > 0) {
                            dzUploadQueue++;
                            el.enqueueFiles(newAttachmentsUpload);
                        }
                    }                    
                })
            }
        })

        $(".video-queue").each(function (e) {
            var el = $(this)
            if (el.val() == 0) {               
                queueDone = false;
                //Finish upload queue
                dropZonesArr.forEach(function (el) {
                    if (el.element.id.includes("dropzoneVideos")) {
                        
                        var newVideosUpload = el.getFilesWithStatus(Dropzone.ADDED);

                        if (newVideosUpload.length > 0) {
                            dzUploadQueue++;
                            el.enqueueFiles(newVideosUpload);
                        }
                    }                    
                })
            }
        })

        if (uploadQueueDone == dzUploadQueue) {
            //console.log($("#frmCourse").serializeArray())
            $("#frmCourse").submit();
            
            return false;
        }
    }
}

$("body").on("click", ".video-button", function (e) {
    e.stopPropagation();
    e.preventDefault();
})

$("body").on("click", ".attachment-button", function (e) {
    e.stopPropagation();
    e.preventDefault();
})

$("body").on("click", ".ip-lesson-name", function (e) {
    e.stopPropagation();
    e.preventDefault();
})

$("body").on("click", ".btn-repeater-remove", function (e) {
    e.stopPropagation();
    e.preventDefault();
})

$("body").on("click", ".btn-lesson-remove", function () {
    var el = $(this);
    var accordionEl = el.closest(".rpt-item-container");
    var dzAttachmentId = accordionEl.find(".dropzone-attachment-area").attr("id");

    dropZonesArr.forEach(function (e, index) {
        if (e.element.id == dzAttachmentId) {
            dropZonesArr.splice(index, 1);
        }
    })

    el.parent().closest(".rpt-item-container").remove();
    LessonRptUpdateCounter();
})

function LessonRptUpdateCounter() {
    var total = 0;
    $(".lesson-idx").each(function () {
        total++;
        var el = $(this);
        el.html(total)

        var currentAccordion = el.closest(".rpt-item-container");
        var accordionHead = currentAccordion.find(".accordion-lesson-head");
        var accordionBody = currentAccordion.find(".accordion-lesson-body");

        accordionHead.attr("href", `#m_accordion_5_item_${total}_body`);
        accordionBody.attr("id", `m_accordion_5_item_${total}_body`);
    })
}