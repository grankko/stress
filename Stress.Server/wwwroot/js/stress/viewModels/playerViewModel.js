'use strict';

import { $ } from '../utils/viewUtils.js';
import { playerHandCardViewModel } from './cardViewModels/playerHandCardViewModel.js';
import { playerOpenCardViewModel } from './cardViewModels/playerOpenCardViewModel.js';

// Represents one of the players
class playerViewModel {
    constructor(connection, isPlayer) {
        this.isPlayer = isPlayer;           // Indicates if the model represents the player or the opponent
        this.elementTemplate = '';          // Prefix with elementTemplate and concatenate to get elements in a player agonstic way
        this.connection = connection;

        if (isPlayer)
            this.elementTemplate = 'player';
        else
            this.elementTemplate = 'opponent';
        
        this.cardSlot1 = new playerOpenCardViewModel(connection, `${this.elementTemplate}Slot1`);
        this.cardSlot2 = new playerOpenCardViewModel(connection, `${this.elementTemplate}Slot2`);
        this.cardSlot3 = new playerOpenCardViewModel(connection, `${this.elementTemplate}Slot3`);
        this.cardSlot4 = new playerOpenCardViewModel(connection, `${this.elementTemplate}Slot4`);

        this.hand = new playerHandCardViewModel(connection, `${this.elementTemplate}Hand`, isPlayer);
        this.hand.setModel('closed');
    }

    setModel(model) {
        this.cardSlot1.setModel(model.cardSlot1);
        this.cardSlot2.setModel(model.cardSlot2);
        this.cardSlot3.setModel(model.cardSlot3);
        this.cardSlot4.setModel(model.cardSlot4);

        $(`${this.elementTemplate}HandCardsLeft`).innerText = model.cardsLeft;
        $(`${this.elementTemplate}InfoLabel`).innerText = model.nickName;
    }

    // Called when the player 'request for draw' state is changed
    toggleDrawRequested(isRequested) {
        this.hand.toggleDrawRequestedStyle(isRequested);
    }
}

export { playerViewModel };
