﻿
function getGrouplist(service_id, currentGroupId, serviceName, sub_category) {
    $(".preloader").show();

    $.ajax({
        url: 'GetServiceGroupList',
        type: 'Post',
        datatype: 'Json',
        data: { serviceId: service_id, currentGroupId: currentGroupId, sub_category: sub_category }
    }).done(function (result) {
        var optionlist = "";
        $(result).each(function () {
            if (this.Selected) {
                optionlist = optionlist + "<option value = " + this.Value + " selected>" + this.Text + "</option>";
            }
            else {
                optionlist = optionlist + "<option value = " + this.Value + ">" + this.Text + "</option>";
            }
        });
        if (optionlist === "") {
            optionlist = optionlist + "<option value=0>--Please Select--</option>";
        }
        $("#price_group_list").html(optionlist);
        $("#serviceid").val(service_id);
        $("#serviceName").val(serviceName);
        $("#sub_category").val(sub_category);
        $("#change-group").addClass("white-popup-block");
        $("#openchangepopup").click();
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
    });

}

function EditCompanyPriceGroup() {
    $("#pricegroupError").html("");
    var price_group_id = $("#price_group_list").val();

    if (price_group_id == null || price_group_id == undefined || price_group_id == "" || price_group_id == 0) {
        $("#pricegroupError").html("Please select group.");
    }
    else {
        $(".preloader").show();

        var company_id = $("#companyid").val();
        var service_id = $("#serviceid").val();
        var service_name = $("#serviceName").val();
        var sub_category = $("#sub_category").val();
        currentTab = $('.tablist .tab-current').attr("id");

        $.ajax({
            url: 'EditCompanyPriceGroup',
            type: 'Post',
            data: { price_group_id: price_group_id, service_id: service_id }
        }).done(function (result) {
            $(".preloader").hide();
            if (result == null || result == "") {
                swal("Status", "There is some error please try again later.", "error");
                $(".confirm").click(function () {
                    $(".mfp-close").click();
                })
            }
            else {
                swal({
                    title: "Success",
                    text: "Record Updated Successfully",
                    type: "success"
                }, function () {
                    $(".mfp-close").click();
                    var arr = currentTab.split("-");
                    $("#section-bar-" + arr[1]).addClass("content-current");
                    getCommissionGroupDetails(price_group_id, "section-bar-" + arr[1], service_name, sub_category);
                });
            }
            $(".mfp-close").click();
            return false;
        });
    }
    return false;
}

function getCommissionGroupDetails(currentGroupId, divId, category, sub_category) {
    $(".preloader").show();
    $("#categorysections section").removeClass("content-current");
    
    if (currentGroupId === "0") {       
        $("#" + divId).html("No allotted group available.");        
        $("#section-bar-" + category).addClass("content-current");
        $(".preloader").hide();
    }
    else {

        $.ajax({
            url: 'getCommissionGroupDetails',
            type: 'get',
            datatype: 'Json',
            data: { currentGroupId: currentGroupId, category: category, sub_category: sub_category }
        }).done(function (result) {
            $("#" + divId).html(result);
            $("#categorysections section").removeClass("content-current");            
            $("#" + divId).addClass("content-current");
            if (category === "recharge") {                
                $("#section-bar-recharge").addClass("content-current");
            }
            $(".preloader").hide();
        }).fail(function (xhr) {
            alert(xhr);
            $(".preloader").hide();
        });
    }
}


function getRechargeCommissionStructure(companyId, sectionId) {
    $(".preloader").show();
    $.ajax({
        url: 'GetRechargeCommissionStructure',
        type: 'get',
        datatype: 'Json',
        data: { companyId: companyId }
    }).done(function (result) {
        $("#" + sectionId).html(result);
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
        $(".preloader").hide();
    });
}


function getCurrentRechargeComissionStructure(comp_seting_id) {
    $(".preloader").show();
    var settingid = $("#CompanySettingId").val();
    $.ajax({
        url: 'GetRechargeCompanySettingView',
        type: 'get',
        datatype: 'Json',
        data: { comp_seting_id: settingid }
    }).done(function (result) {
        $("#commissionrecchargepercent").html(result);
        $("#commissionrecchargepercent").addClass("white-popup-block");
        $("#opencommissionrecchargepercent").click();
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
        $(".preloader").hide();
    });

}

function SubmitCompanyRechargeSetting(settingName) {
    $(".preloader").show();
    var companySettingInfo = $("#" + settingName).serialize();
    var company_id = $("#company_id").val();
    var currentTab = $('.tablist .tab-current').attr("id");
    var arr = currentTab.split("-");
    var sectionId = "section-bar-" + arr[1];
    $.ajax({
        url: 'SubmitCompanyRechargeSetting',
        type: 'Post',
        data: companySettingInfo
    }).done(function (result) {
        if (result == null || result == "") {
            swal("Status", "There is some error please try again later.", "error")
        }
        else {
            swal({
                title: "Success",
                text: "Record Updated Successfully",
                type: "success"
            }, function () {
                $(".mfp-close").click();
                getRechargeCommissionStructure(company_id, sectionId);
            });
        }
        $(".preloader").hide();
        return false;
    });
    return false;
}



