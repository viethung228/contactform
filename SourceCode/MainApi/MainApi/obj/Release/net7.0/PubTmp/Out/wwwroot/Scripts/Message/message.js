$("#setQuestionsRpt").repeater({
    initEmpty: !1,
    show: function () {
        $(this).slideDown();

        SetQuestionRptUpdateCounter();
    },

    hide: function (e) {
        $(this).remove();

        var delItem = parseInt($(this).find(".hdCurrentAddId").val());
        if (delItem > 0) {
            var cv = $("#deleted_set_questions").val();
            if (cv === "") {
                cv = delItem;
            }
            else {
                cv = cv + "," + delItem;
            }

            $("#deleted_set_questions").val(cv);
            if ($("#deleted_set_questions").val() === ",")
                $("#deleted_set_questions").val("");
        }

        SetQuestionRptUpdateCounter();
        MessageControl.SetExceptSetQuestions();
        //QuestionInputCounter();
    }
});

function SetQuestionRptUpdateCounter() {
    var total = 0;
    $(".add-idx").each(function () {
        total++;
        $(this).html(total);
    });
}

$("body").on("click", ".btn-select-set-question", function () {
    var frmData = [];
    frmData.push({ name: 'Page', value: 1 });
    frmData.push({ name: 'Keyword', value: "" });
    frmData.push({ name: 'ExceptIds', value: $("#ExceptSetQuestions").val() });

    $.aPost("/Message/GetSetQuestions", frmData, function (result) {
        if (result.success) {
            {
                $("#myModalContent").html(result.html);
                $('#myModal').modal('show');
            }
        }
        else {
            $.showErrorMessage(result.message, '')
        }
    }, "json", true);
});

var MessageControl = {
    SetQuestionSelectProcess: function (setQuestionId, label, description, questionCount) {
        if (parseInt(setQuestionId) > 0) {
            //QuestionRptUpdateCounter();

            var el = `<tr data-repeater-item="" class="rpt-item-container tr-question-item rpt-question-item-0" data-idx="0">
                                                            <input type="hidden" value="111" name="Questions[0][id]" class="hdCurrentAddId">
                                                            <input type="hidden" class="ip-set-question" name="SetQuestionsId" value="${setQuestionId}" />
                                                            <td class="text-left">${label}</td>
                                                            <td class="text-left">${description}</td>
                                                            <td class="text-right">${questionCount}</td>
                                                            <td class="text-center">
                                                                <a data-repeater-delete="" href="javascript:;" class="text-danger btn-repeater-remove" title="削除">
                                                                    <i class="fa fa-remove"></i>
                                                                </a>
                                                            </td>
                                                        </tr>`;
            $("#selected-set-question-list").append(el);

            SetQuestionRptUpdateCounter();
            //QuestionInputCounter();

        }

        $('#myModal').modal('hide');
    },
    ClearSetQuestion: function () {
        $("#selected-set-question-list").html("");
    },
    GetSetQuestionList: function (idx, searchExec = "") {
        var myDiv = $("#set-question-render");
        var frmData = $("#frmSearchSetQuestions").serializeArray();
        frmData.push({ name: 'Page', value: idx });
        frmData.push({ name: 'SearchExec', value: searchExec });

        $.aPost("/Message/GetSetQuestionsJson", frmData, function (result) {
            if (result.success) {
                myDiv.html(result.html);
            }
        }, "json", true);
    },
    SetExceptSetQuestions: function () {
        $("#ExceptSetQuestions").val("");
        var ExceptSetQuestions = $("#ExceptSetQuestions").val();
        $(".ip-set-question").each(function () {
            if (ExceptSetQuestions != null && ExceptSetQuestions != "") {
                ExceptSetQuestions = ExceptSetQuestions + "," + $(this).val();
            } else {
                ExceptSetQuestions += $(this).val();
            }
        })

        $("#ExceptSetQuestions").val(ExceptSetQuestions);
    },
};

$("body").on("click","#search_question", function () {
    MessageControl.GetSetQuestionList(1);
});