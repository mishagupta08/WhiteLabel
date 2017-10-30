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
                dom: 'Bfrtip',
                buttons: [
                    {                       
                        extend: 'excelHtml5',
                        text : 'Export to Excel',
                        filename: function(){
                            var d = new Date();
                            var n = d.getTime();
                            return 'LedgerReport_' + n;
                        }
                    }
                ],
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

                "footerCallback": function () {
                    var api = this.api(),
                    columns = [4, 5, 6];
                    for (var i = 0; i < columns.length; i++) {
                        $('tfoot th').eq(columns[i]).html('Total: ' + api.column(columns[i], { filter: 'applied' }).data().sum());                       
                    }
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