var currentPageNumber = 1;
var currentPageSlot = 1;
var sortArrow = "<div class='up-arrow'></div>";
var sortDownArrow = "<div class='up-arrow down-arrow'></div>";

$(document).ready(function () {    
    $("#insertCompany").unbind();
    $("#insertCompany").bind("click", GetInsertCompanyView);
    bindCompanyListLink();
    getuserbalance();
});

function bindCompanyListLink() {
    $("#insertCompany").unbind();
    $("#insertCompany").bind("click", GetInsertCompanyView);

    $("a[name=editView]").unbind();
    $("a[name=editView]").bind("click", GetInsertCompanyView);

    $("a[name=company-setting]").unbind();
    $("a[name=company-setting]").bind("click", GetEditCompanySettingView);

    $("a[name=add-fund]").unbind();
    $("a[name=add-fund]").bind("click", GetAddFundView);

    /*To repace value of pages in paging*/
    $(".pagination > li > a").attr("href", "#");

    /*To set focus on page click*/
    $("a[name=page]").focus(function () {
        this.blur();
    });

    /*To set binding of page no. in paging*/
    //$("a[name=page]").unbind("click");
    //$("a[name=page]").bind("click", GetRecordsByPage);

    //checkPreviousButton();
    //checkNextButton();

    /*For bind sorting sunctionality on column*/
    //$("th[role=columnheader]").unbind();
    //$("th[role=columnheader]").bind("click", SortList);

    //var pageSlot = parseInt($("#pageSlot").val());
    //var pageIndex = parseInt($("#pageIndex").val());

    //if (pageSlot != 0) {
    //    currentPageNumber = pageIndex;
    //    currentPageSlot = pageSlot;

    //    checkNextButton();
    //    checkPreviousButton();
    //}
}

//function SortList() {
//    $(".preloader").show();
//    var columnName = $(this).attr("name");
//    var sortingOrder = "Asc";
//    var sortElement = $(this).find(".down-arrow");
//    if (sortElement.length == 0) {
//        sortingOrder = "Desc";
//    }

//    $.ajax({
//        url: 'GetSelectedMenu',
//        type: 'Post',
//        data: { menu: null, isRefresh: false, isBack: false, sortColumn: columnName, sortOrder: sortingOrder }
//    }).done(function (result) {
//        $(".preloader").hide();
//        AssignListView(result);
//        var element = $("th[name=" + columnName + "]");

//        $('.up-arrow').remove();
//        if (sortingOrder == "Asc") {
//            element.append(sortArrow);
//        }
//        else {
//            element.append(sortDownArrow);
//        }        
//    });
//}

function getuserbalance() {
    $(".preloader").show();    
    $.ajax({
        url: 'GetUserBalance',
        type: 'Post'
    }).done(function (result) {
        $("#usercurrentbalance").html("Current Balance   :  "+result);
    }).fail(function (xhr) {
        alert(xhr);
    });
}

function SearchCompanyList() {
    $(".preloader").show();
    var searchCompanyInfo = $('#searchCompanyForm').serialize();
    $.ajax({
        url: 'SearchCompanyList',
        type: 'Post',
        data: searchCompanyInfo
    }).done(function (result) {
        AssignListView(result);
        //$(".mfp-close").click();
        ClearForm("searchCompanyForm");
        $(".preloader").hide();
        return false;
    });

    return false;
}

function SearchUserList() {
    $(".preloader").show();
    var searchCompanyInfo = $('#SearchUserList').serialize();
    $.ajax({
        url: 'SearchUserList',
        type: 'Post',
        data: searchCompanyInfo
    }).done(function (result) {
        $("#Usercontainer").html(result);
        ClearForm("SearchUserList");
        $(".preloader").hide();
        return false;
    });

    return false;
}

function AssignListView(result) {
    $("#dashboardContent").html("");
    $("#dashboardContent").html(result);
    bindCompanyListLink();
}

function AddEditCompany() {
    $("#countryError").html("");
    var countryId = document.getElementById("countryDropDown").selectedIndex;

    if (countryId == 0) {
        $("#countryError").html("Please select Country.");
    }
    else {
        $(".preloader").show();
        var searchCompanyInfo = $('#add-company').serialize();
        $.ajax({
            url: 'AddEditCompany',
            type: 'Post',
            data: searchCompanyInfo
        }).done(function (result) {
            $(".preloader").hide();
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
                    location.reload();                    
                })
            }            
            
            return false;
        });
    }
    return false;
}

