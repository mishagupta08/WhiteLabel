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
                        footer: true,
                        text : 'Export to Excel',
                        filename: function(){
                            var d = new Date();
                            var n = d.getTime();
                            return 'LedgerReport_' + n;
                        },
                        exportOptions: {
                            columns: [0,1,2,3,4,5,6,7,8,9,10,11]
                        }
                   }
                ],
                "aaData": jsonobject,
                "aoColumns": [                   
                    { "mData": "wallet_txn_date"},                    		
                    { "mData": "statement_date"},
                    { "mData": "service_id" },
                    { "mData": "ledger_name" },
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
                "footerCallback": function () {
                    var api = this.api();
                     $(api.column(6).footer()).html('Total:');
                     $(api.column(7).footer()).html(api.column(7, { filter: 'applied' }).data().sum());
                     $(api.column(8).footer()).html(api.column(8, { filter: 'applied' }).data().sum());                     
                     var last_row = api.row(':last').data();
                     $(api.column(9).footer()).html(last_row.balance + " " + last_row.drcr);
                },
                "columnDefs": [
                                   { className: "text-right", "targets": [6,7, 8, 9,11] },
                                   { "targets": [1, 2, 3], "visible": false }
                ],
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