'use strict';

import { cardViewModel } from './viewModels/cardViewModel.js'
import { playerViewModel } from './viewModels/playerViewModel.js'
import { startGameViewModel } from './viewModels/startGameViewModel.js'
import { $ } from './utils/viewUtils.js';

class mainViewModel {
    constructor(connection) {
        this.connection = connection;
        this.startGameViewModel = null;
        this.playerViewModel = null;
        this.opponentViewModel = null;
        this.gameBoardVisible = false;

        this.leftStackViewModel = null;
        this.rightStackViewModel = null;

        this.assignEventListeners();
    }

    // Hide join/create panel and show game panel
    showGame() {
        for (let el of document.querySelectorAll('.startPanel')) el.style.visibility = 'hidden';
        for (let el of document.querySelectorAll('.gamePanel')) el.style.visibility = 'visible';
    }

    showWinner(winnerName) {
        this.connection.openingDrawOccured = false;
        this.playerViewModel.toggleDrawRequested(false);
        this.opponentViewModel.toggleDrawRequested(false);

        this.leftStackViewModel.setModel(null);
        this.rightStackViewModel.setModel(null);

        $('playerWonPanel').style.display = 'block';
        $('playerWonLabel').innerText = `${winnerName} won!`;
    }

    rematch() {
        $('playerWonPanel').style.display = 'none';
        $('playerWonLabel').innerText = '';
        $('restartGameButton').textContent = 'Play again?'
    }

    assignEventListeners() {
        var me = this;
        $('callStressEventButton').addEventListener('click', function (event) {
            if (me.connection.openingDrawOccured)
                me.connection.invoke('playerCallsStress', me.connection.sessionKey, me.connection.playerNumber);
        });
        $('restartGameButton').addEventListener('click', function (event) {
            $('restartGameButton').textContent = 'Waiting..'
            me.connection.invoke('requestNewGame', me.connection.sessionKey, me.connection.playerNumber);
        });

    }
};

document.addEventListener('DOMContentLoaded', function () {
    // Setup signalR Hub connection
    var connection = new signalR.HubConnectionBuilder().withUrl("/stressHub").build();
    var vm = new mainViewModel(connection);

    connection.on('gameStateChanged', function (state) {
        console.log('gameStateChanged message received.');

        if (!vm.gameBoardVisible)
            vm.showGame();

        if (vm.leftStackViewModel == null)
            vm.leftStackViewModel = new cardViewModel(connection, 'leftStack')
        if (vm.rightStackViewModel == null)
            vm.rightStackViewModel = new cardViewModel(connection, 'rightStack')

        vm.leftStackViewModel.setModel(state.leftStackTopCard);
        vm.rightStackViewModel.setModel(state.rightStackTopCard);

        // First server state changed received
        if (connection.playerNumber === 1) {
            if (vm.playerViewModel == null) {
                vm.playerViewModel = new playerViewModel(connection, true);
                vm.opponentViewModel = new playerViewModel(connection, false);
            }
            vm.playerViewModel.setModel(state.playerOneState);
            vm.opponentViewModel.setModel(state.playerTwoState);

        } else if (connection.playerNumber === 2) {
            if (vm.playerViewModel == null) {
                vm.playerViewModel = new playerViewModel(connection, true);
                vm.opponentViewModel = new playerViewModel(connection, false);
            }
            vm.playerViewModel.setModel(state.playerTwoState);
            vm.opponentViewModel.setModel(state.playerOneState);

        } else {
            console.error('No player number set');
        }

        if (state.drawRequestedByPlayer > 0 && state.drawRequestedByPlayer !== connection.playerNumber)
            vm.opponentViewModel.toggleDrawRequested(true);

        if (state.drawExecuted) {
            vm.playerViewModel.toggleDrawRequested(false);
            vm.opponentViewModel.toggleDrawRequested(false);
            connection.openingDrawOccured = true;
        }

        if (state.winnerName != null && state.winnerName.length > 0)
            vm.showWinner(state.winnerName);

        if (state.rematchStarted)
            vm.rematch();
    });


    connection.start().then(function () {
        console.log('connection started.');

        vm.startGameViewModel = new startGameViewModel(connection, vm);

    });
});