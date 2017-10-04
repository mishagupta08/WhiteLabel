function openotpform() {
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
    $.ajax({
        url: "/OTP/VerifyOTP",
        type: "POST",
        data: userOtp
    }).done(function (result) {
        if (result != null && result !== "" && result.indexOf("success") !== -1) {
            ClearForm("VerifyOTP");
            $(".mfp-close").click();
            $("#bookfightform").submit();
        }
        return false;
    }).fail(function (xhr) {
        alert(xhr);
        $(".preloader").hide();
        return false;
    });
    return false;
}