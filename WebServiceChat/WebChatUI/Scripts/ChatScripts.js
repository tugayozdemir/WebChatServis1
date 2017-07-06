
function ChatButton(Id) {
    $.get("/Home/GetChat", { userTwoID: Id },
        function (data, TextStatus) {
            $("#chatArea").show();
            if (data != null) {
                $("#tabName").html(data.UserTwo.UserName);
                $(".speakButton").off('click');
                updateChatArea();
            }

        }, "json")
}

$("#sendMessage").off('click');
$("#sendMessage").on('click', function () {

    var messages = $("#writeMessage").val();
    var FromUser = $("#writeMessage").data('fromuser');
    var ToUser = $("#tabName").html();
    $.get("/Home/SendMessage", {
        messages: messages, FromUser: FromUser, ToUser: ToUser
    },
    function (MessageData, TextStatus) {
        if (MessageData != null) {
            $("#writeMessage").val("");
            ShowMessage(MessageData.From.UserName, MessageData.SendDate, MessageData.Text);
            updateChatArea();
                    }
        else {
            $("#writeMessage").val("");
            alert("Mesaj gönderilemedi.Tekrar deneyiniz.");
        }




    }, "json")


});


$(document).ready(function () {
    setIntervalStart();
});


function  setIntervalStart() {
    var refreshId = setInterval(function(){ updateChatArea(); }, 1000);
}


function updateChatArea() {
    var FromUser = $("#writeMessage").data('fromuser');
    var ToUser = $("#tabName").html();

    $.post("/Home/GetChatMessage/", { fromUser: FromUser,toUser:ToUser }, function (dt, TextStatus) {
     
    
        if (Object.keys(dt).length > 0) {
        
            ShowMessage(dt.From.UserName, dt.SendDate, dt.Text);
           
        }
        else {
            $("#chatarea").html("<p>No Messages</p>");
        }



    }, "json")


}

function ShowMessage(UserName, Date, Text) {

    $("#chatarea").append(
             "<p><b>" +
             UserName + "</b> <i>(" +
             Date + ")</i> - " +
             Text + "</p>");

}


