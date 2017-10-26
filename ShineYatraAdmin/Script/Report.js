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
        if (jsonobject !== "") {
            $('#LedgerList').DataTable({
                "aaData": jsonobject,
                "aoColumns": [
                    {"mData": "wallet_txn_date"},
                    { "mData": "op_ledger_name" },
                    { "mData": "service_name" },
                    { "mData": "wallet_txn_id" },
                    { "mData": "debit" },
                    { "mData": "credit" },
                    {
                        "mData": "balance",                        
                        "mRender": function (data, type, full) {
                            return full.balance + " " + full.drcr;
                        }
                    },
                    { "mData": "remarks" },
                    { "mData": "ref_txn_id" }
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