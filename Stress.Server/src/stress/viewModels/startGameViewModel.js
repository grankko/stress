'use strict';

import { $ } from '../utils/viewUtils.js';

// Logic behind the start/join game view
class startGameViewModel {
    constructor(connection, mainViewModel) {
        this.connection = connection;
        this.mainViewModel = mainViewModel;

        this.assignEventHandlers();
    }

    createGameSession() {
        var nickName = $('create_nickName').value;
        var me = this;

        if (me.validateNickName(nickName)) {
            this.connection.invoke('createGameSession', nickName).then(function (result) {
                me.connection.sessionKey = result;
                me.connection.playerNumber = 1; // Creating player is player one

                $('gameCodeLabel').innerText = `Game key: ${result}`;
                $('joinGamePanel').style.display = 'none';
                $('loadingPanel').style.display = 'block';
                $('createGameButton').disabled = true;
            });
        }
    }

    joinGameSession() {
        var nickName = $('join_nickName').value;
        var sessionKeyInput = $('join_sessionKey').value;
        var me = this;

        if (me.validateNickName(nickName)) {
            this.connection.invoke('joinGameSession', nickName, sessionKeyInput);
            me.connection.sessionKey = sessionKeyInput;
            me.connection.playerNumber = 2; // Joining player is player two
        }
    }

    assignEventHandlers() {

        var me = this;

        $('createGameButton').addEventListener('click', function (event) {
            me.createGameSession();
            event.preventDefault();
        });

        $('joinGameButton').addEventListener('click', function (event) {
            me.joinGameSession()
            event.preventDefault();
        });
    }

    // todo: really?
    validateNickName(input) {
        if (input != null && input.length > 2)
            return true;
        return false;
    }

}

export { startGameViewModel };