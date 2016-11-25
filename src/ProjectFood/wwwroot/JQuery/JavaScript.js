//Defining a listener for our button, specifically, an onclick handler
document.getElementById("addFood").onclick = function () {
    //First things first, we need our text:
    var text = document.getElementById("newFood").value; //.value gets input values

    var node = document.createElement("li");
    var textNode = document.createTextNode(text);
    node.appendChild(textNode);
    //Now use appendChild and add it to the list!
    document.getElementById("myKitchenList").appendChild(node);
}