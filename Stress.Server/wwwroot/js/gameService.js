function $(x) { return document.getElementById(x); }
function showGame() {
    for (let el of document.querySelectorAll('.startPanel')) el.style.visibility = 'hidden';
    for (let el of document.querySelectorAll('.gamePanel')) el.style.visibility = 'visible';
}
var playerNumber = 0;
var currentSessionKey = '';

document.addEventListener('DOMContentLoaded', function () {

    var connection = new signalR.HubConnectionBuilder().withUrl("/stressHub").build();

    connection.on('infoMessage', function (message) {
        console.log('info message received: ' + message);
    });

    connection.on('gameStateChanged', function (state) {
        console.log('gameStateChanged message received.');
        showGame();
        updateBoardFromServerState(state);
    });

    // todo: we need to recover connection if lost
    connection.start()
        .then(function () {
            console.log('connection started.');

            function drop(event) {
                event.preventDefault();
                var cardSlot = event.dataTransfer.getData("text");
                var isLeftStack = event.target.dataset.leftstack;
                event.dataTransfer.clearData();

                connection.invoke('executePlayerAction', currentSessionKey, playerNumber, parseInt(cardSlot), (isLeftStack === 'true'));
            }

            $('leftStack').addEventListener('drop', drop);
            $('rightStack').addEventListener('drop', drop);

            // todo: implement drag and drop from open card to stacks and send event to Hub

            // todo: implement stress button and send event to Hub

            // todo: implement "signal I want to draw" button

            $('createGameButton').addEventListener('click', function (event) {

                var nickName = $('create_nickName').value;

                connection.invoke('createGameSession', nickName).then(function (sessionKey) {
                    $('gameCode').innerText = sessionKey;
                    $('joinGamePanel').style.visibility = 'hidden';
                    $('createGameButton').disabled = true;
                    playerNumber = 1;
                    currentSessionKey = sessionKey;
                });

                event.preventDefault();
            });

            $('joinGameButton').addEventListener('click', function (event) {

                var nickName = $('join_nickName').value;
                var sessionKey = $('join_sessionKey').value;
                playerNumber = 2;

                connection.invoke('joinGameSession', nickName, sessionKey).then(function () {
                    currentSessionKey = sessionKey;
                    showGame();
                });

                event.preventDefault();
            });

        })
        .catch(error => {
            console.error(error.message);
        });
});

function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.dataset.slot);
}

