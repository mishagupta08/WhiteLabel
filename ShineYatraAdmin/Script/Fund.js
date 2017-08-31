﻿function AddEditFund() {
    var amount = $("#amount").val();
    var paymentMode = $("#deposit_mode").val();
    if (amount == null || amount == "" || amount == undefined || amount <= 0 || !jQuery.isNumeric(amount)) {
        $("#amountError").show();
    }
    else if (paymentMode == null || paymentMode == "" || paymentMode == undefined || paymentMode == 0) {
        $("#paymentModeError").show();
    }
    else {
        $("#amountError").hide();
        $("#paymentModeError").hide();

        $(".preloader").show();
        var fundModel = $('#add-company_funds').serialize();

        $.ajax({
            url: 'SaveFundDetail',
            type: 'Post',
            data: fundModel
        }).done(function (result) {
            if (result == null || result == "") {
                swal("Status", "There is some error please try again later.", "error")
                $(".confirm").click(function () {
                    //$(".mfp-close").click();
                })
            }
            else {
                $(".mfp-close").click();
                swal("Status", result, "success")
                $(".confirm").click(function () {
                    ClearForm("add-company_funds");
                })
            }
            $(".preloader").hide();
            return false;
        });
    }
    return false;
}