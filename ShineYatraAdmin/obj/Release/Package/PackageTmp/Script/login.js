$(document).ready(function () {
    $("#closeError").hide();
    $(".preloader").hide();
    //$("#loginButton").bind("click", ValidateLoginUser);
    //$("#password").bind("blur", CheckPasswordError);

    $("#forgotPassword").bind("click", ForgotPassword);
    $("#closeError").bind("click", RemoveLoginErrorMessage);
});

function RemoveLoginErrorMessage() {
    $("#closeError").hide();
    $("#loginError").html("");
    $("#Username").css('border-color', '#ccc');
    $("#Password").css('border-color', '#ccc');
}

function checkFields() {
    var username = RemoveErrorMessage($("#Username"));
    var password = RemoveErrorMessage($("#Password"));
    if ((username && password)) {
        if (CheckPasswordError($("#Password")) == false) {
            $("#loginError").html("Password should be minimum 6 character");
            $("#closeError").show();
            return false;
        }
        else {
            return true;
        }
    }
    else {
        $("#loginError").html("Please enter username or password");
        $("#closeError").show();
        return false;
    }
}

function ValidateLoginUser() {
    $("#loginError").html("");
    var loginDetail = $('#loginform').serialize();
    $(".preloader").show();
    $.ajax({
        url: '/Login/ValidateUser',
        type: 'Post',
        datatype: 'Json',
        data: loginDetail
    }).done(function (result) {

        if (result == "") {
        var returnUrl = $("#returnUrl").val();        
        if (returnUrl != null && returnUrl != "" && returnUrl != undefined) {            
            window.location.href = returnUrl;
        }
        else {
            window.location.href = 'Dashboard/Index';
        }                   
        }
        else {
            $("#loginError").html(result);
            $(".preloader").hide();
            $("#closeError").show();
        }
    }).fail(function (error) {
        $("#loginError").html(error.statusText);
        $(".preloader").hide();
        $("#closeError").show();
        $(".preloader").hide();
    });

    return false;
}

function ForgotPassword() {
    if (RemoveErrorMessage($("#Username"))) {
        RemoveLoginErrorMessage();
        var userInfo = $('#login_form').serialize();
        $("#ajax_loader").show();
        $.ajax({
            url: 'Login/ForgotPassword',
            type: 'Post',
            datatype: 'Json',
            data: userInfo
        }).done(function (result) {
            $("#loginError").html(result);
            $("#ajax_loader").hide();
            $("#closeError").show();
        });
    }
    else {
        $("#loginError").html("Please enter username.");
        $("#ajax_loader").hide();
        $("#closeError").show();
    }
}