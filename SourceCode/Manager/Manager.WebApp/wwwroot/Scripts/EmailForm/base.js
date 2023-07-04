
var JZFormBuilder = {
    DropZone: null,
    DropZoneClass: "",
    Components: [],
    GenerateComponentId: function () {
        return `${new Date().getTime()}`;
    },
    Init: function (dropZoneClass = "form-dropzone") {
        JZFormBuilder.DropZoneClass = dropZoneClass;
        JZFormBuilder.DropZone = $(`.${dropZoneClass}`);

        $('.form-drag-element').draggable({
            appendTo: 'body',
            helper: 'clone'
        });

        $(`.${dropZoneClass}`).droppable({
            activeClass: 'active',
            hoverClass: 'hover',
            accept: ":not(.ui-sortable-helper, .guidance-drag-element)",
            drop: function (e, ui) {
                var dropZone = $(this);
                var dragItemTemp = ui.draggable;
                var dragItem = dragItemTemp.data("info");
                var dragType = dragItemTemp.data("type");

                if (dragType !== "form-field") {
                    return false;
                }

                dragItem.ComponentId = JZFormBuilder.GenerateComponentId();
                var newItem = JZFormBuilder.RenderComponent(dragItem);
                dropZone.append(newItem);

                JZFormBuilder.ParagraphDragable();

                JZFormBuilder.ReIndexFormComponents();
                JZFormBuilder.SetColumnDropZone("column-dropzone");
            }
        }).sortable({
            items: '.form-dropitem',
            sort: function () {
                $(this).removeClass("active");
            }
        });

        $("body").on("click", ".form-dropitem-controls-btn", function () {
            var item = $(this).closest(".form-dropitem");
            var act = $(this).data("action");
            if (act === "remove") {
                JZFormBuilder.RemoveComponent(item);
            } else if (act === "modify") {
                JZFormBuilder.ModifyComponent(item);
            }
            else if (act === "copy") {
                JZFormBuilder.CopyComponent(item);
            }
        });

        $("body").on("click", ".btn-update-component", SpamProtection(function () {
            var btn = $(this);

            if ($("#useMapping").val() == "true") {

                var selectedMappingCatName = $('option:selected', $("#ddlMappingObject")).text();
                var selectedMappingFieldName = $('option:selected', $("#ddlMappingField")).text();
                var selectedMappingRelation = $('option:selected', ".relation").text();
                var isRelative = $('option:selected', ".relation").val() > 1;

                if (selectedMappingCatName && selectedMappingFieldName) {
                    selectedMappingCatName = selectedMappingCatName.replace(/^\s+|\s+$/gm, '')
                    selectedMappingFieldName = selectedMappingFieldName.replace(/^\s+|\s+$/gm, '')
                }

                var selectedDisplayName = ""
                if (isRelative) {
                    selectedDisplayName = selectedMappingRelation + " - " + selectedMappingCatName + " - " + selectedMappingFieldName;
                }
                else {
                    selectedDisplayName = selectedMappingCatName + " - " + selectedMappingFieldName;
                }

                $("#displayName").val(selectedDisplayName);
            }

            var idx = 0;
            $(".guidance-dropitem").each(function () {
                let guidanceGroup = $(this)

                guidanceGroup.find(".guidance").each(function () {
                    let guideItem = $(this);
                    let oldName = guideItem.attr("name"),
                        newName = `Data.Guidances[${idx}].${oldName}`
                    guideItem.attr("name", newName)
                })
                idx++;
            })

            var frmData = new FormData($("#frmUpdateComponent")[0]);

            //for testing
            //var frmObj = Object.fromEntries(frmData)
            //console.log(frmObj)

            var modalCtn = $("#modalUpdateComponent");
            modalCtn.modal("hide");
            btn.buttonLoading();

            $.ajax({
                url: "/Form/UpdateComponent",
                method: "post",
                processData: false,
                contentType: false,
                data: frmData,
                success: function (model) {
                    btn.buttonLoading();
                    if (model && model.result) {
                        JZFormBuilder.BindComponentData(model.result.Data);
                    }
                }
            });

        }, 100));
    },
    SetColumnDropZone: function (dropZoneClass = "column-dropzone") {
        $(`.${dropZoneClass}`).droppable({
            activeClass: 'active',
            hoverClass: 'hover',
            accept: ":not(.ui-sortable-helper, .guidance-drag-element)",
            greedy: true,
            over: function () {
                $(".form-dropzone").removeClass("active");
                $(this).children(".column-dropzone-title").show();
            },
            out: function () {
                $(".form-dropzone").addClass("active");
                $(this).children(".column-dropzone-title").hide();
            },
            drop: function (e, ui) {
                $(".column-dropzone-title").hide();
                var dropZone = $(this);
                var colIdx = $(this).attr("col-idx");
                var dragItemTemp = ui.draggable;
                var dragItem = dragItemTemp.data("info");

                var myId = 'colCmp-' + JZFormBuilder.GenerateComponentId();
                dragItem.ComponentId = myId;

                dragItem.ColumnId = colIdx;
                var newItem = JZFormBuilder.RenderColumnComponent(dragItem);
                dropZone.append(newItem);

                JZFormBuilder.ReIndexFormComponents();
            }
        }).sortable({
            items: '.form-dropitem',
            sort: function () {
                $(JZFormBuilder.DropZone).removeClass("active");
            }
        });
    },
    SetGuidanceDropZone: function (dropZoneClass = "guidance-dropzone") {
        $(`.${dropZoneClass}`).droppable({
            accept: ":not(.ui-sortable-helper)",
            greedy: true,
            drop: function (e, ui) {
                $(".guidance-dropzone-title").hide();
                var dropZone = $(this);

                var dragItemTemp = ui.draggable;
                var dragItem = dragItemTemp.data("info");
                var newItem = JZFormBuilder.RenderGuidanceComponent(dragItem);
                dropZone.append(newItem);
            }
        }).sortable({
            items: '.guidance-dropitem',
            sort: function () {
                $(JZFormBuilder.DropZone).removeClass("active");
            }
        });
    },
    RenderColumnComponent: function (data, editable = true) {

        if (!data.ComponentId) {
            data.ComponentId = 'colCmp-' + JZFormBuilder.GenerateComponentId();
        }

        var dataType = data.Type;

        if (data.Type == "columns") {
            return false;
        }

        var dropItem = $(`<div class="form-dropitem column-dropitem form-group ${editable === false ? 'readonly' : ''}" id="comp-${data.ComponentId}" data-id="${data.ComponentId}">`);
        var dropItemLabel = $(`<div>`);

        var hasMapping = "";
        if (data.Mapping || data.HasMapping) {
            var tooltipText = data.Mapping.DisplayName;
            var tooltipIcon = "fa fa-info-circle text-small";
            hasMapping = `<span class="" data-tooltip="${tooltipText}" data-tooltip-location="right"><i class="${tooltipIcon} tooltip-icon"></i></span>`

            dropItem.append(`<input class="crtInput colInput" type="hidden" value="true" name="HasMapping">`);
            dropItem.append(`<input class="crtInput nested-input" type="hidden" value="${data.Mapping.ObjectType}" name="Mapping.ObjectType">`);
            dropItem.append(`<input class="crtInput nested-input" type="hidden" value="${data.Mapping.Name}" name="Mapping.Name">`);
            dropItem.append(`<input class="crtInput nested-input" type="hidden" value="${data.Mapping.DisplayName}" name="Mapping.DisplayName">`);
            dropItem.append(`<input class="crtInput nested-input" type="hidden" value="${data.Mapping.Relation}" name="Mapping.Relation">`);
        }

        if (data.Name) {
            dropItemLabel.append(`<p class="form-dropitem-title">${data.Name} ${hasMapping}</p>`);
        }
        else {
            data.Name = "";
        }

        var dataDescription = data.Description != null ? data.Description : "";

        var dropItemControls = $(`<div class="form-dropitem-controls">`);
        dropItemControls.append(`<a href="javascript:;" class="form-dropitem-controls-btn text-info" data-action="modify"><span class="fa fa-pencil"></span></a>`);
        dropItemControls.append(`<a href="javascript:;" class="form-dropitem-controls-btn text-danger" data-action="remove"><span class="fa fa-trash"></span></a>`);

        dropItem.append(dropItemLabel);
        dropItem.append(`<input type="hidden" value="${data.ComponentId}" name="ComponentId" class="crtInput colInput">`);
        dropItem.append(`<input type="hidden" value="${dataType}" name="Type" class="crtInput colInput">`);
        dropItem.append(`<input type="hidden" value="${data.Name}" name="Name" class="crtInput colInput">`);
        dropItem.append(`<input type="hidden" value="${data.ColumnId}" name="ColumnId" class="crtInput colInput">`);
        dropItem.append(`<input type="hidden" value="${dataDescription}" name="Description" class="crtInput colInput">`);

        if (dataType == "textarea") {
            dropItem.append(`<textarea name="Value" class="form-control form-component" />`);
        }
        else if (dataType == "email") {
            dropItem.append(`<input class="crtInput colInput form-control form-component" name="Value" type="email" />`);
        }
        else if (dataType == "radio-button") {
            dropItem.append(`<input class="crtInput colInput" type="hidden" value="${data.Inline}" name="Inline">`);

            var radioGroup = $(`<div class="form-radio">`);

            if (data.Options && data.Options.length > 0) {

                for (let i = 0; i < data.Options.length; i++) {
                    dropItem.append(`<input type="hidden" value='${data.Options[i].Value}' name="Options[${i}].Value"  class="crtInput nested-input">`);
                    dropItem.append(`<input type="hidden" value='${data.Options[i].Text}' name="Options[${i}].Text" class="crtInput nested-input">`);
                    if (data.Inline) {
                        radioGroup.append(`<div class="form-check form-check-inline"><label class="form-check-label  d-inline-flex"><input type="radio" value="${data.Options[i].Value}" class="form-component form-check-input" name="Value">${data.Options[i].Text}</label></div>`);
                    }
                    else {
                        radioGroup.append(`<div class="form-check meo"><label class="form-check-label"><input type="radio" value="${data.Options[i].Value}" class="form-component form-check-input" name="Value">${data.Options[i].Text}</label></div>`);
                    }
                }
                dropItem.append(radioGroup);
            }
        }
        else if (dataType == "select") {
            var selectGroup = $(`<div class="form-select">`);
            let select = $(`<select class="form-control form-control-sm">`);

            if (data.Options && data.Options.length > 0) {

                for (let i = 0, length = data.Options.length; i < length; i++) {
                    let opt = data.Options[i];
                    dropItem.append(`<input class="crtInput nested-input" type="hidden" value='${opt.Value}' name="Options[${i}].Value">`);
                    dropItem.append(`<input class="crtInput nested-input" type="hidden" value='${opt.Text}' name="Options[${i}].Text">`);

                    select.append(`<option value='${opt.Value}' name="Value">${opt.Text}</option>`)
                }
                selectGroup.append(select);
                dropItem.append(selectGroup);
            }
        }
        else if (dataType == "date") {
            dropItem.append(`<input class="crtInput colInput form-control form-control-sm form-component" name="Value" type="date" />`);
        }
        else if (dataType == "password") {
            dropItem.append(`<input class="crtInput colInput form-control form-component" name="Value" type="password" />`);
        }
        else {
            dropItem.append(`<input class="crtInput colInput form-control form-control-sm form-component" name="Value" type="text" />`);
            if (data.Description) {
                dropItem.append(`<div class="jz-custom-form-guide-text">${dataDescription}</span>`)
            }
        }

        if (data.Guidances && data.Guidances.length > 0) {
            let guideModalContent = $(`<div class="guide-modal-content hidden">`)

            for (let i = 0; i < data.Guidances.length; i++) {
                let guide = data.Guidances[i]

                if (!guide.Value)
                    continue;

                dropItem.append(`<input type="hidden" class="crtInput nested-input" name="Guidances[${i}].Name" value="${guide.Name}">`)
                dropItem.append(`<input type="hidden" class="crtInput nested-input" name="Guidances[${i}].Type" value="${guide.Type}">`)
                dropItem.append(`<input type="hidden" class="crtInput nested-input" name="Guidances[${i}].Value" value="${guide.Value}">`)

                if (guide.Type == "image") {
                    guideModalContent.append(`<div class="jz-custom-form-file-wrapper mb-2"><img class="img-thumbnail jz-custom-form-preview" src="${guide.Value}"></div>`)
                    continue;
                }

                if (guide.Type == "paragraph") {
                    guideModalContent.append(`<div class="jz-custom-form-paragraph-wrapper mb-2">${guide.Value}</div>`)
                }
            }

            let guideLinkContainer = $(`<div style="font-size:12px">`)
            let guideLink = $(`<a class="guide-link text-info"  href="#">Xem thêm</a>`);
            guideLinkContainer.append(guideLink)
            dropItem.append(guideLinkContainer)

            dropItem.append(guideModalContent)
        }

        if (editable) {
            dropItem.append(dropItemControls);
        }

        dropItem.attr("data-info", JSON.stringify(data));

        JZFormBuilder.Components = JZFormBuilder.Components.filter(function (el) {
            return (el.ComponentId !== `${data.ComponentId}`);
        });

        return dropItem;
    },
    RenderComponent: function (data, editable = true) {
        if (!data.ComponentId) {
            data.ComponentId = JZFormBuilder.GenerateComponentId();
        }

        var dataType = data.Type;

        var colCtn = "";
        if (dataType == "columns") {
            colCtn = "column-dropzone-wrapper"
        }

        var dropItem = $(`<div data-repeater-item class="jz-custom-form-form-dropitem form-dropitem form-group ${colCtn} ${editable === false ? 'readonly' : ''}" id="comp-${data.ComponentId}" data-id="${data.ComponentId}">`);
        var dropItemLabel = $(`<div>`);

        var hasMapping = "";
        if (data.Mapping) {
            var tooltipText = data.Mapping.DisplayName;
            var tooltipIcon = "fa fa-info-circle text-small";

            hasMapping = `<span class="" data-tooltip="${tooltipText}" data-tooltip-location="right"><i class="${tooltipIcon} tooltip-icon"></i></span>`

            dropItem.append(`<input class="crtInput nested-input" type="hidden" value="${data.Mapping.ObjectType}" name="Mapping.ObjectType">`);
            dropItem.append(`<input class="crtInput nested-input" type="hidden" value="${data.Mapping.Name}" name="Mapping.Name">`);
            dropItem.append(`<input class="crtInput nested-input" type="hidden" value="${data.Mapping.DisplayName}" name="Mapping.DisplayName">`);
            dropItem.append(`<input class="crtInput nested-input" type="hidden" value="${data.Mapping.Relation}" name="Mapping.Relation">`);
            dropItem.append(`<input class="crtInput normInput" type="hidden" value="true" name="HasMapping">`);
        }

        if (data.Name) {
            dropItemLabel.append(`<p class='form-dropitem-title'>${data.Name} ${hasMapping}</p>`);
        }
        else {
            data.Name = "";
        }

        var dataDescription = data.Description != null ? data.Description : "";

        var dropItemControls = $(`<div class="form-dropitem-controls">`);        
        dropItemControls.append(`<a href="javascript:;" class="form-dropitem-controls-btn text-danger" data-action="remove"><span class="fa fa-trash"></span></a>`);
        dropItemControls.append(`<a href="javascript:;" class="form-dropitem-controls-btn text-success" data-action="copy"><span class="fa fa-copy"></span></a>`);

        dropItem.append(dropItemLabel);
        dropItem.append(`<input class="crtInput normInput" type="hidden" value="${data.ComponentId}" data-name="ComponentId">`);
        dropItem.append(`<input class="crtInput normInput" type="hidden" value="${data.Type}" data-name="Type">`);
        dropItem.append(`<input class="crtInput normInput" type="hidden" value="${data.Name}" data-name="Name">`);
        dropItem.append(`<input class="crtInput normInput" type="hidden" value="${dataDescription}" data-name="Description">`);

        //dropItem.append(`<input type="hidden" class="form-dropitem-idx" value="0" name="SortOrder">`);

        if (dataType == "textarea") {
            dropItem.append(`<textarea data-provide="markdown" id="${data.ComponentId}" data-name="Value" class="form-control crtInput normInput form-component data-dragable summernote summernote-new resizable" rows="5">${(data.Value) ? data.Value : ""}</textarea >`);
        }
        else if (dataType == "email") {
            dropItem.append(`<input class="form-control form-component crtInput normInput" data-name="Value" type="email" />`);
        }
        else if (dataType == "radio-button") {
            dropItem.append(`<input class="crtInput normInput" type="hidden" value="${data.Inline}" name="Inline">`);
            var radioGroup = $(`<div class="form-radio">`);

            if (data.Options && data.Options.length > 0) {

                for (let i = 0; i < data.Options.length; i++) {
                    dropItem.append(`<input class="crtInput nested-input" type="hidden" value='${data.Options[i].Value}' name="Options[${i}].Value">`);
                    dropItem.append(`<input class="crtInput nested-input" type="hidden" value='${data.Options[i].Text}' name="Options[${i}].Text">`);

                    if (data.Inline) {
                        radioGroup.append(`<div class="form-check form-check-inline"><label class="form-check-label  d-inline-flex"><input type="radio" value="${data.Options[i].Value}" class="form-component form-check-input" name="Value">${data.Options[i].Text}</label></div>`);
                    }
                    else {
                        radioGroup.append(`<div class="form-check meo"><label class="form-check-label"><input type="radio" value="${data.Options[i].Value}" class="form-component form-check-input" name="Value">${data.Options[i].Text}</label></div>`);
                    }
                }
                dropItem.append(radioGroup);
            }
        }
        else if (dataType == "select") {
            var selectGroup = $(`<div class="form-select">`);
            let select = $(`<select class="form-control form-control-sm">`);

            if (data.Options && data.Options.length > 0) {

                for (let i = 0, length = data.Options.length; i < length; i++) {
                    let opt = data.Options[i];
                    dropItem.append(`<input class="crtInput nested-input" type="hidden" value='${opt.Value}' name="Options[${i}].Value">`);
                    dropItem.append(`<input class="crtInput nested-input" type="hidden" value='${opt.Text}' name="Options[${i}].Text">`);

                    select.append(`<option value='${opt.Value}' name="Value">${opt.Text}</option>`)
                }
                selectGroup.append(select);
                dropItem.append(selectGroup);
            }
        }
        else if (dataType == "date") {
            dropItem.append(`<input class="crtInput normInput form-control form-control-sm form-component" name="Value" type="date" />`);
        }
        else if (dataType == "password") {
            dropItem.append(`<input class="crtInput normInput form-control form-control-sm form-component" name="Value" type="password" />`);
        }
        else if (dataType == "columns") {
            let columnContainer = $(`<div data-repeater-list="Columns" class="form-group column-dropzone-container row"/>`);

            if (data.Columns && data.Columns.length > 0) {

                for (let i = 0; i < data.Columns.length; i++) {
                    dropItem.append(`<input class="crtInput nested-input" type="hidden" value='${data.Columns[i].Width}' name="Columns[${i}].Width">`)

                    let myColId = `colNo.${i}`;
                    dropItem.append(`<input type="hidden" class="crtInput nested-input" value='${myColId}' name="Columns[${i}].ColumnId">`)
                    let col = JZFormBuilder.RenderMultipleColumnComponents(data.Columns[i], myColId, editable);
                    columnContainer.append(col);
                }
                dropItem.append(columnContainer);
            }

            dropItemControls.append(`<a href="javascript:;" class="form-dropitem-controls-btn text-info" data-action="modify"><span class="fa fa-pencil"></span></a>`);
        }
        else {
            dropItem.append(`<input class="crtInput normInput form-control form-control-sm form-component" name="Value" type="text" />`);
        }

        if (data.Description) {
            dropItem.append(`<div class="jz-custom-form-guide-text">${dataDescription}</span>`)
        }

        if (data.Guidances && data.Guidances.length > 0 && dataType != "columns") {
            let guideModalContent = $(`<div class="guide-modal-content hidden">`)

            for (let i = 0; i < data.Guidances.length; i++) {
                let guide = data.Guidances[i]

                if (!guide.Value)
                    continue;

                dropItem.append(`<input type="hidden" class="crtInput nested-input" name="Guidances[${i}].Name" value="${guide.Name}">`)
                dropItem.append(`<input type="hidden" class="crtInput nested-input" name="Guidances[${i}].Type" value="${guide.Type}">`)
                dropItem.append(`<input type="hidden" class="crtInput nested-input" name="Guidances[${i}].Value" value="${guide.Value}">`)

                if (guide.Type == "image") {
                    guideModalContent.append(`<div class="jz-custom-form-file-wrapper mb-2"><img class="img-thumbnail jz-custom-form-preview" src="${guide.Value}"></div>`)
                    continue;
                }

                if (guide.Type == "paragraph") {
                    guideModalContent.append(`<div class="jz-custom-form-paragraph-wrapper mb-2">${guide.Value}</div>`)
                }
            }

            let guideLinkContainer = $(`<div style="font-size:12px">`)
            let guideLink = $(`<a class="guide-link text-info"  href="#">Xem thêm</a>`);
            guideLinkContainer.append(guideLink)
            dropItem.append(guideLinkContainer)

            dropItem.append(guideModalContent)
        }

        if (editable) {
            //dropItem.append(dropItemControls);
        }


        JZFormBuilder.Components = JZFormBuilder.Components.filter(function (el) {
            return (el.ComponentId !== `${data.ComponentId}`);
        });

        dropItem.attr("data-info", JSON.stringify(data));

        return dropItem;
    },
    RenderGuidanceComponent: function (data, editable = true) {
        var dropItem = $(`<div class="guidance-dropitem">`);

        var dataType = data.Type;

        dropItem.append(`<input class="guidance" type="hidden" name="Type" value="${dataType}" />`)

        if (dataType == "paragraph") {
            dropItem.append(`<input type="hidden" class="guidance" value="My paragraph" name="Name"/>`)
            dropItem.append(`<textarea class="guidance form-control paragraph" placeholder="My paragraph..." name="Value"></textarea>`)
        }

        if (dataType == "image") {
            let imagePreviewWrapper = `<div class="guidance jz-custom-form-file-wrapper hidden"><img class="img-thumbnail jz-custom-form-preview"><i class="fa fa-times img-remove" onclick="removePreview(this)"></i></div>`

            let file = `<input type="file" class="hidden jz-custom-form-file"/>`,
                fileName = `<input type="text" class="guidance form-control m-input form-control-sm jz-custom-form-file-name" readonly placeholder="No file chosen" name="Name">`,
                fileValue = `<input type="hidden" class="guidance file-value" name="Value">`
            browser = `<div class="input-group-append"><button type="button" class="browse btn btn-info btn-sm" onclick="loadFile(this)">Browse</button></div>`;

            let imageFileWrapper = `<div class="w-50 input-group" style="min-width:200px;">${file}${fileName}${fileValue}${browser}</div>`

            let imageContainer = `<div class="">${imagePreviewWrapper}${imageFileWrapper}</div>`
            dropItem.append(imageContainer)
        }

        var dropItemControlPanel = $(`<div class="guidance-dropitem-control-panel">`);

        var remove = `<div class="guidance-dropitem-control"><a href="#" class="guidance-dropitem-control-btn guidance-dropitem-control-btn-remove" data-action="remove" onclick="removeGuidanceItem(this)"><span class="fa fa-close"></span></a></div>`
        var copy = `<div class="guidance-dropitem-control"><a href="#" class="guidance-dropitem-control-btn guidance-dropitem-control-btn-copy" data-action="copy" onclick="copyGuidanceItem(this)"><span class="fa fa-copy"></span></a></div>`

        dropItemControlPanel.append(remove)
        dropItemControlPanel.append(copy)

        if (editable) {
            dropItem.append(dropItemControlPanel)
        }

        dropItem.attr("data-info", JSON.stringify(data))

        return dropItem;
    },
    BindComponentData: function (data, componentId = null) {
        if (!componentId) {
            componentId = data.ComponentId;
        }

        console.log(data.ComponentId)
        var component = $(`#comp-` + componentId);

        var renderedItem;
        if (data.ComponentId.includes("colCmp")) {
            renderedItem = JZFormBuilder.RenderColumnComponent(data);
        }
        else {
            renderedItem = JZFormBuilder.RenderComponent(data);
        }

        component.html(renderedItem.html());

        JZFormBuilder.SetColumnDropZone("column-dropzone");

        $(".guide-link").click(function (e) {
            e.preventDefault();
            let link = $(this);

            let content = link.parent().siblings(".guide-modal-content").html();
            $("#guider").find(".modal-body-content").html(content)
            $("#guider").modal("show")
        })
    },
    ModifyComponent: function (el) {

        var myForm = $(`<form>`);

        let myFakeEl = el.clone();
        JZFormBuilder.SetNameForNestedObject(myFakeEl, "column-dropitem", "Components");
        JZFormBuilder.SetNameForNestedObject(myFakeEl, "column-dropzone", "Columns");
        JZFormBuilder.SetNameForFlatObject(myFakeEl, "Data");

        myForm.append(myFakeEl.html())

        var frmData = new FormData(myForm[0])
        //console.log(Object.fromEntries(frmData))

        var modalCtn = $("#modalUpdateComponent");
        modalCtn.find(".modal-body-content").html("");

        $.ajax({
            url: "/Form/ModifyComponent",
            method: "post",
            processData: false,
            contentType: false,
            data: frmData,
            success: function (result) {
                modalCtn.find(".modal-body-content").html(result);
                modalCtn.modal("show");

                $('.guidance-drag-element').draggable({
                    appendTo: 'body',
                    helper: "clone"
                });

                JZFormBuilder.SetGuidanceDropZone();
            }
        });
    },
    RemoveComponent: function (el) {
        var compId = el.data("id");
        el.detach();

        $(`#comp-${compId}`).remove();

        JZFormBuilder.Components = JZFormBuilder.Components.filter(function (el) {
            return (el.ComponentId !== `${compId}`);
        });
    },
    RemoveComponentById: function (compId) {
        $(`#comp-${compId}`).remove();
    },
    CopyComponent: function (el) {
        var compId = el.data("id");
        //el.detach();
        var newId = JZFormBuilder.GenerateComponentId();
        var newItem = $(`#comp-${compId}`).clone();

        newItem.find(".form-component").addClass("new-item");

        var dataObj = newItem.data("info");
        dataObj.ComponentId = newId;

        newItem.attr("id", `comp-${newId}`);
        newItem.attr("data-id", `${newId}`);
        newItem.find(".form-component").each(function () {
            $(this).attr("id", newId);
        });

        newItem.find("input[data-name=ComponentId]").each(function () {
            let myInput = $(this);
            let myInputValue = myInput.val();

            if (myInputValue.includes("colCmp")) {
                myInput.val(`colCmp-${newId}`);

                let newValue = myInput.val();
                let myInputParent = myInput.closest(".column-dropitem");
                let myInputParentObj = myInputParent.data("info");

                myInputParentObj.ComponentId = newValue;
                myInputParent.attr("id", `comp-${newValue}`);
                myInputParent.attr("data-id", `${newValue}`);
                myInputParent.attr("data-info", JSON.stringify(myInputParentObj));
            }
            else {
                myInput.val(`${newId}`);
            }

            newId++;
        })

        newItem.attr("data-info", JSON.stringify(dataObj));
        newItem.insertAfter(el);

        JZFormBuilder.Components = JZFormBuilder.Components.filter(function (el) {
            return (el.ComponentId !== `${compId}`);
        });

        newItem.find(".data-dragable").each(function () {
            var editor = $(this);

            newItem.find(".cke_ltr").remove();
            editor.removeClass("replaced");
            var editorId = editor.attr("id");

            CKEDITOR.replace(editorId, {
                extraPlugins: 'hcard'
            });

            //var value = CKEDITOR.instances[editorId].getData();

            //CKEDITOR.instances[editorId].setData(value);
            //console.log(value);
        });

        //JZFormBuilder.ParagraphDragable();
    },
    GetComponents: function () {
        return JZFormBuilder.Components;
    },
    GetJsonComponents: function () {
        var components = JZFormBuilder.GetComponents();
        return JSON.stringify(components);
    },
    RenderMultipleComponents: function (components) {
        if (components && components.length > 0) {
            components.forEach(t => {
                var item = JZFormBuilder.RenderComponent(t);
                JZFormBuilder.DropZone.append(item);
            });
        }
    },
    RenderMultipleColumnComponents: function (column, columnId, editable = true) {

        let col = $(`<div class="col-${column.Width} column-dropzone column-dropzone-style" col-idx="${columnId}">`)

        if (editable == false) {
            col = $(`<div class="col-${column.Width} column-dropzone" col-idx="${columnId}">`)
        }

        col.append(`<div class="column-dropzone-title text-center align-middle"><span> Drop your item to the column </span></div>`)

        if (column.ComponentTemplate) {
            column.Components = JSON.parse(column.ComponentTemplate)
        }

        if (column.Components && column.Components.length > 0 && typeof column.Components === "object") {
            column.Components.forEach(c => {
                var item = JZFormBuilder.RenderColumnComponent(c, editable);
                item.children("input[name=ColumnId]").first().val(columnId);
                col.append(item)
            });
        }

        return col;
    },
    RenderMultipleComponentsToHtml: function (components) {
        var container = $("<div>");
        if (components && components.length > 0) {
            components.forEach(t => {
                var item = JZFormBuilder.RenderComponent(t);
                container.append(item);
            });
        }

        return container;
    },
    RenderForm: function (formInfo) {
        var container = $(`<div data-repeater-list="Components">`);
        if (formInfo.Components && formInfo.Components.length > 0) {

            formInfo.Components.forEach(t => {
                if (t.TemplateComponents && t.TemplateComponents.length > 0) {
                    t.TemplateComponents.forEach(c => {
                        var item = JZFormBuilder.RenderComponent(c, false);
                        container.append(item);
                    });
                }
            });
        }

        return container;
    },
    ReIndexFormComponents: function () {
        let stt = 0;
        $(".form-dropitem").each(function () {
            var item = $(this);
            item.find(".form-dropitem-idx").val(stt);
            stt++;
        });      

        $(".summernote").each(function () {
            autosize($(this));
            autosize.update($(this));
        });

        //$(".summernote-new").each(function () {
        //    var note = $(this);

        //    note.summernote({
        //        followingToolbar: false,
        //        placeholder: 'You can modify the text here !!!',
        //        height: 150,
        //        toolbar: [
        //            ['style', ['style']],
        //            ['font', ['bold', 'underline', 'clear']],
        //            ['fontname', ['fontname']],
        //            ['color', ['color']],
        //            ['para', ['ul', 'ol', 'paragraph']],
        //            ['table', ['table']],
        //            ['insert', ['link']],
        //            ['view', ['fullscreen', 'codeview', 'help']],
        //        ]
        //    });

        //    note.removeClass("summernote-new");
        //});
    },
    SetNameForNestedObject: function (scope, ctnClass, ctnName) {
        let idx = 0;
        scope.find(`.${ctnClass}`).each(function () {
            let ctn = $(this);
            let sibs = ctn.siblings(`.${ctnClass}`).length;

            ctn.find(".crtInput").each(function () {
                let el = $(this);
                let name = el.data("name");
                el.attr("name", "");
                console.log(el.attr("name"));
                if (name) {
                    el.attr("name", `${ctnName}[${idx}].${name}`);
                }
            });

            if (idx == sibs) {
                idx = 0;
            }
            else {
                idx++;
            }
        });
    },
    SetNameForFlatObject: function (scope, ctnName) {
        scope.find(`.crtInput`).each(function () {
            let el = $(this);
            let name = el.attr("name");
            if (name) {
                el.attr("name", `${ctnName}.${name}`);
            }
        });
    },
    FormToArray: function (myForm) {
        let group = {};
        let result = [];

        if (!myForm) {
            return result;
        }

        let myArray = myForm.serializeArray();

        for (let i = 0; i < myArray.length; i++) {
            //myArray[i].value = myArray[i].value.replace(/^\s+|\s+$/gm, '');
            myArray[i].value = myArray[i].value.trim();
            let groupName = myArray[i].name;
            /*
                        if (groupName.includes("Value"))
                            groupName = "Value";*/

            if (group.hasOwnProperty(groupName)) {
                result.push(group);
                group = {};
            }

            if (!group.hasOwnProperty(groupName)) {
                group[groupName] = myArray[i].value;
            }

            if (i == myArray.length - 1) {
                result.push(group);
            }
        }
        return result;
    },
    BuildTemporaryForm: function (containerEl, inputClass = "", exception = "") {
        let form = $("<form>");
        let myInput = `input.${inputClass}`

        if (inputClass == "") {
            myInput = "input"
        }

        if (exception == "") {
            containerEl.find(`${myInput}`).each(function () {
                let input = $(this).clone();
                form.append(input);
            })
        }
        else {
            containerEl.find(`${myInput}`).not(`.${exception}`).each(function () {
                let input = $(this).clone();
                form.append(input);
            })
        }

        if (!form.html()) {
            form = null;
        }

        return form;
    },
    FormToObject: function (frmData) {
        let obj = Object.fromEntries(frmData)
        return obj;
    },
    ParagraphDragable: function () {
        $(".form-component.data-dragable").each(function () {
            var el = $(this);
            if (!el.hasClass("replaced")) {
                // Initialize the editor with the hcard plugin.
                CKEDITOR.replace(el.attr("id"), {
                    extraPlugins: 'hcard'
                });

                el.addClass("replaced");
            }

            //$(".customer-drag-element").draggable({
            //    appendTo: "body",
            //    helper: "clone",
            //    cursor: "select",
            //    revert: "invalid"
            //});

            //initDroppable(el);     
        });

        function initDroppable($elements) {
            $elements.droppable({
                hoverClass: "textarea",
                accept: ":not(.ui-sortable-helper)",
                drop: function (event, ui) {
                    var dragItemTemp = ui.draggable;
                    var dragItem = dragItemTemp.data("info");
                    var dropText;
                    dropText = dragItem;
                    
                    var droparea = $elements.get(0);

                    tinymce.activeEditor.setContent("123", { format: 'raw' });

                    var range1 = droparea.selectionStart;
                    var val = droparea.value;
                    var str1 = val.substring(0, range1);
                    var str3 = val.substring(range1, val.length);
                    droparea.value = str1 + dropText + str3;
                }
            });
        }
    },
    AddFormFieldByName: function (name) {
        $(".form-drag-element").each(function () {
            var itemName = $(this).data("name");
            if (itemName === name) {
                var dropZone = JZFormBuilder.DropZone;
                var dragItem = $(this).data("info");

                dragItem.ComponentId = JZFormBuilder.GenerateComponentId();
                var newItem = JZFormBuilder.RenderComponent(dragItem);
                dropZone.append(newItem);

                JZFormBuilder.ParagraphDragable();

                JZFormBuilder.ReIndexFormComponents();
                JZFormBuilder.SetColumnDropZone("column-dropzone");
            }
        });
    }
};