function updateBoardFromServerState(state) {
    var playerOneElementIdTemplate = 'playerSlot';
    var playerTwoElementIdTemplate = 'opponentSlot';
    if (playerNumber === 2) {
        playerOneElementIdTemplate = 'opponentSlot';
        playerTwoElementIdTemplate = 'playerSlot';
    }

    $(`${playerOneElementIdTemplate}1`).innerText = cardShortHandJsRepresentation.get(state.playerOneState.cardSlot1).char;
    $(`${playerOneElementIdTemplate}1`).style.color = cardShortHandJsRepresentation.get(state.playerOneState.cardSlot1).color;
    $(`${playerOneElementIdTemplate}1`).dataset.cardShortHand = state.playerOneState.cardSlot1;

    $(`${playerOneElementIdTemplate}2`).innerText = cardShortHandJsRepresentation.get(state.playerOneState.cardSlot2).char;
    $(`${playerOneElementIdTemplate}2`).style.color = cardShortHandJsRepresentation.get(state.playerOneState.cardSlot2).color;
    $(`${playerOneElementIdTemplate}2`).dataset.cardShortHand = state.playerOneState.cardSlot2;

    $(`${playerOneElementIdTemplate}3`).innerText = cardShortHandJsRepresentation.get(state.playerOneState.cardSlot3).char;
    $(`${playerOneElementIdTemplate}3`).style.color = cardShortHandJsRepresentation.get(state.playerOneState.cardSlot3).color;
    $(`${playerOneElementIdTemplate}3`).dataset.cardShortHand = state.playerOneState.cardSlot3;

    $(`${playerOneElementIdTemplate}4`).innerText = cardShortHandJsRepresentation.get(state.playerOneState.cardSlot4).char;
    $(`${playerOneElementIdTemplate}4`).style.color = cardShortHandJsRepresentation.get(state.playerOneState.cardSlot4).color;
    $(`${playerOneElementIdTemplate}4`).dataset.cardShortHand = state.playerOneState.cardSlot4;

    $(`${playerTwoElementIdTemplate}1`).innerText = cardShortHandJsRepresentation.get(state.playerTwoState.cardSlot1).char;
    $(`${playerTwoElementIdTemplate}1`).style.color = cardShortHandJsRepresentation.get(state.playerTwoState.cardSlot1).color;
    $(`${playerTwoElementIdTemplate}1`).dataset.cardShortHand = state.playerTwoState.cardSlot1;

    $(`${playerTwoElementIdTemplate}2`).innerText = cardShortHandJsRepresentation.get(state.playerTwoState.cardSlot2).char;
    $(`${playerTwoElementIdTemplate}2`).style.color = cardShortHandJsRepresentation.get(state.playerTwoState.cardSlot2).color;
    $(`${playerTwoElementIdTemplate}2`).dataset.cardShortHand = state.playerTwoState.cardSlot2;

    $(`${playerTwoElementIdTemplate}3`).innerText = cardShortHandJsRepresentation.get(state.playerTwoState.cardSlot3).char;
    $(`${playerTwoElementIdTemplate}3`).style.color = cardShortHandJsRepresentation.get(state.playerTwoState.cardSlot3).color;
    $(`${playerTwoElementIdTemplate}3`).dataset.cardShortHand = state.playerTwoState.cardSlot3;

    $(`${playerTwoElementIdTemplate}4`).innerText = cardShortHandJsRepresentation.get(state.playerTwoState.cardSlot4).char;
    $(`${playerTwoElementIdTemplate}4`).style.color = cardShortHandJsRepresentation.get(state.playerTwoState.cardSlot4).color;
    $(`${playerTwoElementIdTemplate}4`).dataset.cardShortHand = state.playerTwoState.cardSlot4;

    $('leftStack').innerText = cardShortHandJsRepresentation.get(state.leftStackTopCard).char;
    $('leftStack').style.color = cardShortHandJsRepresentation.get(state.leftStackTopCard).color;

    $('rightStack').innerText = cardShortHandJsRepresentation.get(state.rightStackTopCard).char;
    $('rightStack').style.color = cardShortHandJsRepresentation.get(state.rightStackTopCard).color;
}

