

function changeBillServiceProviders() {
    var selectedBillServiceprovider = $("#bill_service_providers").val();
    $(".preloader").show();
    var selectedBillService = $("#bill_service").val();
    $("#billServiceStructure").html("Loading please wait...");
    $.ajax({
        url: '/BillService/GetBillServiceProvidersFields',
        type: 'GET',
        data: { selectedBillService: selectedBillService, selectedBillServiceprovider: selectedBillServiceprovider }
    }).done(function (result) {
        $("#billServiceStructure").html(result);
        $(".preloader").hide();
    }).error(function (err) {
        alert(err);
    });
}

function getSubZoneList(zoneid)
{
    $(".preloader").show();
    $("#eroList").html("");
    $.ajax({
        url: '/BillService/GetSubZoneList',
        type: 'GET',
        data: { zoneid: zoneid }
    }).done(function (result) {
        var optionlist = "";
        optionlist = optionlist + "<option value=''>--Please Select--</option>";
        $(result).each(function () {           
            optionlist = optionlist + "<option value = " + this.sub_zone_id + ">" + this.sub_zone_name + "</option>";
        });                           
        $("#eroList").append(optionlist);
        $(".preloader").hide();
    }).error(function (err) {
        alert(err);
    });
}