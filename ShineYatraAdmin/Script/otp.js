﻿function openotpform() {
    $(".preloader").show();
    $.ajax({
        url: "/OTP/GenerateOTP",
        type: "GET"
    }).done(function (result) {
        if (result != null && result !== "") {
            $("#showverifyOTPform").click();
            $(".preloader").hide();
        }
        return false;
    }).fail(function (xhr) {
        alert(xhr);
        $(".preloader").hide();
    });
    return false;
}

function VerifyOTP() {
    var userOtp = $("#VerifyOTP").serialize();
    var response = "error";
    $.ajax({
        url: "/OTP/VerifyOTP",
        type: "POST",
        data: userOtp,
        async:false
    }).done(function (result) {
        if (result != null && result !== "" && result.indexOf("success") !== -1) {
            ClearForm("VerifyOTP");
            $("#otperror").html("");
            $(".mfp-close").click();
            checkOTP("success");            
        } else {
            $("#otperror").html("Invalid OTP Entered");
        }
            return false;
        })
        .fail(function (xhr) {
            $("#otperror").html("Some problem occured please try again");
        });
    return false;
}