'use strict';

import { cardViewModel } from './viewModels/cardViewModel.js'
import { playerViewModel } from './viewModels/playerViewModel.js'
import { startGameViewModel } from './viewModels/startGameViewModel.js'
import { $ } from './utils/viewUtils.js';

class mainViewModel {
    constructor() {
        this.playerNumber = 0;
        this.sessionKey = 0;

        this.startGameViewModel = null;
        this.playerViewModel = null;
        this.opponentViewModel = null;

        this.leftStackViewModel = null;
        this.rightStackViewModel = null;
    }

    // Hide join/create panel and show game panel
    showGame(state) {
        for (let el of document.querySelectorAll('.startPanel')) el.style.visibility = 'hidden';
        for (let el of document.querySelectorAll('.gamePanel')) el.style.visibility = 'visible';


        // If showGame is triggered by update from server
        if (state != null) {
            if (_playerNumber === 1) {
                $('playerInfoLabel').innerText = state.playerOneState.nickName;
                $('opponentInfoLabel').innerText = state.playerTwoState.nickName;
            } else {
                $('playerInfoLabel').innerText = state.playerTwoState.nickName;
                $('opponentInfoLabel').innerText = state.playerOneState.nickName;
            }
        }
    }
};

var vm = new mainViewModel();

document.addEventListener('DOMContentLoaded', function () {
    // Setup signalR Hub connection
    var connection = new signalR.HubConnectionBuilder().withUrl("/stressHub").build();

    connection.on('gameStateChanged', function (state) {
        console.log('gameStateChanged message received.');

        if (vm.playerNumber === 1) {
            vm.playerViewModel = new playerViewModel(state.playerOneState, true);
            vm.opponentViewModel = new playerViewModel(state.playerTwoState, false);
        } else if (vm.playerNumber === 2) {
            vm.playerViewModel = new playerViewModel(state.playerTwoState, true);
            vm.opponentViewModel = new playerViewModel(state.playerOneState, false);
        } else {
            console.error('No player number set');
        }
            

        //if (!gameStarted) {
        //    showGame(state);
        //    gameStarted = true;
        //}

        //if (state.rematchStarted)
        //    rematch();

        //updateBoardFromServerState(state);

        //if (state.winnerName != null && state.winnerName.length > 0)
        //    showWinner(state.winnerName);
    });


    connection.start().then(function () {
        console.log('connection started.');

        vm.startGameViewModel = new startGameViewModel(connection, vm);

    });
});