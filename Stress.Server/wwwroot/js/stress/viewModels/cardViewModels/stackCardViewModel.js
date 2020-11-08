'use strict';

import { cardViewModel } from './cardViewModel.js';

// Represents the top card of one of the stacks the player can play their open cards on.
class stackCardViewModel extends cardViewModel {
    constructor(connection, elementId, isLeftStack) {
        super(connection, elementId);

        this.isLeftStack = isLeftStack;

        this.enableDrop();
        this.enableDragOver();
    }

    // When a player drops a card on the stack, signal is sent to the server that the player wants to make this move
    enableDrop() {

        var me = this;

        function droppedListener(event) {
            var droppedCard = event.relatedTarget;
            var cardSlot = parseInt(droppedCard.dataset.slot);
            me.connection.invoke('playerPlaysCardOnStack', me.connection.sessionKey, me.connection.playerNumber, cardSlot, me.isLeftStack);
        }

        interact(this.element).dropzone({
            ondrop: droppedListener
        });

    }

    // Make the stack droppable
    enableDragOver() {
        this.element.addEventListener('dragover', function (event) {
            event.preventDefault();
        });
    }
}

export { stackCardViewModel };