function GetInsertCompanyView() {
    $("#addformHtml").html("");
    var selectedCompanyId = $(this).attr("data-company-id");
    $(".preloader").show();

    $.ajax({
        url: 'GetInsertCompanyView',
        type: 'Post',
        datatype: 'Json',
        data: { companyId: selectedCompanyId }
    }).done(function (result) {
        $("#addformHtml").html(result);
        $("#add-company").addClass("white-popup-block");
        $("#popupForm").click();
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
    });
}

function GetEditCompanySettingView() {
    $("#companySettingsFormHtml").html("");
    $(".preloader").show();

    var selectedcompanySettingId = $(this).attr("data-company-setting-id");
    var selectedCompanyName = $(this).attr("data-company-name");
    $.ajax({
        url: 'GetEditCompanySettingView',
        type: 'Post',
        data: { companySettingId: selectedcompanySettingId, companyName: selectedCompanyName }
    }).done(function (result) {
        $("#companySettingsFormHtml").html(result);
        $("#editPopupForm").click();
        $(".preloader").hide();
    });
}

function GetAddFundView() {
    $("#add_fundFormHtml").html("");
    $(".preloader").show();

    var selectedCompanyId = $(this).attr("data-company-id");
    var selectedCompanyName = $(this).attr("data-company-name");

    $.ajax({
        url: 'GetAddFundView',
        type: 'Post',
        data: { companyId: selectedCompanyId, companyName: selectedCompanyName }
    }).done(function (result) {
        $("#add_fundFormHtml").html(result);
        $("#addFundPopupForm").click();
        $(".preloader").hide();
    });
}

function SaveAddFundDetail()
{
    $(".preloader").show();
    var addFundInfo = $('#add_fund').serialize();
    $.ajax({
        url: 'SaveFundDetail',
        type: 'Post',
        data: addFundInfo
    }).done(function (result) {
        if (result == null || result == "") {
            swal("Status", "There is some error please try again later.", "error")
            $(".confirm").click(function () {
                $(".preloader").hide();
                //$(".mfp-close").click();
            })
        }
        else {
            swal("Status", result, "success")
            $(".confirm").click(function () {
                $(".preloader").hide();
                SearchCompanyList();
            })
        }

       
        return false;
    });
}

/***Paging functions * Start***/

//function GetRecordsByPage(pageNumber, viewName) {
//    if ($(this).attr("data-page-index") != undefined) {
//        pageNumber = $(this).attr("data-page-index");
//        viewName = $(this).attr("data-view");
//        $(".pagination > li").removeClass('active');
//        $(this).parent('li').addClass('active');
//    }
//    var paging_count = parseInt($("#pagingCount").val());
//    var RecordCount = parseInt($("#RecordCount").val()); 
//    currentPageNumber = parseInt(pageNumber);
//    $.ajax({
//        url: 'GetRecordsByPageIndex',
//        type: 'Post',
//        data: { pageIndex: pageNumber, View: viewName, paging_count: paging_count, RecordCount: RecordCount }
//    }).done(function (result) {
//        $("#container").html("");
//        $("#container").html(result);
//        $(".pagination > li > a").attr("href", "#");
//        $("a[name=page]").unbind("click");
//        $("a[name=page]").bind("click", GetRecordsByPage);

//        $("#fromPageIndex").html($("#fromPage").val());
//        $("#toPageIndex").html($("#toPage").val());

//        bindCompanyListLink();
//    });

//    checkPreviousButton();
//    checkNextButton();
//}

//function checkPreviousButton() {
//    if (currentPageNumber == 1) {
//        $("#previous").parent('li').addClass('disabled');
//        $("#previous").unbind("click");
//    }
//    else {
//        $("#previous").parent('li').removeClass('disabled');
//        $("#previous").unbind("click");
//        $("#previous").bind("click", PreviousRecordsByPage);
//    }

//    if (currentPageSlot == 1) {
//        $("#previousPages").parent('li').addClass('disabled');
//        $("#previousPages").unbind("click");
//    }
//    else {
//        $("#previousPages").parent('li').removeClass('disabled');
//        $("#previousPages").unbind("click");
//        $("#previousPages").bind("click", GetPreviousPages);
//    }
//}

