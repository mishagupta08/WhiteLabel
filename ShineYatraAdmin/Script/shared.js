var currentPage = 1;
$(document).ready(function () {    
    getCompanyThemeandSetting()
});

function RemoveErrorMessage(inputElement) {
    if (inputElement.val() != "" && inputElement.val().length > 0) {
        ////  inputElement.css('border-color', '#78c92c');
        return true;
    }
    else {
        inputElement.css('border-color', 'red');
        return false;
    }
}

function CheckEmailError(userElement) {
    var inputElement = $(this);
    if (userElement.selector != undefined) {
        inputElement = userElement;
    }

    if (RemoveErrorMessage(inputElement) == true) {
        if (validateEmail(inputElement.val()) == true) {
            ////   inputElement.css('border-color', '#78c92c');
            return true;
        }
        else {
            inputElement.css('border-color', 'red');
            return false;
        }
    }
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function CheckPasswordError(userElement) {
    var inputElement = $(this);

    if (userElement.selector != undefined) {
        inputElement = userElement;
    }

    if (RemoveErrorMessage(inputElement) == true) {
        if (inputElement.val().length > 3) {
            ////inputElement.css('border-color', '#78c92c');
            return true;
        }
        else {
            inputElement.css('border-color', 'red');
            return false;
        }
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isNumberValid(number) {
    var val = number.val();
    if (/^\d{10}$/.test(val)) {
        return true;
    } else {
        number.css('border-color', 'red');
        return false
    }
}


function ClearForm(formName) {
    if (document.getElementById(formName) != null) {
        document.getElementById(formName).reset();
    }
}

function closePopup() {
    $.magnificPopup.close();
}

function getCompanyThemeandSetting() {
    $(".preloader").show();
    $.ajax({
        url: '/Common/GetCompanyThemeAndSetting',
        type: 'Post',
        async: false,
    }).done(function (result) {        
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert("getCompanyThemeandSetting " + xhr);
    });
}
