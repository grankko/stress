function $(x) { return document.getElementById(x); }
function showGame() {
    for (let el of document.querySelectorAll('.startPanel')) el.style.visibility = 'hidden';
    for (let el of document.querySelectorAll('.gamePanel')) el.style.visibility = 'visible';
}

document.addEventListener('DOMContentLoaded', function () {

    var connection = new signalR.HubConnectionBuilder().withUrl("/stressHub").build();

    connection.on('gameStateUpdated', function (gameState) {
        console.log('game state changed received');
        // todo: everything
    });

    connection.on('infoMessage', function (message) {
        console.log('info message received: ' + message);
    });

    connection.on('gameStateChanged', function (state) {
        console.log('gameStateChanged message received.');
        showGame();
    });

    // todo: we need to recover connection if lost
    connection.start()
        .then(function () {
            console.log('connection started.');

            // todo: implement drag and drop from open card to stacks and send event to Hub

            // todo: implement stress button and send event to Hub

            // todo: implement "signal I want to draw" button

            $('createGameButton').addEventListener('click', function (event) {

                var nickName = $('create_nickName').value;

                connection.invoke('createGameSession', nickName).then(function (sessionKey) {
                    $('gameCode').innerText = sessionKey;
                    $('joinGamePanel').style.visibility = 'hidden';
                    $('createGameButton').disabled = true;
                });

                event.preventDefault();
            });

            $('joinGameButton').addEventListener('click', function (event) {

                var nickName = $('join_nickName').value;
                var sessionKey = $('join_sessionKey').value;

                connection.invoke('joinGameSession', nickName, sessionKey).then(function () {
                    showGame();
                });

                event.preventDefault();
            });

        })
        .catch(error => {
            console.error(error.message);
        });
});