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
    function log(message) {
        $("<div>").text(message).prependTo("#log");
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
            log(ui.item.value);
            console.log(ui.item.value);
            $.post({
                url: "/User/SaveFood",
                dataType: "json",
                data: {
                    food: ui.item.value
                },
                success: function (data) {
                    console.log(data)
                }
            });

        }
    });
});