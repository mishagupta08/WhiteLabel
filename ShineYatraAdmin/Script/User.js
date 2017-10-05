function setSessionForSelectedUser(member_id, company_id) {
    $(".preloader").show();
    $.ajax({
        url: 'setSessionForSelectedUser',
        type: 'Post',
        data: { member_id: member_id, company_id: company_id }
    }).done(function (result) {
        if (result == true) {
            window.location.href = "/User/commissionStructure";
        }
        else {

            swal({
                title: "Fail",
                text: "Something went wrong please try again!!",
                type: "error"
            }, function () {
                $(".preloader").hide();
            });
        }
    });
}


function getFundForm(member_id, company_id,member_name,company_name) {
    $("#addcompanyfund").html("");    
    $(".preloader").show();
    
    $.ajax({
        url: 'setSessionForSelectedUser',
        type: 'Post',
        data: { member_id: member_id, company_id: company_id }
    }).done(function (result) {
        if (result == true) {
            $.ajax({
                url: 'FundRequest',
                type: 'Post',                              
            }).done(function (result) {
                $("#addcompanyfunds").html(result);
                $("#add-company_funds").addClass("white-popup-block");
                $("#popupfundForm").click();
                $(".preloader").hide();
            }).fail(function (xhr) {
                alert(xhr);
            });
        }
        else {

            swal({
                title: "Fail",
                text: "Something went wrong please try again!!",
                type: "error"
            }, function () {
                $(".preloader").hide();
            });
        }
    });


}

function AddEditFund() {
    var amount = $("#amount").val();
    var paymentMode = $("#deposit_mode").val();
    if (amount == null || amount == "" || amount == undefined ||amount <= 0 || !jQuery.isNumeric(amount)) {
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

function GetInsertUserView() {
    $("#addformHtml").html("");
   $(".preloader").show();
    $.ajax({
        url: 'GetInsertUserView',
        type: 'Post',
        datatype: 'Json',
        async: false
    }).done(function (result) {
        $("#addformHtml").html(result);
        $("#addUser").addClass("white-popup-block");
        $("#InsertUserForm").click();
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
    });
}

function AddUser() {
    var userDetail = $('#addUser').serialize();    
    $(".preloader").show();
    $.ajax({
        url: 'AddUser',
        type: 'Post',
        datatype: 'Json',
        async: false,
        data: userDetail
    }).done(function (result) {
        if (result == null || result == "") {
            swal("Status", "There is some error please try again later.", "error");
            $(".confirm").click(function() {
                $(".mfp-close").click();
            });
        }
        else if (result.indexOf("SUCCESS") !== -1) {
            swal({
                title: "Success",
                text: "Record Added Successfully",
                type: "success"
            }, function () {
                ClearForm("addUser");
                location.reload();              
            });
        }
        else {
            swal({
                title: "FAILED",
                text: result,
                type: "error"
            }, function () {
                $(".preloader").hide();
            });
            return false;
        }
    }).fail(function (xhr) {
        alert(xhr);
    });
    return false;
}