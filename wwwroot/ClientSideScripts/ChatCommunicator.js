const connection = new signalR.HubConnectionBuilder().withUrl("/chatService").build();
console.log(connection)

connection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(function () {
    var userName = localStorage.getItem("userName");
    if (!userName) {
        userName = prompt('Enter Your Name');
        localStorage.setItem("userName", userName);
    }
     $('#userName').val(userName);
})

connection.on("joinedRoom", function (message) {
    alert(message);
});

connection.on("roomCreated", function (response) {
    createRoomMarkup(response[1]);
    $.jGrowl(response[0], {
        header: 'Notification',
        theme: 'bg-success'
    });
});

connection.on("newRoomCreated", function (response) {
    createRoomMarkup(response[1]);
});

connection.on("receiveMessage", function (message) {
    alert(message);
});

connection.on("notifyUsers", function (message) {
    alert(message);
});

function createRoomMarkup(markUp) {
    $('#tableBody').append(markUp)
}

function createRoom() {
    var result = validateAndGetInfo();
    if (result) {
        connection.invoke("CreateRoom", result.userName.val(), result.roomName.val()).catch(function (err) {
            return console.error(err.toString());
        });
        result.roomName.val('');
        result.userName.prop('readonly', 'true');
    }
}

function JoinRoom(room) {
    let userName = $('#userName').val();
    connection.invoke('joinRoom', userName, room).catch((err) => {
        return console.error(err.toString());
    });
}

function validateAndGetInfo() {
    const roomName = $('#roomName');
    const userName = $('#userName');
    if (roomName.val() == '' || userName.val() == '') {
        alert('Fill All Fields');
        userName.removeAttr('readonly').focus();
        return null;
    }
    return { roomName, userName }
}