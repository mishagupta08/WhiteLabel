
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

function displayReturn(type)
{
    //var type = $("#modeDropDown").val();
    if (type == "ONE") {
        $("#ReturnDateField").hide();
        $("#returnDate").prop('required', false);
    }
    else {
        $("#ReturnDateField").show();
        $("#returnDate").prop('required', true);
    }
}