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

    setModel(model) {
        super.setModel(model);

        // Reset css transition animation after card has been dropped on stack
        this.element.classList.remove('playingCard');
        this.element.classList.add('playableCard');

    }

    // Dragging an open card to one of the stacks will send a signal to the server that the player
    // wants to make a move.
    enableDragStart() {

        function dragStartListener(event) {
            var target = event.target;

            // Removes css transition on hover while dragging
            target.classList.remove('playableCard');
            target.classList.add('playingCard');

            target.style.opacity = '0.5';
        }

        function dragMoveListener(event) {
            var target = event.target,
                // keep the dragged position in the data-x/data-y attributes
                x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx,
                y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

            // move card by applying transform
            target.style.webkitTransform =
                target.style.transform =
                'translate(' + x + 'px, ' + y + 'px)';

            // update the position attributes
            target.setAttribute('data-x', x);
            target.setAttribute('data-y', y);
        }

        function dragEndListener(event) {
            var target = event.target;

            target.style.opacity = '1';

            // move back the card when dropped and reset previous recorded delta in x/y
            target.style.removeProperty('webkitTransform');
            target.style.removeProperty('transform');

            target.setAttribute('data-x', 0);
            target.setAttribute('data-y', 0);
        }

        interact(this.element).draggable({
            onstart: dragStartListener,
            onmove: dragMoveListener,
            onend: dragEndListener
        });
    }
}

export { playerOpenCardViewModel };
