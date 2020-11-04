// Client state
var playerNumber = 0;           // Client needs to keep track of player number and game session
var currentSessionKey = '';     // Session key and signalR group key
var gameStarted = false;
var playerWantsToDraw = false;
var openingDrawOccured = false;

document.addEventListener('DOMContentLoaded', function () {

    // Setup signalR Hub connection
    var connection = new signalR.HubConnectionBuilder().withUrl("/stressHub").build();

    connection.on('infoMessage', function (message) {
        console.log('info message received: ' + message);
    });

    connection.on('gameStateChanged', function (state) {
        console.log('gameStateChanged message received.');
        if (!gameStarted) {
            showGame(state);
            gameStarted = true;
        }

        updateBoardFromServerState(state);
    });

    // todo: we need to recover connection if lost
    connection.start()
        .then(function () {
            console.log('connection started.');

            // When a card is dropped on a stack, send action to server
            function drop(event) {
                event.preventDefault();

                // Don't allow drop if the game has not had it's first draw
                if (openingDrawOccured) {
                    var cardSlot = event.dataTransfer.getData("text");
                    var isLeftStack = event.target.dataset.leftstack;
                    event.dataTransfer.clearData();

                    connection.invoke('playerPlaysCardOnStack', currentSessionKey, playerNumber, parseInt(cardSlot), (isLeftStack === 'true'));
                }
            }

            $('leftStack').addEventListener('drop', drop);
            $('rightStack').addEventListener('drop', drop);

            $('playerHand').addEventListener('click', function (event) {
                if (!playerWantsToDraw) {
                    $('playerHand').style.opacity = '0.5';
                    connection.invoke('playerWantsToDraw', currentSessionKey, playerNumber);
                }
            });

            // todo: implement stress button and send event to Hub

            $('createGameButton').addEventListener('click', function (event) {

                var nickName = $('create_nickName').value;

                connection.invoke('createGameSession', nickName).then(function (sessionKey) {
                    $('gameCode').innerText = `Game key: ${sessionKey}`;
                    $('joinGamePanel').style.display = 'none';
                    $('loadingPanel').style.display = 'block';
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

function updateBoardFromServerState(state) {

    // Draw executed on server, clear draw state
    if (state.drawExecuted) {
        $('playerHand').style.opacity = '1';
        $('opponentHand').style.opacity = '1';
        openingDrawOccured = true;
    }

    if (state.drawRequestedByPlayer > 0 && state.drawRequestedByPlayer !== playerNumber) // Opponent requested draw
        $('opponentHand').style.opacity = '0.5';


    // Server model does not know if the receiving client is player one or player two.
    // Turn the table accordingly.
    var playerOneElementIdTemplate = 'playerSlot';
    var playerTwoElementIdTemplate = 'opponentSlot';
    if (playerNumber === 2) {
        playerOneElementIdTemplate = 'opponentSlot';
        playerTwoElementIdTemplate = 'playerSlot';
    }

    updateCardSlotFromState(`${playerOneElementIdTemplate}1`, state.playerOneState.cardSlot1);
    updateCardSlotFromState(`${playerOneElementIdTemplate}2`, state.playerOneState.cardSlot2);
    updateCardSlotFromState(`${playerOneElementIdTemplate}3`, state.playerOneState.cardSlot3);
    updateCardSlotFromState(`${playerOneElementIdTemplate}4`, state.playerOneState.cardSlot4);

    updateCardSlotFromState(`${playerTwoElementIdTemplate}1`, state.playerTwoState.cardSlot1);
    updateCardSlotFromState(`${playerTwoElementIdTemplate}2`, state.playerTwoState.cardSlot2);
    updateCardSlotFromState(`${playerTwoElementIdTemplate}3`, state.playerTwoState.cardSlot3);
    updateCardSlotFromState(`${playerTwoElementIdTemplate}4`, state.playerTwoState.cardSlot4);

    updateCardSlotFromState('leftStack', state.leftStackTopCard);
    updateCardSlotFromState('rightStack', state.rightStackTopCard);

    if (playerNumber === 1) {
        $('playerHandCardsLeft').innerText = state.playerOneState.cardsLeft;
        $('opponentHandCardsLeft').innerText = state.playerTwoState.cardsLeft;
    } else {
        $('opponentHandCardsLeft').innerText = state.playerOneState.cardsLeft;
        $('playerHandCardsLeft').innerText = state.playerTwoState.cardsLeft;
    }
}

// Set color and card character of card slot
function updateCardSlotFromState(elementId, card) {
    $(elementId).innerText = cardShortHandJsRepresentation.get(card).char;
    $(elementId).style.color = cardShortHandJsRepresentation.get(card).color;
    $(elementId).dataset.cardShortHand = card;
}

// Hide join/create panel and show game panel
function showGame(state) {
    for (let el of document.querySelectorAll('.startPanel')) el.style.visibility = 'hidden';
    for (let el of document.querySelectorAll('.gamePanel')) el.style.visibility = 'visible';

    $('opponentHand').innerText = cardShortHandJsRepresentation.get(null).char; // Closed card character
    $('playerHand').innerText = cardShortHandJsRepresentation.get(null).char;

    if (playerNumber === 1) {
        $('playerInfoLabel').innerText = state.playerOneState.nickName;
        $('opponentInfoLabel').innerText = state.playerTwoState.nickName;
    } else {
        $('playerInfoLabel').innerText = state.playerTwoState.nickName;
        $('opponentInfoLabel').innerText = state.playerOneState.nickName;
    }

}

// Utils
function $(x) { return document.getElementById(x); }

function allowDrop(ev) {
    ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.dataset.slot);
}

// translate short representation of card to card character
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