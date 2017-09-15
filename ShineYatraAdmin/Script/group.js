function getgroupList(category,subcategory)
{
    $(".preloader").show();
    $("#sub_service_name").val(subcategory);
    $("#serviceGroupList").html("");
    $("#" + subcategory + "_structure").html("Loading Please wait.....");
    $.ajax({
        url: '/Group/Getgrouplist',
        type: 'get',
        datatype: 'Json',
        data: { category: category, subcategory: subcategory }
    }).done(function (result) {
        for (var i = 0; i < result.length;i++)
        {
            var option = new Option(result[i].comp_group_name, result[i].comp_group_id);
            option.setAttribute("data-rowid",result[i].row_id);
            $('#serviceGroupList').append(option);
        }
        getStructure(subcategory + "_structure");
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
        $(".preloader").hide();
    });
}

function GetAllottedGroup(category, subcategory) {
    $(".preloader").show();
    $("#sub_service_name").val(subcategory);
    $("#serviceGroupList").html("");
    $("#" + subcategory + "_structure").html("Loading Please wait.....");
    $.ajax({
        url: '/Group/GetAllottedGroup',
        type: 'get',
        datatype: 'Json',
        data: { category: category, sub_category: subcategory }
    }).done(function (result) {        
        $("#displayAllottedGroup").html(result.comp_group_name);
        $("#serviceGroupList").val(result.comp_group_id);
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
    var type = $("#Grouptype").val();

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
        data: { service_id: service_id, currentGroupId: selectedgroup, sub_service: subservice, type:type }
    }).done(function (result) {
        $("#" + contentHolder).html(result);
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
        $(".preloader").hide();
    });
}



function getRowEditform(row_id)
{
   
    $("#group_name").html($("#serviceGroupList option:selected").text());       
    $("#editGroupRowDetails input[name=service_id]").val($("#CurrentserviceId").val());
    $("#editGroupRowDetails input[name=company_group_id]").val($("#serviceGroupList").val());
    $("#editGroupRowDetails input[name=sub_category]").val($("#sub_service_name").val());
       $("#row_" + row_id).find('input').each(function () {
           var name= $(this).attr('name');
           $("#editGroupRowDetails input[name="+name+"]").val($(this).val());
       });
       if (row_id != "recharge") {
           $("h3[name=airlineName]").html($("input[name=airlineName]").val());
       }
       else {           
           $("#editGroupRowDetails input[name=row_id]").val($("#serviceGroupList option:selected").attr("data-rowid"));
       }
}

function EditGroupRow() {
    $(".preloader").show();
    var UpdatedRow = $("#editGroupRowDetails").serialize();
    var service_id= $("#CurrentserviceId").val();
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
                    text: "Record Updated Successfully",
                    type: "success"
                }, function () {
                    if (service_id != "4") {
                        $("span[class=row_value_" + subServiceId + "]").each(function () {
                            var name = $(this).attr('name');
                            var value = $("#editGroupRowDetails input[name=" + name + "]").val();
                            $("#row_" + subServiceId + " input[type='hidden'][name='" + name + "']").val(value);
                            $(this).html(value);
                        });
                    }
                    else {
                        ClearForm("editGroupRowDetails");
                        getStructure("recharge");
                    }

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