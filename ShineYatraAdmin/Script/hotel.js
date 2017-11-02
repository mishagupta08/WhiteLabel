var oTable;
$(document).ready(function () {
    $(".preloader").hide();
    if ($("div[name=fareBreakup]").length > 0) {
        $("div[name=fareBreakup]").bind("click", function () {
            var count = $(this).attr("data-count");
            var id = "#fareBreakupTable" + count;
            $(id).fadeToggle("slow");
        });
    }

    $("button[name=viewHotel]").unbind();
    $("button[name=viewHotel]").bind("click", ViewHotelDetail);
    $("button[name=bookHotel]").unbind();
    $("button[name=bookHotel]").bind("click", BookHotelView);
    $("#cancelBooking").unbind();
    $("#cancelBooking").bind("click", CancelHotelBooking);

    /*Hotel list filter*/

    $('#searchHotelForm').unbind();
    $('#searchHotelForm').bind("submit", function () {
        $(".preloader").show();
    });

    oTable = $('#HotelListContainer').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                column
                    .search('', true, false)
                    .draw();
            });
        },
        "destroy": true,
        "bLengthChange": false,
        "language": {
            "paginate": {
                "previous": "<<",
                "next": ">>"
            }
        },
        //"aoColumnDefs": [
        //  { 'bSortable': false, 'aTargets': [1] }
        //],
        stateSave: true
    });

    $(".flat").change(function () {
        //build a regex filter string with an or(|) condition
        var types = $('input:checkbox[name="rating"]:checked').map(function () {
            return this.value;
        }).get().join('|');
        //filter in column 0, with an regex, no smart filtering, no inputbox,not case sensitive
        oTable
        .columns(0)
        .search('^' + types + '$', true, false)
        .draw();
    });

    $("SELECT").bind("change", function (e) { });

    ChildRowInitialization();
    ParentRowInitialization();
    $("#roomCountDropDown").change(function (e) {
        ParentRowInitialization();
    });

    $('[id^=childCountDropDown]').change(function (e) {
        ChildRowInitialization();
    });
});

function ParentRowInitialization() {
    var rowId = 0;
    if ($("roomCountDropDown").length > 0) {
        var rowId = document.getElementById("roomCountDropDown").selectedIndex;
    }

    $("div[name=guestRow").hide();
    //$('[id^=childCountDropDown]').prop('selectedIndex', 0);
    for (var i = 0; i <= rowId; i++) {
        var guestRowId = "#guestRow" + (i + 1);
        $(guestRowId).show();
    }
}

function ChildRowInitialization() {
    $("div[name=childRow").hide();
    var rowId = document.getElementById("roomCountDropDown").selectedIndex;
    var childDopdownId = "";
    for (var i = 0; i <= rowId; i++) {
        childDopdownId = "childCountDropDown-" + (i + 1);
        var childId = document.getElementById(childDopdownId).selectedIndex;
        for (var j = 0; j < childId; j++) {
            var childRowId = "#childRow-" + (j + 1) + (i + 1);
            $(childRowId).show();
        }
    }
}

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
    //SubmitBookingDetail
    $.ajax({
        url: 'BookResponse',
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
        var cookieIdValue = $("#hotelRequestCookieId").val();
        $(".preloader").show();
        ///*Set parameter*/
        //var cookieName = ('HotelId' + cookieIdValue);
        //document.cookie = cookieName + "=" + selectHotelId + ";";

        //cookieName = ('webservice' + cookieIdValue);
        //document.cookie = cookieName + "=" + selectHotelWebservice + ";";

        //cookieName = ('cookieId');
        //document.cookie = cookieName + "=" + cookieId + ";";

        window.location.href = '/Hotel/GetHotelDetailView?hotelId=' + selectHotelId + '&hotelWebService=' + selectHotelWebservice + '&cookieId=' + cookieIdValue;

        //window.location = window.location.href.split("?")[0];
        //$.ajax({
        //    url: 'GetHotelDetailView',
        //    type: 'Post',
        //    data: { hotelId: selectHotelId, hotelWebService: selectHotelWebservice, cookieId: cookieIdValue }
        //}).done(function (result) {
        //    if (result == "" || result == undefined) {
        //        $("#hoteSearchHotel").html("Something went wrong. Please try again later.");
        //    }
        //    else {
        //        $("#hotelContent").html("");
        //        $("#hotelContent").html(result);
        //    }

        //    $(".preloader").hide();
        //});
    }

    return false;
}

function BookHotelView() {
    $("#hoteSearchHotel").html("");
    $(".preloader").show();
    var selectedRoomCode = $(this).attr("data-roomcode");
    var cookieId = $("#hotelRequestCookieId").val();
    //$.ajax({
    //    url: 'BookHotelView',
    //    type: 'Post',
    //    datatype: 'Json',
    //    data: { roomCode: , hotelRequestCookieId: cookieId }
    //}).done(function (result) {
    //    if (result == "" || result == undefined) {
    //        $("#hoteSearchHotel").html("Something went wrong. Please try again later.");
    //    }
    //    else {
    //        $("#hotelContent").html("");
    //        $("#hotelContent").html(result);
    //    }

    //    $(".preloader").hide();
    //});

    window.location.href = '/Hotel/BookHotelView?roomCode=' + selectedRoomCode + '&hotelRequestCookieId=' + cookieId;

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
