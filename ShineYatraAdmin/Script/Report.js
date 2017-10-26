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
        var jsonobject = jQuery.parseJSON(result);        
        if (jsonobject != "") {
            $('#LedgerList').DataTable({
                "aaData": jsonobject,
                "aoColumns": [
                    {"mDataProp": "wallet_txn_date"},
                    {"mDataProp": "op_ledger_name" },
                    {"mDataProp": "service_name" },
                    {"mDataProp": "wallet_txn_id"},
                    {"mDataProp": "debit"},
                    {"mDataProp": "credit"},
                    {"mDataProp": "balance"},
                    {"mDataProp": "remarks" },
                    { "mDataProp": "ref_txn_id" }
                ],
                initComplete: function() {
                    this.api().columns().every(function() {
                        var column = this;
                        column
                            .search('', true, false)
                            .draw();
                    });
                },
                "destroy": true,
                "paging": false,
                "ordering": false,
                "bLengthChange": false,
                "language": {
                    "paginate": {
                        "previous": "<<",
                        "next": ">>"
                    }
                },
                stateSave: true
            });
        }
        $(".preloader").hide();
    }).fail(function (xhr) {
        alert(xhr);
    });
    return false;
}