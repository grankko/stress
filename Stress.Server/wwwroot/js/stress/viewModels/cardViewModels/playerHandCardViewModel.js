'use strict';

import { cardViewModel } from './cardViewModel.js';

// Represents the top of the stack of the players closed cards on hand.
class playerHandCardViewModel extends cardViewModel {
    constructor(connection, elementId, isPlayersHand) {
        super(connection, elementId);

        this.drawSignaled = false;

        // Clicking the opponents card should not signal a draw event
        if (isPlayersHand)
            this.enableSignalDrawClick();
        
    }

    // Sends a signal to the server that the player wants to draw from his closed stack.
    // For a draw event to occur, both players must signal.
    enableSignalDrawClick() {
        var me = this;

        this.element.addEventListener('click', function (event) {
            if (!me.drawSignaled) {
                me.toggleDrawRequestedStyle(true);
                me.connection.invoke('playerWantsToDraw', me.connection.sessionKey, me.connection.playerNumber);
            }
        });
    }

    toggleDrawRequestedStyle(isRequested) {
        if (isRequested)
            this.element.style.opacity = '0.5';
        else
            this.element.style.opacity = '1';
    }
}

export { playerHandCardViewModel };
