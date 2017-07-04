function getgroupList(category,subcategory)
{
    $("#sub_service_name").val(subcategory);
    $("#serviceGroupList").html("");
    $.ajax({
        url: '/Group/Getgrouplist',
        type: 'get',
        datatype: 'Json',
        data: { category: category, subcategory: subcategory }
    }).done(function (result) {
        for (var i = 0; i < result.length;i++)
        {
            $('#serviceGroupList').append(new Option(result[i].comp_group_name, result[i].comp_group_id));
        }
        getStructure(subcategory + "_structure");
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
        $(".preloader").hide();
    });
}

function AddNewGroup()
{
    $(".preloader").show();
    $("#NewGroupForm input[name=sub_category]").val($("#sub_service_name").val());
    var newGroup = $("#NewGroupForm").serialize();    
    $.ajax({
        url: '/Group/AddNewGroup',
        type: 'Post',        
        data: newGroup
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
                    ClearForm("NewGroupForm");
                    location.reload();
                });
            }
            else {
                swal({
                    title: "Fail",
                    text: result,
                    type: "error"
                }, function () {
                    ClearForm("NewGroupForm");
                });
            }
        }
        $(".preloader").hide();
        return false;
    });    
    return false;
}

function getStructure(contentHolder)
{    
    var selectedgroup = $("#serviceGroupList").val();
    var service_id = $("#CurrentserviceId").val();
    var subservice = $("#sub_service_name").val();

    if (contentHolder == "recharge")
    {
        contentHolder = subservice + "_structure";
    }

    $("#" + contentHolder).html("Loading Please wait...");

    $(".preloader").show();
    $.ajax({
        url: '/Group/getCommissionGroupDetails',
        type: 'get',
        datatype: 'Json',
        data: { service_id: service_id, currentGroupId: selectedgroup, sub_service: subservice }
    }).done(function (result) {
        $("#" + contentHolder).html(result);
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
        $(".preloader").hide();
    });
}

//function getrechagegroupstructure(id, selectedgroup, service_id) {
//    $(".preloader").show();
//    var subservice = $("#sub_service_name").val();
//    $.ajax({
//        url: '/Group/getCommissionGroupDetails',
//        type: 'get',
//        datatype: 'Json',
//        data: { service_id: service_id, currentGroupId: selectedgroup, sub_service: subservice }
//    }).done(function (result) {
//        $("#" + id).html(result);
//        $(".preloader").hide();
//    }).fail(function (xhr) {
//        alert(xhr);
//        $(".preloader").hide();
//    });
//}

function getRowEditform(row_id)
{
    var row = {};
    $("#group_name").html($("#serviceGroupList option:selected").text());
    $("h3[name=airlineName]").html($("input[name=airlineName]").val());
   
    $("#editGroupRowDetails input[name=service_id]").val($("#CurrentserviceId").val());
    $("#editGroupRowDetails input[name=company_group_id]").val($("#serviceGroupList").val());
    $("#editGroupRowDetails input[name=sub_category]").val($("#sub_service_name").val());
       $("#row_" + row_id).find('input').each(function () {
           var name= $(this).attr('name');
           $("#editGroupRowDetails input[name="+name+"]").val($(this).val());
        });        
}

function EditGroupRow() {
    $(".preloader").show();
    var UpdatedRow = $("#editGroupRowDetails").serialize();
    var subServiceId = $("#editGroupRowDetails input[name=sub_service_id]").val();
    $.ajax({
        url: '/Group/UpdateCommissionGroupDetails',
        type: 'Post',
        data: UpdatedRow
    }).done(function (result) {
        if (result == null || result == "") {
            swal("Status", "There is some error please try again later.", "error");
            $(".preloader").hide();
        }
        else {
            if (result.indexOf("SUCCESS") != -1) {
                swal({
                    title: "Success",
                    text: "Record Added Successfully",
                    type: "success"
                }, function () {

                    $("span[class=row_value_" + subServiceId + "]").each(function () {
                        var name = $(this).attr('name');
                        var value = $("#editGroupRowDetails input[name=" + name + "]").val();
                        $("#row_" + subServiceId + " input[type='hidden'][name='" + name + "']").val(value);
                        $(this).html(value);
                    });

                    $(".mfp-close").click();
                    $(".preloader").hide();                   
                    //getStructure();
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
            }
        }       
        return false;
    });
    return false;
}