function GetTransactionListDetail() {
    $(".preloader").show();
    var ledgerfilter = $("#hotelFilterForm").serialize();
    $.ajax({
        url: 'GetHotelTransactionList',
        type: 'Post',
        datatype: 'Json',
        async: false,
        data: ledgerfilter
    }).done(function (result) {
        $(".preloader").hide();
        var jsonobject = jQuery.parseJSON(result);
        if (jsonobject !== "") {
            $('#LedgerList').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        footer: true,
                        text: 'Export to Excel',
                        filename: function () {
                            var d = new Date();
                            var n = d.getTime();
                            return 'LedgerReport_' + n;
                        },
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                        }
                    }
                ],
                "aaData": jsonobject,
                "aoColumns": [
                    { "mData": "mobile" },
                    { "mData": "email" },
                    { "mData": "hotel_name" },
                    { "mData": "hotel_city" },
                    { "mData": "check_in_date" },
                    { "mData": "check_out_date" },
                    { "mData": "txn_date" },
                    { "mData": "amount" },
                    {
                        "mData": "txn_id",
                        "mRender": function (data, type, full) {
                            //var colValue = '<a href="@Url.Action(GetInvoice, Hotel , new { txnId=' + full.txn_id + '})">' + full.txn_id + '</a>';
                            var colValue = '<a href="/Hotel/GetInvoice?txnId=' + full.txn_id + '">' + full.txn_id + '</a>';
                            return colValue;
                        }
                    },
                    { "mData": "status" },
                ],
                //"footerCallback": function () {
                //    var api = this.api();
                //    $(api.column(6).footer()).html('Total:');
                //    $(api.column(7).footer()).html(api.column(7, { filter: 'applied' }).data().sum());
                //    $(api.column(8).footer()).html(api.column(8, { filter: 'applied' }).data().sum());
                //    var last_row = api.row(':last').data();
                //    $(api.column(9).footer()).html(last_row.balance + " " + last_row.drcr);
                //},
                "columnDefs": [
                                   { className: "text-right", "targets": [6, 7, 8, 9] },
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
            $(".preloader").hide();
        }

    }).fail(function (xhr) {
        $(".preloader").hide();
        alert(xhr);
    });
    return false;
}