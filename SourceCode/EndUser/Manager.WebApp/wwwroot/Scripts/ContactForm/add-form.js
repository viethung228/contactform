window.onload = (event) => {
    $('label[for="first-pickup-1"]').click();
    $('label[for="sec-pickup-1"]').click();
    //date of create
    $("#ContactForm_CreatedDate").val(formatDate(new Date()));
    $("#CreatedDate-Year").val(
        GetJapaneseDate($("#ContactForm_CreatedDate")).split(";")[0]
    );
    $("#CreatedDate-Month").val(
        GetJapaneseDate($("#ContactForm_CreatedDate")).split(";")[1]
    );
    $("#CreatedDate-Day").val(
        GetJapaneseDate($("#ContactForm_CreatedDate")).split(";")[2]
    );
};
function formatDate(date) {
    var d = new Date(date),
        month = "" + (d.getMonth() + 1),
        day = "" + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = "0" + month;
    if (day.length < 2) day = "0" + day;

    return [year, month, day].join("-");
}
function GetJapaneseDate(selector) {
    var tempYear = "";
    var tempMonth = "";
    var tempDay = "";
    var count = 0;
    var getDate = new Intl.DateTimeFormat("ja-JP-u-ca-japanese", { era: "long" })
        .format(Date.parse(selector.val()))
        .split("");
    for (let i = 0; i < getDate.length; i++) {
        if (Number.isFinite(parseInt(getDate[i]))) {
            if (count == 0) tempYear += getDate[i];
            else if (count == 1) tempMonth += getDate[i];
            else if (count == 2) tempDay += getDate[i];
        }
        if (getDate[i] == "/") {
            count++;
        }
    }
    return tempYear + ";" + tempMonth + ";" + tempDay;
} $("input").change(function () {
    $(this).val(ConvertFullWidthNumber($(this).val()));
});
$("#third-pickup-2").click(function () {
    $("#numberOfDependents").val(null);
    hasDependents();
});
$("#third-pickup-1").click(function () {
    $("#numberOfDependents").attr("disabled", false)
    $("#numberOfDependents").val(1);
    $(".itemcol-1").attr("disabled", false);
    hasDependents();
    $(".radio-custom-gender").each(function () {
        $(this).attr("disabled", false);
    });
});
$("#numberOfDependents").change(function () {
    //Get number of dependents
    if ($("#numberOfDependents").val() > 4) {
        $("#numberOfDependents").val(4);
    } else if ($("#numberOfDependents").val() < 1) {
        $("#numberOfDependents").val(1);
    }
    var number = $("#numberOfDependents").val();
    for (let i = 1; i <= 4; i++) {
        if (i <= number) $(".itemcol-" + i + "").attr("disabled", false);
        else {
            $(".itemcol-" + i + "")
                .attr("disabled", true)
                .val(null);
        }
    }
});
function hasDependents() {
    // Get Has Dependent
    var vehicle = $('input[name="third-radio-group-3"]:checked').attr("id");

    if (vehicle == "third-pickup-2" || vehicle == "") {
        $("#dependents :input").attr("disabled", true).val(null);
        $('input[name="third-radio-group-3"]').attr("disabled", false);
    } else {
        if (!$("#numberOfDependents").val()) {
            $("#numberOfDependents").val(1);
            $(".itemcol-1").attr("disabled", false);
            $(".itemcol-2").attr("disabled", true).val(null);
            $(".itemcol-3").attr("disabled", true).val(null);
            $(".itemcol-4").attr("disabled", true).val(null);
        } else {
            for (let i = 1; i <= $("#numberOfDependents").val(); i++) {
                $(".itemcol-" + i + "").attr("disabled", false);
            }
        }
        $(".nenkinnumbersinput").each(function () {
            $(this).attr("disabled", false);
        });
        $(".radio-custom-gender").each(function () {
            $(this).attr("disabled", false);
        });
    }
}
$("#numberOfDependents").change(function () {
    //Get number of dependents
    if ($("#numberOfDependents").val() > 4) {
        $("#numberOfDependents").val(4);
    } else if ($("#numberOfDependents").val() < 1) {
        $("#numberOfDependents").val(1);
    }
    var number = $("#numberOfDependents").val();
    for (let i = 1; i <= 4; i++) {
        if (i <= number) $(".itemcol-" + i + "").attr("disabled", false);
        else {
            $(".itemcol-" + i + "")
                .attr("disabled", true)
                .val(null);
        }
    }
    $("#third-pickup-1").attr("checked", "checked");
    $("#third-pickup-2").attr("checked", false);
});
$(".allowancemoney").each(function () {
    $(this).val('');
});
$(".allowancemoney").change(function () {
    //Get Total Allowance money
    var allowancemoney = 0;
    $(".allowancemoney").each(function () {
        if (!Number.isNaN($(this).val()) && $(this).val())
            allowancemoney += parseInt($(this).val());

    });
    $("#Allowance_TotalMonthlyAmount").val(allowancemoney);
});
$("input").change(function () {
    getData();
    hasDependents();
    //Get nenkinnumbersinput
    var nenkinnumbers = "";
    $(".nenkinnumbersinput").each(function () {
        nenkinnumbers += $(this).val();
    });
    if (nenkinnumbers.length == 10) {
        $("#Dependents_0__DependentSpouseNenkinNumber").val(nenkinnumbers);
        $("#Dependents_1__DependentSpouseNenkinNumber").val(nenkinnumbers);
        $("#Dependents_2__DependentSpouseNenkinNumber").val(nenkinnumbers);
        $("#Dependents_3__DependentSpouseNenkinNumber").val(nenkinnumbers);
    }
});
function getData() {
    //Get value ForOffice
    var getvalueForOffice = "";
    $("input:checkbox[name='cbForOffice']:checked").each(function () {
        getvalueForOffice += $(this).val() + ";";
    });
    $("#ContactForm_ForOffice").val(getvalueForOffice);
    //Get Previous Job
    var previousJob = "";
    $(".previousJob").each(function () {
        if ($(this).val()) previousJob += $(this).val() + ";";
    });
    $("#ContactForm_PreviousJob").val(previousJob);
    //Get Insurance number
    var insurance = "";
    $(".contactinsurancenumber").each(function () {
        if ($(this).val()) insurance += $(this).val();
    });
    $("#ContactForm_Insurance").val(insurance);
    //Get Nenkin number
    var nenkinnumber = "";
    $(".contactnenkinnumber").each(function () {
        if ($(this).val()) nenkinnumber += $(this).val();
    });

    $("#ContactForm_NenkinNumber").val(nenkinnumber);
    // Get Vehicle
    var vehicle = $('input[name="vehicle-radio-group-2"]:checked').val();
    if (vehicle) {
        vehicle == "その他"
            ? $("#ContactForm_Transportation").val($("#OtherVehicle").val())
            : $("#ContactForm_Transportation").val(vehicle);
    }

    // Get Address
    var address = "";
    $(".address_contactform").each(function () {
        address += $(this).val() + ";";
    });
    $("#ContactForm_Address").val(address);
    //Date Of Joining

    var getJoiningYear = 2019 + parseInt($("#Joining-Year").val()) - 1;
    var JoiningDay = $("#Joining-Day").val();
    var Joiningmonth = $("#Joining-Month").val();
    var getJoiningDateTime =
        getJoiningYear + "-" + Joiningmonth + "-" + JoiningDay;
    $("#ContactForm_DateOfJoining").val(getJoiningDateTime);

    //Date Of Birth
    var getBirthdayType = $('input[name="birth-radio-group-1"]:checked').val();
    var getBirthYear = 0;
    if (getBirthdayType) {
        switch (getBirthdayType) {
            case "平":
                getBirthYear = 1989 + parseInt($("#Birthday-Year").val()) - 1;
                break;
            case "昭":
                getBirthYear = 1926 + parseInt($("#Birthday-Year").val()) - 1;
                break;
            default:
                getBirthYear = 1989 + parseInt($("#Birthday-Year").val()) - 1;
        }
    } else getBirthYear = 2019 + parseInt($("#Birthday-Year").val()) - 1;

    var Birthday = $("#Birthday-Day").val();
    var Birthmonth = $("#Birthday-Month").val();
    var getBirthDateTime = getBirthYear + "-" + Birthmonth + "-" + Birthday;
    $("#ContactForm_DateOfBirth").val(getBirthDateTime);

    //Created Date
    var getNengoYear = $("#listNengoYear").find(":selected").val();
    var getYear = 0;
    switch (getNengoYear) {
        case "reiwa":
            getYear = 2019 + parseInt($("#CreatedDate-Year").val()) - 1;
            break;
        case "heisei":
            getYear = 1989 + parseInt($("#CreatedDate-Year").val()) - 1;
            break;
        case "shouwa":
            getYear = 1926 + parseInt($("#CreatedDate-Year").val()) - 1;
            break;
        case "taishou":
            getYear = 1912 + parseInt($("#CreatedDate-Year").val()) - 1;
            break;
        default:
            getYear = 2019 + parseInt($("#CreatedDate-Year").val()) - 1;
    }
    var day = $("#CreatedDate-Day").val();
    var month = $("#CreatedDate-Month").val();
    var getDateTime = getYear + "-" + month + "-" + day;
    $("#ContactForm_CreatedDate").val(getDateTime);
}
