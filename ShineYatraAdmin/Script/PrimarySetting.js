function saveRechargeDefaultSetting() {
    var defautSettings = $("#RechargeDefaultSetting").serialize();
    $(".preloader").show();
    $.ajax({
        url: '/PrimarySetting/SaveRechargeDefaultSetting',
        type: 'Post',        
        data: defautSettings
    }).done(function (result) {
        if (result == null || result == "") {
            swal("Status", "There is some error please try again later.", "error")
        }
        else {
            if (result.indexOf("SUCCESS") != -1) {                
                swal({
                    title: "Success",
                    text: "Record Added Successfully",
                    type: "success"
                }, function () {
                    $(".mfp-close").click();
                    ClearForm("RechargeDefaultSetting");
                    $("#defaultSettingblock").html("Loading Please wait...");
                    $.ajax({
                        url: '/PrimarySetting/DefaultRechargeSetting',
                        type: 'Get',
                    }).done(function (result) {
                        $("#defaultSettingblock").html(result);
                    });
                });
            }
            else {
                swal({
                    title: "Fail",
                    text: result,
                    type: "error"
                }, function () {
                    ClearForm("RechargeDefaultSetting");
                });
            }
        }
        $(".preloader").hide();
        return false;
    });

    return false;
}


function geteditrechargemarginpopup(servicetype, margin, serviceid, sub_service_id)
{    
    $("#edit-primary-margin").find("span[id='serviceProvider']").html(servicetype);
    $("#edit-primary-margin").find("input[id='back_discount_per']").val($("#"+margin).val());
    $("#edit-primary-margin").find("input[id='service_id']").val(serviceid);
    $("#edit-primary-margin").find("input[id='sub_service_id']").val(sub_service_id);
}

function getRowEditform(subServiceId, service_id, airlineName) {

    $("#edit-primary-margin input[name=sub_service_id]").val(subServiceId);
    $("#edit-primary-margin input[name=service_id]").val(service_id);
    $("#edit-primary-margin h3[name=airlineName]").html(airlineName);
    $("#row_" + subServiceId).find('input').each(function () {
        var name = $(this).attr('name');
        $("#edit-primary-margin input[name=" + name + "]").val($(this).val());
    });
}

function UpdatePrimaryMargin() {
    var primaryMargin = $("#edit-primary-margin").serialize();
    var subServiceId = $("#edit-primary-margin input[name=sub_service_id]").val();
    $(".preloader").show();
    $.ajax({
        url: '/PrimarySetting/UpdatePrimaryMargin',
        type: 'Post',
        data: primaryMargin
    }).done(function (result) {
        if (result == null || result == "") {
            swal("Status", "There is some error please try again later.", "error")
        }
        else {
            if (result.indexOf("SUCCESS") != -1) {
                swal({
                    title: "Success",
                    text: "Record updated Successfully",
                    type: "success"
                }, function () {

                    $("span[class=row_value_"+ subServiceId + "]").each(function () {
                        var name = $(this).attr('name');
                        var value = $("#edit-primary-margin input[name=" + name + "]").val();
                        $("#row_" + subServiceId + " input[type='hidden'][name='" + name + "']").val(value);
                        $(this).html(value);
                    });

                    ClearForm("edit-primary-margin");
                    $(".mfp-close").click();
                });
            }
            else {
                swal({
                    title: "Fail",
                    text: result,
                    type: "error"
                }, function () {
                    ClearForm("edit-primary-margin");
                    $(".preloader").hide();
                });
            }
        }
        $(".preloader").hide();
        return false;
    });
    return false;
}

function changeBillService()
{    
    var selectedBillService = $("#bill_service").val();
    $("#billServiceStructure").html("Loading please wait...");
    $.ajax({
        url: '/PrimarySetting/GetServiceSructure',
        type: 'GET',
        data: { selectedBillService: selectedBillService }
    }).done(function (result) {
       
        if (result == null || result == "") {            
            $("#billServiceStructure").html("No result found");
        }
        else {           
            $("#billServiceStructure").html(result);
        }        
    });  
}
