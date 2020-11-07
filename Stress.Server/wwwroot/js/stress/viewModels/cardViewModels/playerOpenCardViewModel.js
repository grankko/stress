'use strict';

import { cardViewModel } from './cardViewModel.js';

// Represents one of the four open playable cards
class playerOpenCardViewModel extends cardViewModel {
    constructor(connection, elementId) {
        super(connection, elementId);

        this.enableDragStart();
    }

    // Dragging an open card to one of the stacks will send a signal to the server that the player
    // wants to make a move.
    enableDragStart() {
        this.element.addEventListener('dragstart', function (event) {
            event.dataTransfer.setData("text", event.target.dataset.slot);
        });
    }
}

export { playerOpenCardViewModel };
