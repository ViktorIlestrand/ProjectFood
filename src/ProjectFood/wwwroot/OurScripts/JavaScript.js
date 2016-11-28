////Defining a listener for our button, specifically, an onclick handler
//document.getElementById("addFood").onclick = function () {
//    //First things first, we need our text:
//    var text = document.getElementById("newFood").value; //.value gets input values

//    var node = document.createElement("li");
//    var textNode = document.createTextNode(text);
//    node.appendChild(textNode);

//    //Now use appendChild and add it to the list!
//    document.getElementById("log").appendChild(node);
//}

$(function () {
    function logFood(message) {
        $("<tr>").text(message).prependTo("#log");
        $("#log").scrollTop(0);
    }

    $("#myFood").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/User/FoodQuery?query=" + $("#myFood").val(),
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            console.log(ui.item.value);
            $.post({
                url: "/User/SaveFood",
                dataType: "json",
                data: {
                    food: ui.item.value
                },
                success: function (data) {
                    console.log(data);
                    if (data === "Added") {
                        logFood(ui.item.value);
                        //logOptions();
                        $('#myFood').val('');
                    }
                }
            });
        }
    });
});
$(function () {
    $("#datepicker").datepicker({
        dateFormat: 'yy-mm-dd',
        onSelect: function (dateText, inst) {
            var date = $(this).val();
            var dateIndex = $('#selectedDateIndex').val();
            $('table#log tr#' + dateIndex).children('#' + dateIndex).html(date.toString());
            $("#datepicker").hide();
            //plocka fram userfooditem
            var foodName = $('table#log tr#' + dateIndex).children('#' + 1).val();

            //ajax för att ändra datum på server sidan
            $.ajax({
                url: "/User/SaveExpireDate",
                dataType: "json",
                data: {
                    expireDate: date,
                    foodName: foodName
                },
                success: function (data) {
                    console.log(data);
                }
            });
        }
    });
    $("#datepicker").hide();
});
function doStuff(id) {
    //markering

    //options för att lägga till expiry date

    //delete
}

function removeItem(id) {
    $('table#log tr#' + id).remove();
    //ajax för ta bort mat ingrediens
}

function changeDate(id) {
    $('#datepicker').show();
    $('#selectedDateIndex').val(id);

}