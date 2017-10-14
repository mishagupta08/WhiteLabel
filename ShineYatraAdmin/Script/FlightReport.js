function confirmCancel(transid, partnerRefernceId, txnid) {

    swal({
        title: "Are you sure?",
        text: "Do you really want to canel this ticket?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, Cancel it!",
        closeOnConfirm: false
    }, function (isConfirm) {
        if (!isConfirm) return;
        $.ajax({
            url: "CancelFlight",
            type: "POST",
            async: false,
            data: { transId: transid, partnerRefernceId: partnerRefernceId, txnid: txnid },
            success: function (result) {
                if (result === "success") {
                    swal({
                        title: "Success",
                        text: "Ticket Cancelled Successfully",
                        type: "success"
                    }, function () {
                        location.reload();
                    });
                } else {
                    swal("Error in Cancelation!", "Please try again", "error");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                swal("Error in Cancelation!", "Please try again", "error");
            }
        });
    });
}

function GetFlightList() {
    $(".preloader").show();
    var filter = $("#FlightListFliter").serialize();
    $.ajax({
        url: 'GetFlightList',
        type: 'Post',
        datatype: 'Json',
        async: false,
        data: filter
    }).done(function (result) {
        $(".preloader").hide();
        $("#Usercontainer").html(result);
        return false;
    }).fail(function (xhr) {
        alert(xhr);
    });
    return false;
}
