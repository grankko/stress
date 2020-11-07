'use strict';

import { $ } from '../utils/viewUtils.js';
import { cardViewModel } from './cardViewModel.js';

class playerViewModel {
    constructor(connection, isPlayer) {
        this.isPlayer = isPlayer;
        this.elementTemplate = '';
        this.connection = connection;

        if (isPlayer)
            this.elementTemplate = 'player';
        else
            this.elementTemplate = 'opponent';
        
        this.cardSlot1 = new cardViewModel(connection, `${this.elementTemplate}Slot1`, isPlayer);
        this.cardSlot2 = new cardViewModel(connection, `${this.elementTemplate}Slot2`, isPlayer);
        this.cardSlot3 = new cardViewModel(connection, `${this.elementTemplate}Slot3`, isPlayer);
        this.cardSlot4 = new cardViewModel(connection, `${this.elementTemplate}Slot4`, isPlayer);

        this.hand = new cardViewModel(connection, `${this.elementTemplate}Hand`, isPlayer);
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

    toggleDrawRequested(isRequested) {
        this.hand.toggleDrawRequestedStyle(isRequested);
    }
}

export { playerViewModel };
