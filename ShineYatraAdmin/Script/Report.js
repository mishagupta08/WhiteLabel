function GetLedgerList() {
    $(".preloader").show();
    var ledgerfilter = $("#LegderFliter").serialize();
    $.ajax({
        url: 'GetLedgerList',
        type: 'Post',
        datatype: 'Json',
        async: false,
        data: ledgerfilter
    }).done(function (result) {
        $(".preloader").hide();
        $("#Usercontainer").html(result);
        return false;
    }).fail(function (xhr) {
        alert(xhr);
    });
    return false;
}