//function checkNextButton() {
//    var total = parseInt($("#pagingCount").val());
//    if (currentPageNumber == total) {
//        $("#next").parent('li').addClass('disabled');
//        $("#next").unbind("click");
//    }
//    else {
//        $("#next").parent('li').removeClass('disabled');
//        $("#next").unbind("click");
//        $("#next").bind("click", NextRecordsByPage);
//    }

//    var totalPages = parseInt(total / 5);
//    if (total % 5) {
//        totalPages = totalPages + 1;
//    }

//    if (totalPages == 0 || totalPages == currentPageSlot) {
//        $("#nextPages").parent('li').addClass('disabled');
//        $("#nextPages").unbind("click");
//    }
//    else {
//        $("#nextPages").parent('li').removeClass('disabled');
//        $("#nextPages").unbind("click");
//        $("#nextPages").bind("click", GetNextPages);
//    }

//    return true;
//}

//function GetPreviousPages() {

//    if (currentPageSlot == 1) {
//        return;
//    }

//    var element = $("li[page-slot=" + currentPageSlot + "]");
//    for (var i = 0; i < element.length; i++) {
//        element[i].className = element[i].className.replace("db", "dn");
//    }

//    currentPageSlot = currentPageSlot - 1;
//    currentPageNumber = currentPageSlot * 5;
//    var element = $("li[page-slot=" + currentPageSlot + "]");
//    for (var i = 0; i < element.length; i++) {
//        element[i].className = element[i].className.replace("dn", "db");
//    }

//    checkPreviousButton();
//    checkNextButton();
//    PreviousSlotLastRecord();
//}

//function GetNextPages() {

//    var total = parseInt($("#pagingCount").val());
//    if (currentPageSlot == total / 5) {
//        return;
//    }

//    var element = $("li[page-slot=" + currentPageSlot + "]");
//    for (var i = 0; i < element.length; i++) {
//        element[i].className = element[i].className.replace("db", "dn")
//    }

//    currentPageNumber = currentPageSlot * 5;
//    currentPageSlot = currentPageSlot + 1;
//    var element = $("li[page-slot=" + currentPageSlot + "]");

//    for (var i = 0; i < element.length; i++) {
//        element[i].className = element[i].className.replace("dn", "db")
//    }

//    checkPreviousButton();
//    checkNextButton();
//    NextSlotFirstRecords();
//}

//function NextRecordsByPage() {
//    var element = $("li.active").next();
//    $(".pagination > li").removeClass('active');
//    element.addClass("active");
//    currentPageNumber = currentPageNumber + 1;
//    var viewName = $("#next").attr("data-view");
//    GetRecordsByPage(currentPageNumber, viewName);
//}

//function PreviousRecordsByPage() {
//    var preElement = $("li.active").prev();
//    $(".pagination > li").removeClass('active');
//    preElement.addClass("active");

//    currentPageNumber = currentPageNumber - 1;
//    var viewName = $("#previous").attr("data-view");
//    GetRecordsByPage(currentPageNumber, viewName);
//}

//function NextSlotFirstRecords() {
//    currentPageNumber = currentPageNumber + 1;
//    $(".pagination > li").removeClass('active');
//    var element = $("a[data-page-index=" + currentPageNumber + "]").parent();
//    element.addClass("active");
//    var viewName = $("#nextPages").attr("data-view");
//    GetRecordsByPage(currentPageNumber, viewName);
//}

//function PreviousSlotLastRecord() {
//    var preElement = $("a[data-page-index=" + currentPageNumber + "]").parent();
//    $(".pagination > li").removeClass('active');
//    preElement.addClass("active");
//    var viewName = $("#previousPages").attr("data-view");
//    GetRecordsByPage(currentPageNumber, viewName);
//}

function SubmitCompanySetting(settingName) {
    $(".preloader").show();

    companySettingInfo = $("#" + settingName).serialize();

    $.ajax({
        url: 'SubmitCompanySetting',
        type: 'Post',
        data: companySettingInfo
    }).done(function (result) {
        if (result == null || result == "") {
            swal("Status", "There is some error please try again later.", "error")
        }
        else {
            swal("Status", result, "success")
        }

        $(".preloader").hide();
        return false;
    });

    return false;
}

/***Paging functions * End***/