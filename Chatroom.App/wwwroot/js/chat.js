"use strict";

// Add a message to the chat window.
function WriteMessage(userName, message, msgtype, time) {
    var text = "";

    if (msgtype == "Command") {
        text = '[' + time + '] Bot says: ' + htmlEncode(message);
    }
    else {
        text = '[' + time + '] ' + htmlEncode(userName) + ' says: ' + htmlEncode(message);
    }

    var li = document.createElement("li");
    li.textContent = text;
    document.getElementById("messagesList").appendChild(li);
}

// Html-encode messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessageFromServer", function (user, message, msgtype, time) {
    WriteMessage(user, message, msgtype, time);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    connection.invoke("Connected").then(function(messages) {
        for (var i = 0; i < messages.length; i++) {
            WriteMessage(messages[i].owner, messages[i].textMessage, "", messages[i].time);
            }
        })
        .catch(function (err) {
        return console.error(err.toString());
    });

}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = '';
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});