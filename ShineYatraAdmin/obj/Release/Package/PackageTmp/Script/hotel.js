$(document).ready(function () {
    $(".preloader").hide();
    $("button[name=viewHotel]").bind("click", ViewHotelDetail);
    $("button[name=bookHotel]").bind("click", BookHotelView);
    $("#cancelBooking").bind("click", CancelHotelBooking);

});

function CancelHotelBooking() {
    $.ajax({
        url: 'GetHotelCancelView',
        type: 'Get',
    }).done(function (result) {
        $("#hotelContent").html("");
        $("#hotelContent").html(result);
    });
}

function SubmitBookingDetail() {
    $("#hoteSearchHotel").html("");
    $(".preloader").show();
    var hotelInfo = $('#bookHotelForm').serialize();
    $.ajax({
        url: 'SubmitBookingDetail',
        type: 'Post',
        datatype: 'Json',
        data: hotelInfo
    }).done(function (result) {
        //  ClearForm("searchHotelForm");
        if (result == "" || result == undefined) {
            $("#hoteSearchHotel").html("There is some error in Hotel Booking");
        }
        else {
            $("#hoteSearchHotel").html(result);
        }

        $(".preloader").hide();
    });

    return false;
}

function SubmitHotelSearch() {
    $("#hoteSearchHotel").html("");
    $(".preloader").show();
    var searchCompanyInfo = $('#searchHotelForm').serialize();
    $.ajax({
        url: 'SearchHotel',
        type: 'Post',
        datatype: 'Json',
        data: searchCompanyInfo
    }).done(function (result) {
        //  ClearForm("searchHotelForm");
        if (result == "" || result == undefined) {
            $("#hoteSearchHotel").html("No Hotel Found");
        }
        else {
            $("#hotelContent").html("");
            $("#hotelContent").html(result);
        }

                $(".preloader").hide();
    });

    return false;
}

function ViewHotelDetail() {
    $(".preloader").show();
    $("#hotelContent").html("");
    var selectHotelId = $(this).attr("hotel-id");
    var currentlyRoomsLeft = $(this).attr("room-left");
    var selectHotelWebservice = $(this).attr("hotel-webservice");
    if (currentlyRoomsLeft == "0") {
        $("#hoteSearchHotel").html("Room Not Available");
        $(".preloader").hide();
    }
    else {
        $.ajax({
            url: 'GetHotelDetailView',
            type: 'Post',
            data: { hotelId: selectHotelId, hotelWebService: selectHotelWebservice }
        }).done(function (result) {
            if (result == "" || result == undefined) {
                $("#hoteSearchHotel").html("Something went wrong. Please try again later.");
            }
            else {
                $("#hotelContent").html("");
                $("#hotelContent").html(result);
            }

            $(".preloader").hide();
        });
    }

    return false;
}

function BookHotelView() {
    $("#hoteSearchHotel").html("");
    $(".preloader").show();
    var selectedRoomCode = $(this).attr("data-roomcode");
    $.ajax({
        url: 'BookHotelView',
        type: 'Post',
        datatype: 'Json',
        data: { roomCode: selectedRoomCode }
    }).done(function (result) {
        if (result == "" || result == undefined) {
            $("#hoteSearchHotel").html("Something went wrong. Please try again later.");
        }
        else {
            $("#hotelContent").html("");
            $("#hotelContent").html(result);
        }

        $(".preloader").hide();
    });

    return false;
}

function SubmitHotelCancelRequest() {
    $("#hoteSearchHotel").html("");
    $(".preloader").show();
    var cancelInfo = $('#cancelHotelForm').serialize();
    $.ajax({
        url: 'SubmitHotelCancelrequest',
        type: 'Post',
        datatype: 'Json',
        data: cancelInfo
    }).done(function (result) {
        //  ClearForm("searchHotelForm");
        if (result == "" || result == undefined) {
            $("#hoteSearchHotel").html("There is some error in Hotel Booking");
        }
        else {
            $("#hoteSearchHotel").html(result);
        }

        $(".preloader").hide();
    });

    return false;
}
