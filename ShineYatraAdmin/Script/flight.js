var oTable;
var maxvalue;
var minvalue;
$(document).ready(function () {
    
    oTable = $('#flightsearchResult').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;                
                        column
                            .search('', true, false)
                            .draw();
                    });                           
        },
        "destroy":true,              
        "bPaginate": false,
        "order":[],
        "aoColumnDefs": [      
             {'bSortable': false, 'aTargets': [0,1,2,4,5,7]},             
             {
                 "aTargets": [0],
                 "mData": function (source, type, val) {
                     if (type === 'set') {
                         var airlinename = val.split("<breakit>");
                         source.airlinedisplay = val,
                         source.airlinefilter = airlinename[0];
                         return;
                     }
                     else if (type === 'display') {
                         return source.airlinedisplay;
                     }
                     else if (type === 'filter') {
                         return source.airlinefilter;
                     }
                     // 'sort', 'type' and undefined all just use the integer
                     return source.airlinefilter;
                 }
             },
         {
             "aTargets": [3],
             "mData": function (source, type, val) {
                 if (type === 'set') {                     
                     source.durationdisplay = val;
                     source.duration = parseFloat(val);
                     return;
                 }
                 else if (type === 'display') {
                     return source.durationdisplay;
                 }
                 else if (type === 'filter') {
                     return source.duration;
                 }
                 // 'sort', 'type' and undefined all just use the integer
                 return source.duration;
             }
         },
         {
             "aTargets": [6],
             "mData": function (source, type, val) {
                 if (type === 'set') {
                     var arr = val.split("<breakit>");
                     source.pricedisplay = val;
                     source.price = parseFloat(arr[0]);
                     return;
                 }
                 else if (type === 'display') {
                     return source.pricedisplay;
                 }
                 else if (type === 'filter') {
                     return source.price;
                 }
                 // 'sort', 'type' and undefined all just use the integer
                 return source.price;
             }
         }
        ],

        stateSave: true
    });     

    var airlineNames = oTable.column(0).data().unique().sort();
    var airlinechkbox = "";
    for (var i = 0 ; i < airlineNames.length; i++)
    {
        airlinechkbox += "<label><input type=\"checkbox\" checked name=\"type\" class=\"airlinetype\" value=\"" + airlineNames[i] + "\">&nbsp;" + airlineNames[i] +"</label><br />";
    }

    $("#AirlineNameCheckbox").html(airlinechkbox);

    var stops = oTable.column(4).data().unique().sort();
    var stopschkbox = "";
    for (var i = 0 ; i < stops.length; i++) {
        stopschkbox += "<label><input type=\"checkbox\" checked name=\"Stops\" class=\"Stops\" value=\"" + stops[i] + "\">&nbsp;" + stops[i] + "</label><br />";
    }

    $("#stopsCheckbox").html(stopschkbox);
       
    var price = oTable.column(6).data().unique().join(',');  

    var ids = price.split(',').map(parseFloat);
    
    maxvalue = Math.max.apply(Math, ids);
    minvalue = Math.min.apply(Math, ids);

   
    $(".airlinetype").change(function () {
        //build a regex filter string with an or(|) condition
        var types = $('input:checkbox[name="type"]:checked').map(function () {
            return this.value;
        }).get().join('|');
        //filter in column 0, with an regex, no smart filtering, no inputbox,not case sensitive
        oTable
        .columns(0)
        .search('^' + types + '$', true, false)
        .draw();                
    });

    $(".Stops").change(function () {
        //build a regex filter string with an or(|) condition
        var types = $('input:checkbox[name="Stops"]:checked').map(function () {
            return this.value;
        }).get().join('|');
        //filter in column 0, with an regex, no smart filtering, no inputbox,not case sensitive
        oTable
        .columns(4)
        .search('^' + types + '$', true, false)
        .draw();
    });

});


function modifyflightsearch(divId)
{
    $("#" + divId).toggle();
}

