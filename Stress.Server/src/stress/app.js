'use strict';

import { stackCardViewModel } from './viewModels/cardViewModels/stackCardViewModel.js'
import { playerViewModel } from './viewModels/playerViewModel.js'
import { startGameViewModel } from './viewModels/startGameViewModel.js'
import { $ } from './utils/viewUtils.js';
import * as signalR from '@microsoft/signalr';

class mainViewModel {
    constructor(connection) {
        // signalR connection. Also carries common game state across view models
        this.connection = connection;

        this.gameBoardVisible = false;
        this.firstGameStateReceived = false;

        this.startGameViewModel = null;
        this.playerViewModel = null;
        this.opponentViewModel = null;
        this.leftStackViewModel = null;
        this.rightStackViewModel = null;

        this.assignEventListeners();
    }

    assignEventListeners() {
        var me = this;

        $('callStressEventButton').addEventListener('click', function (event) {
            // Allow to call for stress event if the first move of the game has been made
            if (me.connection.openingDrawOccured)
                me.connection.invoke('playerCallsStress', me.connection.sessionKey, me.connection.playerNumber);
        });

        $('restartGameButton').addEventListener('click', function (event) {
            // Signals that this player wants a rematch
            $('restartGameButton').textContent = 'Waiting..'
            me.connection.invoke('requestNewGame', me.connection.sessionKey, me.connection.playerNumber);
        });
    }

    // Hide join/create panel and show game panel
    showGame() {
        for (let el of document.querySelectorAll('.startPanel')) el.style.display = 'none';
        for (let el of document.querySelectorAll('.gamePanel')) el.style.display = 'grid';
    }

    // Show winner panel and resets some state for a potential rematch
    showWinner(winnerName) {
        this.connection.openingDrawOccured = false;
        this.playerViewModel.toggleDrawRequested(false);
        this.opponentViewModel.toggleDrawRequested(false);

        this.leftStackViewModel.setModel(null);
        this.rightStackViewModel.setModel(null);

        $('playerWonPanel').style.display = 'block';
        $('playerWonLabel').innerText = `${winnerName} won!`;
    }

    // Rematch initatied, reset visual state of the winner panel
    rematch() {
        $('playerWonPanel').style.display = 'none';
        $('playerWonLabel').innerText = '';
        $('restartGameButton').textContent = 'Play again?'
    }

    // Initializes view models when first server game state has been received
    initialize() {
        this.showGame();

        this.leftStackViewModel = new stackCardViewModel(this.connection, 'leftStack', true)
        this.rightStackViewModel = new stackCardViewModel(this.connection, 'rightStack', false)

        this.firstGameStateReceived = true;

        if (this.connection.playerNumber === 1) {
            this.playerViewModel = new playerViewModel(this.connection, true);
            this.opponentViewModel = new playerViewModel(this.connection, false);
        } else if (this.connection.playerNumber === 2) {
            this.playerViewModel = new playerViewModel(this.connection, true);
            this.opponentViewModel = new playerViewModel(this.connection, false);
        } else {
            throw 'Invalid player number.';
        }
    }
};

document.addEventListener('DOMContentLoaded', function () {
    // Setup signalR Hub connection
    var connection = new signalR.HubConnectionBuilder().withUrl("/stressHub").build();
    var vm = new mainViewModel(connection);

    // Only type of message communicated by server, indicating that the server game state
    // has changed and clients needs to update.
    connection.on('gameStateChanged', function (state) {
        console.log('gameStateChanged message received.');

        if (!vm.firstGameStateReceived)
            vm.initialize()

        // Update stacks with server state
        vm.leftStackViewModel.setModel(state.leftStackTopCard);
        vm.rightStackViewModel.setModel(state.rightStackTopCard);

        // Update players models with server state.
        // Server does not know if this client is player one or player two.
        if (connection.playerNumber === 1) {
            vm.playerViewModel.setModel(state.playerOneState);
            vm.opponentViewModel.setModel(state.playerTwoState);
        } else if (connection.playerNumber === 2) {
            vm.playerViewModel.setModel(state.playerTwoState);
            vm.opponentViewModel.setModel(state.playerOneState);
        }

        // Server signaling that the opponent has requested a draw event
        if (state.drawRequestedByPlayer > 0 && state.drawRequestedByPlayer !== connection.playerNumber)
            vm.opponentViewModel.toggleDrawRequested(true);

        // Server sinaling that a draw event has executed
        if (state.drawExecuted) {
            vm.playerViewModel.toggleDrawRequested(false);
            vm.opponentViewModel.toggleDrawRequested(false);
            connection.openingDrawOccured = true;
        }

        // Server signaling that someone won the game
        if (state.winnerName != null && state.winnerName.length > 0)
            vm.showWinner(state.winnerName);

        // Server signaling that a rematch has started
        if (state.rematchStarted)
            vm.rematch();
    });

    connection.start().then(function () {
        console.log('connection started.');

        vm.startGameViewModel = new startGameViewModel(connection, vm);
    });
});