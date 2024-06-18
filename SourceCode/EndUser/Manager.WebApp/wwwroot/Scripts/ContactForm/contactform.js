
let listFormId = localStorage.getItem("items")
    ? JSON.parse(localStorage.getItem("items"))
    : [];

window.onload = (event) => {
    localStorage.removeItem("items");
    $("input:checkbox[name='contactForm']").each(function () {
        if (listFormId.includes($(this).val())) {
            $(this).attr("checked", "checked");
        }
    });
    $("#selectedCSVDownload").prop("disabled", true);

    $(".m-datatable__pager-link-number").each(function () {
        $(this).attr("href", "javascript:");
    });
    contactForm();
    $("input:checkbox[name='contactForm']:checked").each(function () {
        $(this).prop("checked", false);
    });
};
$("input").change(function () {
    $(this).val(ConvertFullWidthNumber($(this).val()));
});
function isChecked() {
    listFormId = localStorage.getItem("items")
        ? JSON.parse(localStorage.getItem("items"))
        : [];
    if (listFormId.length > 0) {
        $("input:checkbox[name='contactForm']").each(function () {
            if (listFormId.includes($(this).val())) {
                $(this).attr("checked", "checked");
            }
        });
        $("#selectedCSVDownload").prop(
            "disabled",
            listFormId.length > 0 ? false : true
        );
        $("#unselectall").prop(
            "disabled",
            listFormId.length > 0 ? false : true
        );
    }
}

function contactForm() {
    $(".m-datatable__pager-link-number").on("click", function (event) {
        event.preventDefault();
        var keyword = $("#Keyword").val();
        $.aGet(
            "/ContactForm/Pagination",
            { Page: $(this).text(), Keyword: keyword },
            function (result) {
                if (result.html) {
                    $("#tableContactForm").html(result.html);
                    contactForm();
                    isChecked();
                }
            }
        );
    });

    $("#bigCheckBox").click(function () {
        arrDel = [];
        $(".childCheckBox").prop("checked", $(this).prop("checked"));
        $("input:checkbox[name='contactForm']:checked").each(function () {
            if (!listFormId.includes($(this).val())) {
                listFormId.push($(this).val());
                localStorage.setItem("items", JSON.stringify(listFormId));
            }
        });
        $("input:checkbox[name='contactForm']:not(:checked)").each(function () {
            listFormId = listFormId.filter((item) => item !== $(this).val());
            localStorage.setItem("items", JSON.stringify(listFormId));
        });
        $("#unselectall").prop(
            "disabled",
            listFormId.length > 0 ? false : true
        );
    });
    $(".childCheckBox").click(function () {
        $("input:checkbox[name='contactForm']:checked").each(function () {
            if (!listFormId.includes($(this).val())) {
                listFormId.push($(this).val());
                localStorage.setItem("items", JSON.stringify(listFormId));
            }
        });
        $("input:checkbox[name='contactForm']:not(:checked)").each(function () {
            listFormId = listFormId.filter((item) => item !== $(this).val());
            localStorage.setItem("items", JSON.stringify(listFormId));
        });

        $("#selectedCSVDownload").prop(
            "disabled",
            listFormId.length > 0 ? false : true
        );
        $("#unselectall").prop(
            "disabled",
            listFormId.length > 0 ? false : true
        );
    });
    $("#selectedCSVDownload").click(function () {
        if (listFormId.length > 0) {
            ExportContactFormByListFormIdCSV(listFormId);
        }
    });

    $(".listing-item").click(function (ev) {
        var targetClass = ev.target.className;
        var tagName = ev.target.tagName.toLowerCase();
        if (
            tagName === "a" ||
            tagName === "i" ||
            tagName === "img" ||
            targetClass == "childCheckBox"
        ) {
            return true;
        } else {
            ev.preventDefault();

            var link = document.createElement("a");
            link.href = $(this).find(".DetailLink").val();
            document.body.appendChild(link);
            link.click();

            return false;
        }
    });
}

