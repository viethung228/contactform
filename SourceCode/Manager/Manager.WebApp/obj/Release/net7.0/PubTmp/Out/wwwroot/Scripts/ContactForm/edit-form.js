function validateMyForm() {
  var flag = true;
  var isPhoneNumber =
    /^[(]{0,1}[0-9]{3}[)]{0,1}[-\s\.]{0,1}[0-9]{3}[-\s\.]{0,1}[0-9]{4}$/;
  if (!isPhoneNumber.test($("#ContactForm_PhoneNumber").val())) {
    flag = false;
  }
  if ($("#Dependents_0__Furigana").val()) {
    if (!$("#Dependents_0__DependentSpouseNenkinNumber").val()) flag = false;
  }
  if (!flag) {
    alert("validation failed false");
    window.location.reload();
  }
  return flag;
}
function insertString(origString, stringToAdd, indexPosition) {
  newString =
    origString.slice(0, indexPosition) +
    stringToAdd +
    origString.slice(indexPosition);
  return newString;
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
}
window.onload(DisplayValues());
window.onload = (event) => (

    $(".dpaddress").each(function () {

        if (!$(this).val()) {
            $(this).val('なし');
        }
    })
);
function DisplayValues() {
  //Display values

  // Get Vehicle
  var getvehicle = $("#ContactForm_Transportation").val();
  $('input[name="vehicle-radio-group-2"]').each(function () {
    if ($(this).val() == getvehicle) {
      $(this).attr("checked", "checked");
      return false;
    }
    if ($(this).val() == "その他") {
      $(this).attr("checked", "checked");
      $("#OtherVehicle").val($("#ContactForm_Transportation").val());
    }
  });

  //get for Office
  var getForOfficeValue = $("#ContactForm_ForOffice").val();
  $("input:checkbox[name='cbForOffice']").each(function () {
    if (getForOfficeValue.includes($(this).val())) {
      $(this).attr("checked", "checked");
    }
  });

  // get dependents
  var countDependents = 0;
  var getNenkinNumber = "";

  for (let i = 0; i < 4; i++) {
    if ($("#Dependents_" + i + "__DependentSpouseNenkinNumber").val()) {
      getNenkinNumber = $(
        "#Dependents_" + i + "__DependentSpouseNenkinNumber"
      ).val();
      countDependents++;
      $("#third-pickup-1").attr("checked", "checked");
    } else {
      break;
    }
  }
  if (countDependents > 0) $("#numberOfDependents").val(countDependents);
  if (getNenkinNumber != "") {
    $(".nenkinnumbersinput").each(function (index) {
      $(this).val(getNenkinNumber.split("")[index]);
    });
  }
  for (let i = 1; i <= 4; i++) {
    if (i <= countDependents) $(".itemcol-" + i + "").attr("disabled", false);
    else {
      $(".itemcol-" + i + "")
        .attr("disabled", true)
        .val(null);
    }
  }
  //get Previous Job
  $(".previousJob").each(function (index, value) {
    $(this).val($("#ContactForm_PreviousJob").val().split(";")[index]);
  });
  //get address
  $(".address_contactform").each(function (index, value) {
    $(this).val($("#ContactForm_Address").val().split(";")[index]);
  });
  //date of join
  $("#Joining-Year").val(
    GetJapaneseDate($("#ContactForm_DateOfJoining")).split(";")[0]
  );
  $("#Joining-Month").val(
    GetJapaneseDate($("#ContactForm_DateOfJoining")).split(";")[1]
  );
  $("#Joining-Day").val(
    GetJapaneseDate($("#ContactForm_DateOfJoining")).split(";")[2]
  );
  //date of birth
  var dateOfBirth = new Intl.DateTimeFormat("ja-JP-u-ca-japanese", {
    era: "long",
  }).format(Date.parse($("#ContactForm_DateOfBirth").val()));
  $("#Birthday-Year").val(
    GetJapaneseDate($("#ContactForm_DateOfBirth")).split(";")[0]
  );
  $("#Birthday-Month").val(
    GetJapaneseDate($("#ContactForm_DateOfBirth")).split(";")[1]
  );
  $("#Birthday-Day").val(
    GetJapaneseDate($("#ContactForm_DateOfBirth")).split(";")[2]
  );
  $("input:radio[name=birth-radio-group-1]")
    .filter("[value=" + dateOfBirth[0] + "]")
    .prop("checked", true);
  //Nenkin number
  var arrNenkinNumber = $("#ContactForm_NenkinNumber").val().split("");
  $(".contactnenkinnumber").each(function (index) {
    $(this).val(arrNenkinNumber[index]);
  });
  //insurance
  var arrInsurance = $("#ContactForm_Insurance").val().split("");
  $(".contactinsurancenumber").each(function (index) {
    $(this).val(arrInsurance[index]);
  });
  //created date
  var createdDate = new Intl.DateTimeFormat("ja-JP-u-ca-japanese", {
    era: "long",
  }).format(Date.parse($("#ContactForm_CreatedDate").val()));
  createdDate = insertString(createdDate, " ", 2);
  var stringCreatedDate = "記入日 ";
  createdDate.split("/").forEach(function (value, index) {
    if (index == 0) {
      stringCreatedDate += value + " 年 ";
    } else if (index == 1) {
      stringCreatedDate += value + " 月 ";
    } else if (index == 2) {
      stringCreatedDate += value + " 日";
    } else {
      stringCreatedDate += " ";
    }
  });
  $("#createdDate").text(stringCreatedDate);
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
function hasDependents() {
  // Get Has Dependent
  var vehicle = $('input[name="third-radio-group-3"]:checked').val();
  if (vehicle) {
    if (vehicle == "無") {
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
      $(".nenkinnumbersinput").attr("disabled", false);
    }
    $("#numberOfDependents").attr("disabled", false);
  }
}
$(".allowancemoney").change(function () {
  //Get Total Allowance money
  var allowancemoney = 0;
  $(".allowancemoney").each(function () {
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
$("textarea").change(function () {
  getData();
  hasDependents();
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
