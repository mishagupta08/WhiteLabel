
function SubmitFlightSearch() {
    $(".preloader").show();
    var searchCompanyInfo = $('#searchFlightForm').serialize();
    $.ajax({
        url: 'SearchFlight',
        type: 'Post',
        data: searchCompanyInfo
    }).done(function (result) {
        ClearForm("SearchFlight");
        $(".preloader").hide();
        return false;
    });

    return false;
}

function viewRule(divId)
{
    $("#"+divId).toggle();
}