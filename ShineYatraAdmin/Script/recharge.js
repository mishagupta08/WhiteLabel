function getServiceProviderList(type) {
    $(".preloader").show();
    //var searchCompanyInfo = $('#searchFlightForm').serialize();
    $.ajax({
        url: 'Recharge/getServiceProviderList',
        type: 'Post',
        data: { type: type }
    }).done(function (result) {
        for (var i = 0; i < result.length; i++) {
            var option = new Option(result[i].Text, result[i].Value);
            //option.setAttribute("data-margin", result[i].margin);
            $('#operator').append(option);
        }
        $(".preloader").hide();
        return false;
    });
    return false;
}

function setvalue()
{    
    $("#ProviderName").val($("#operator option:selected").text());
}

function doRecharge() {
    $(".preloader").show();    
    var prepaidDetails = $('#RechargeForm').serialize();
    var response;
    $.ajax({
        url: 'Recharge/ValidateTransaction',
        type: 'Post',
        data: prepaidDetails
    }).done(function (result) {
        if (result == "success") {
            response =  true;
        }
        else {
            swal("Status", result, "error")
            response = false;
        }
        $(".preloader").hide();        
    });
    return response;
}