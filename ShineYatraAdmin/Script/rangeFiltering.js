/**
 * Filter a specific numeric column on the value being between two given 
 * numbers. Note that you will likely need to change the id's on the inputs
 * and the column in which the numeric value is given.
 *
 *  @summary Filter the data between two numbers (inclusive)
 *  @name Range filtering (numbers)
 *  @author [Allan Jardine](http://sprymedia.co.uk)
 *
 *  @example
 *    $(document).ready(function() {
 *        // Initialise datatables
 *        var table = $('#example').DataTable();
 *         
 *        // Add event listeners to the two range filtering inputs
 *        $('#min').keyup( function() { table.draw(); } );
 *        $('#max').keyup( function() { table.draw(); } );
 *    } );
 */

//jQuery.fn.dataTableExt.afnFiltering.push(
//	function (oSettings, aData, iDataIndex) {
//	    var iColumn = 5;
//	    var iMin = document.getElementById('minprice').value * 1;
//	    var iMax = document.getElementById('maxprice').value * 1;

//	    var iVersion = aData[iColumn] == "-" ? 0 : aData[iColumn] * 1;
//	    if (iMin === "" && iMax === "") {
//	        return true;
//	    }
//	    else if (iMin === "" && iVersion < iMax) {
//	        return true;
//	    }
//	    else if (iMin < iVersion && "" === iMax) {
//	        return true;
//	    }
//	    else if (iMin < iVersion && iVersion < iMax) {
//	        return true;
//	    }
//	    return false;
//	}
//);

jQuery.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var min = parseFloat(jQuery('#minprice').html(), 10);
        var max = parseFloat(jQuery('#maxprice').html(), 10);
        var price = parseFloat(data[6]) || 0; // use data for the price column

        if ((isNaN(min) && isNaN(max)) ||
             (isNaN(min) && price <= max) ||
             (min <= price && isNaN(max)) ||
             (min <= price && price <= max)) {
            return true;
        }
        return false;
    }
);