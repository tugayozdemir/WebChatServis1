$("#btnLoginModal").off('click');
$("#btnLoginModal").on('click', function () {
    $.post("/Home/Login", {
        UserName: $("#userName").val()
    }, function (data, textStatus) {
        if (data.IsSuccess) {
            $.get("/Home/LoginData", "", function (loginData, loginTextStatus) {
                location.href = 'http://localhost:16862/Home/Rooms';
                $("userForm").html(loginData);

            }, "json");
        }
        else{
            $("#msg").html(data.ExMessage);
        }

    }, "json");



});


$("#roomAdd").off('click');
$("#roomAdd").on('click', function () {
    $.post("/Home/CreateRoom", {
        roomName: $("#roomText").val()
    }, function (roomData, textStatus) {
        if (roomData.IsSuccess) {
            location.href = 'http://localhost:16862/Home/Rooms';

        }
        else {
            alert(roomData.ExMessage);
        }

    }, "json");
  
});




















