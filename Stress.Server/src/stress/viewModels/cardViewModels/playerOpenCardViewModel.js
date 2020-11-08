'use strict';

import { cardViewModel } from './cardViewModel.js';
import interact from 'interactjs';

// Represents one of the four open playable cards
class playerOpenCardViewModel extends cardViewModel {
    constructor(connection, elementId, isPlayer) {
        super(connection, elementId);

        if (isPlayer)
            this.enableDragStart();
    }

    // Dragging an open card to one of the stacks will send a signal to the server that the player
    // wants to make a move.
    enableDragStart() {

        function dragMoveListener(event) {
            var target = event.target,
                // keep the dragged position in the data-x/data-y attributes
                x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx,
                y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

            // translate the element
            target.style.webkitTransform =
                target.style.transform =
                'translate(' + x + 'px, ' + y + 'px)';

            // update the position attributes
            target.setAttribute('data-x', x);
            target.setAttribute('data-y', y);
        }

        function dragEndListener(event) {
            var target = event.target;
            target.style.webKitTransform =
                target.style.transform = 'none';

            target.setAttribute('data-x', 0);
            target.setAttribute('data-y', 0);
        }

        interact(this.element).draggable({
            onmove: dragMoveListener,
            onend: dragEndListener
        });
    }
}

export { playerOpenCardViewModel };
