﻿$(function () {
    function logFood(message) {
        var indexNextRow = $('#indexNextRow').html();

        $("#log tbody").prepend('<tr id="' + indexNextRow + '" role="row"><td id="denna">' + message + '</td><td id="' + indexNextRow + '"><span class="point" onclick="changeDate(' + indexNextRow + ')">Ej inlagt</span></td><td><span id="remove" onclick="removeItem(\'' + message + '\',' + indexNextRow + ')" class="glyphicon glyphicon-remove pull-right point"></span></td></tr>');
        $('.tablesorter').trigger('update');
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
                        $('#myFood').val('');
                    }
                }
            });
        }
    });
});
function pickDate (id, oldDate) {
    $("#datepicker" + id ).datepicker({
        dateFormat: 'yy-mm-dd',
        onSelect: function (dateText, inst) {
            var date = $(this).val();
            $('table#log tr#' + id).children('#' + id).prop('outerHTML', '<td id="' + id + '" onclick="changeDate(' + id + ',\'' + date + '\')">' + date + '</td>');

            //plocka fram userfooditem
            var foodName = $('table#log tr#' + id).children('#denna').html();

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
        },
        onClose: function (dateText) {
            if (dateText == "") {
                $('table#log tr#' + id).children('#' + id).prop('outerHTML', '<td id="' + id + '" onclick="changeDate(' + id + ',\'' + oldDate + '\')">' + oldDate + '</td>');

                //plocka fram userfooditem
                var foodName = $('table#log tr#' + id).children('#denna').html();
            }
        }
    });
};

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
}

function changeDate(id,date) {

    $('table#log tr#' + id).children('#' + id).prop('outerHTML', '<td id="' + id + '"><input type="text" class="form-control" style="width: 80px" id="datepicker' + id + '"></td>');
    $('#datepicker' + id ).show();
    $('#selectedDateIndex').val(id);
    pickDate(id,date);
    $('#datepicker' + id).focus();

}

$(document).ready(function () {
    $("#log").tablesorter();
}
);