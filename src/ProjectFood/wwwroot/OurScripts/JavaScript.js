﻿////Defining a listener for our button, specifically, an onclick handler
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
        //$("<tr>").text(message).appendTo("#log");
        var indexNextRow = $('#indexNextRow').html();

        $('#log').append('<tr id="' + indexNextRow + '" role="row"><td id="denna">' + message + '</td><td id="' + indexNextRow + '" onclick="changeDate(' + indexNextRow + ')">Saknar utgång</td><td onclick="removeItem(\'' + message + '\',' +indexNextRow +')">Ta bort</td></tr>');
        var indexNew = parseInt(indexNextRow) + 1;
        $('#indexNextRow').html(indexNew);
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
                        $("#log").tablesorter();
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
            var foodName = $('table#log tr#' + dateIndex).children('#denna').html();


            //ajax för att ändra datum på server sidan
            $.post({
                url: "/User/SaveExpireDate",
                dataType: "json",
                data: {
                    date: date,
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

function removeItem(foodName, id) {
    $('table#log tr#' + id).remove();
    //ajax för att ändra datum på server sidan
    $.post({
        url: "/User/RemoveFood",
        dataType: "json",
        data: {
            foodName: foodName
        },
        success: function (data) {
            console.log(data);
        }
    });
    //ajax för ta bort mat ingrediens
}

function changeDate(id) {
    $('#datepicker').show();
    $('#selectedDateIndex').val(id);
}

$(document).ready(function () {
    $("#log").tablesorter();
}
);
