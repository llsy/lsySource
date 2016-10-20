
function Timesa(id, hour, minute, second) {
    if (hour == "0" && minute == "0" && second == "0") {
        $("#date-" + id + "").html("0");
        $("#time-" + id + "").html("0");
        $("#second-" + id + "").html("0");
        return false;
    }
    second--;
    if (second <= -1 && minute > 0) {
        second = 59;
        minute--;
    }
    if (minute <= 0) {
        minute = 0;
        hour--;
    }
    if (hour <= 0) {
        hour = 0;
    }
    if (minute <= 0 && second <= 0) {
        minute = 0;
        second = 0;
    }
    if (hour == 0 && minute == 0 && second == 1) {
        $("#second-" + id).html(0);
        return false;
    }

    $("#date-" + id + "").html(hour);
    $("#time-" + id + "").html(minute);
    $("#second-" + id + "").html(second);
    setTimeout("Timesa('" + id + "'," + hour + "," + minute + "," + second + ")", 1000);
}
function Timesa1(id, day, hour, minute, second) {
    if (day == "0" && hour == "0" && minute == "0" && second == "0") {
        //$("#day1-" + id + "").html("0");
        $("#date1-" + id + "").html("0");
        $("#time1-" + id + "").html("0");
        $("#second1-" + id + "").html("0");
        return false;
    }
    if (parseInt(day) > 0) {
        hour = parseInt(day) * 24 + parseInt(hour);
        day = 0;
    }
    second--;
    if (second <= -1 && minute > 0) {
        second = 59;
        minute--;
    }
    if (minute <= 0 && hour > 0) {
        minute = 59;
        hour--;
    }
    else if (minute <= 0) {
        minute = 0;
        hour--;
    }
    if (hour <= 0) {
        hour = 0;
    }
    if (minute <= 0 && second <= 0) {
        minute = 0;
        second = 0;
    }
    if (hour == 0 && minute == 0 && second == 1) {
        $("#second-" + id).html(0);
        $("#Send-" + id).attr("disabled", false);
        $("#Send-" + id).attr("class", "input-butto100-xhs");
        return false;
    }

    $("#date-" + id + "").html(hour);
    $("#time-" + id + "").html(minute);
    $("#second-" + id + "").html(second);
    setTimeout("Timesa1('" + id + "'," + day + "," + hour + "," + minute + "," + second + ")", 1000);
}
