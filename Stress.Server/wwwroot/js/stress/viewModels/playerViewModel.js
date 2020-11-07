import { $ } from '../utils/viewUtils.js';
import { cardViewModel } from './cardViewModel.js';

class playerViewModel {
    constructor(player, isPlayer) {

        var elementTemplate;
        if (isPlayer)
            elementTemplate = 'opponentSlot';
        else
            elementTemplate = 'playerSlot';
        
        this.cardSlot1 = new cardViewModel(`${elementTemplate}1`, player.cardSlot1, isPlayer);
        this.cardSlot2 = new cardViewModel(`${elementTemplate}2`, player.cardSlot2, isPlayer);
        this.cardSlot3 = new cardViewModel(`${elementTemplate}3`, player.cardSlot3, isPlayer);
        this.cardSlot4 = new cardViewModel(`${elementTemplate}4`, player.cardSlot4, isPlayer);

        if (isPlayer)
            this.hand = new cardViewModel('playerHand', 'closed', false);
        else
            this.hand = new cardViewModel('opponentHand', 'closed', false);
    }
}

export { playerViewModel };