const cardShortHandJsRepresentation = new Map();
cardShortHandJsRepresentation.set("S14", { char: "\uD83C\uDCA1", color: "#000000" });
cardShortHandJsRepresentation.set("S2",  { char: "\uD83C\uDCA2", color: "#000000" });
cardShortHandJsRepresentation.set("S3",  { char: "\uD83C\uDCA3", color: "#000000" });
cardShortHandJsRepresentation.set("S4",  { char: "\uD83C\uDCA4", color: "#000000" });
cardShortHandJsRepresentation.set("S5",  { char: "\uD83C\uDCA5", color: "#000000" });
cardShortHandJsRepresentation.set("S6",  { char: "\uD83C\uDCA6", color: "#000000" });
cardShortHandJsRepresentation.set("S7",  { char: "\uD83C\uDCA7", color: "#000000" });
cardShortHandJsRepresentation.set("S8",  { char: "\uD83C\uDCA8", color: "#000000" });
cardShortHandJsRepresentation.set("S9",  { char: "\uD83C\uDCA9", color: "#000000" });
cardShortHandJsRepresentation.set("S10", { char: "\uD83C\uDCAA", color: "#000000" });
cardShortHandJsRepresentation.set("S11", { char: "\uD83C\uDCAB", color: "#000000" });
cardShortHandJsRepresentation.set("S12", { char: "\uD83C\uDCAD", color: "#000000" });
cardShortHandJsRepresentation.set("S13", { char: "\uD83C\uDCAE", color: "#000000" });
cardShortHandJsRepresentation.set("H14", { char: "\uD83C\uDCB1", color: "#FF0000" });
cardShortHandJsRepresentation.set("H2",  { char: "\uD83C\uDCB2", color: "#FF0000" });
cardShortHandJsRepresentation.set("H3",  { char: "\uD83C\uDCB3", color: "#FF0000" });
cardShortHandJsRepresentation.set("H4",  { char: "\uD83C\uDCB4", color: "#FF0000" });
cardShortHandJsRepresentation.set("H5",  { char: "\uD83C\uDCB5", color: "#FF0000" });
cardShortHandJsRepresentation.set("H6",  { char: "\uD83C\uDCB6", color: "#FF0000" });
cardShortHandJsRepresentation.set("H7",  { char: "\uD83C\uDCB7", color: "#FF0000" });
cardShortHandJsRepresentation.set("H8",  { char: "\uD83C\uDCB8", color: "#FF0000" });
cardShortHandJsRepresentation.set("H9",  { char: "\uD83C\uDCB9", color: "#FF0000" });
cardShortHandJsRepresentation.set("H10", { char: "\uD83C\uDCBA", color: "#FF0000" });
cardShortHandJsRepresentation.set("H11", { char: "\uD83C\uDCBB", color: "#FF0000" });
cardShortHandJsRepresentation.set("H12", { char: "\uD83C\uDCBD", color: "#FF0000" });
cardShortHandJsRepresentation.set("H13", { char: "\uD83C\uDCBE", color: "#FF0000" });
cardShortHandJsRepresentation.set("D14", { char: "\uD83C\uDCC1", color: "#FF0000" });
cardShortHandJsRepresentation.set("D2",  { char: "\uD83C\uDCC2", color: "#FF0000" });
cardShortHandJsRepresentation.set("D3",  { char: "\uD83C\uDCC3", color: "#FF0000" });
cardShortHandJsRepresentation.set("D4",  { char: "\uD83C\uDCC4", color: "#FF0000" });
cardShortHandJsRepresentation.set("D5",  { char: "\uD83C\uDCC5", color: "#FF0000" });
cardShortHandJsRepresentation.set("D6",  { char: "\uD83C\uDCC6", color: "#FF0000" });
cardShortHandJsRepresentation.set("D7",  { char: "\uD83C\uDCC7", color: "#FF0000" });
cardShortHandJsRepresentation.set("D8",  { char: "\uD83C\uDCC8", color: "#FF0000" });
cardShortHandJsRepresentation.set("D9",  { char: "\uD83C\uDCC9", color: "#FF0000" });
cardShortHandJsRepresentation.set("D10", { char: "\uD83C\uDCCA", color: "#FF0000" });
cardShortHandJsRepresentation.set("D11", { char: "\uD83C\uDCCB", color: "#FF0000" });
cardShortHandJsRepresentation.set("D12", { char: "\uD83C\uDCCD", color: "#FF0000" });
cardShortHandJsRepresentation.set("D13", { char: "\uD83C\uDCCE", color: "#FF0000" });
cardShortHandJsRepresentation.set("C14", { char: "\uD83C\uDCD1", color: "#000000" });
cardShortHandJsRepresentation.set("C2",  { char: "\uD83C\uDCD2", color: "#000000" });
cardShortHandJsRepresentation.set("C3",  { char: "\uD83C\uDCD3", color: "#000000" });
cardShortHandJsRepresentation.set("C4",  { char: "\uD83C\uDCD4", color: "#000000" });
cardShortHandJsRepresentation.set("C5",  { char: "\uD83C\uDCD5", color: "#000000" });
cardShortHandJsRepresentation.set("C6",  { char: "\uD83C\uDCD6", color: "#000000" });
cardShortHandJsRepresentation.set("C7",  { char: "\uD83C\uDCD7", color: "#000000" });
cardShortHandJsRepresentation.set("C8",  { char: "\uD83C\uDCD8", color: "#000000" });
cardShortHandJsRepresentation.set("C9",  { char: "\uD83C\uDCD9", color: "#000000" });
cardShortHandJsRepresentation.set("C10", { char: "\uD83C\uDCDA", color: "#000000" });
cardShortHandJsRepresentation.set("C11", { char: "\uD83C\uDCDB", color: "#000000" });
cardShortHandJsRepresentation.set("C12", { char: "\uD83C\uDCDD", color: "#000000" });
cardShortHandJsRepresentation.set("C13", { char: "\uD83C\uDCDE", color: "#000000" });

cardShortHandJsRepresentation.set(null, { char: "\uD83C\uDCA0", color: "#000000" });