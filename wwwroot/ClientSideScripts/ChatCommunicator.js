const connection = new signalR.HubConnectionBuilder().withUrl("/chatService").build();

console.log(connection)

$(document).ready(function () {
  
        let userName = prompt('Enter Your Name');
        $('#userName').val(userName);

})


connection.start().then(function () {
        console.log("Connection started");
    }).catch(function (err) {
        return console.error(err.toString());
});

connection.on("joinedRoom", function (message) {
    alert(message);
});

connection.on("roomCreated", function (message, roomName) {
    createRoomMarkup(roomName);
});

connection.on("receiveMessage", function (message) {
    alert(message);
});

connection.on("notifyUsers", function (message) {
    alert(message);
});



function createRoomMarkup(roomName) {
    $('#tableBody').append(`<tr><td>${roomName}</td><td><button id="joinBtn" data-attr="${roomName}" class="btn btn-info">Join</button></td></tr>`)
}

function createRoom() {
    var result = validateAndGetInfo();
    if (result) {
        connection.invoke("createRoom", result.userName.val(), result.roomName.val()).catch(function (err) {
            return console.error(err.toString());
        });
        result.roomName.val('');
        result.userName.val('');
    }
}

function JoinRoom(arg) {
    let userName = $('#userName').val();
    let roomName = $(arg).data('room');
    connection.invoke('joinRoom', userName, roomName).catch((err) => {
        return console.error(err.toString());
    });
}


function validateAndGetInfo() {
    const roomName = $('#roomName');
    const userName = $('#userName');
    if (roomName.val() == '' || userName.val() == '') {
        alert('Fill All Fields');
        return null;
    }
    return { roomName, userName }
}