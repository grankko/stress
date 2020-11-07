'use strict';

import { $ } from '../../utils/viewUtils.js';

// Abstract view model of a card, common logic for all different card types
class cardViewModel {
    constructor(connection, elementId) {
        this.element = $(elementId);
        this.connection = connection;
        
        if (new.target === cardViewModel) {
            throw new TypeError("Cannot construct cardViewModel instances directly");
        }
    }

    setModel(model) {
        if (model == null) {
            this.element.innerText = '';
        } else {
            this.element.innerText = cardShortHandJsRepresentation.get(model).char;
            this.element.style.color = cardShortHandJsRepresentation.get(model).char;
            this.element.dataset.cardShortHand = model;
        }
    }
}

export { cardViewModel };

// Translates short string representation of card to card character
const cardShortHandJsRepresentation = new Map();
cardShortHandJsRepresentation.set("S14", { char: "\uD83C\uDCA1", color: "#000000" });
cardShortHandJsRepresentation.set("S2", { char: "\uD83C\uDCA2", color: "#000000" });
cardShortHandJsRepresentation.set("S3", { char: "\uD83C\uDCA3", color: "#000000" });
cardShortHandJsRepresentation.set("S4", { char: "\uD83C\uDCA4", color: "#000000" });
cardShortHandJsRepresentation.set("S5", { char: "\uD83C\uDCA5", color: "#000000" });
cardShortHandJsRepresentation.set("S6", { char: "\uD83C\uDCA6", color: "#000000" });
cardShortHandJsRepresentation.set("S7", { char: "\uD83C\uDCA7", color: "#000000" });
cardShortHandJsRepresentation.set("S8", { char: "\uD83C\uDCA8", color: "#000000" });
cardShortHandJsRepresentation.set("S9", { char: "\uD83C\uDCA9", color: "#000000" });
cardShortHandJsRepresentation.set("S10", { char: "\uD83C\uDCAA", color: "#000000" });
cardShortHandJsRepresentation.set("S11", { char: "\uD83C\uDCAB", color: "#000000" });
cardShortHandJsRepresentation.set("S12", { char: "\uD83C\uDCAD", color: "#000000" });
cardShortHandJsRepresentation.set("S13", { char: "\uD83C\uDCAE", color: "#000000" });
cardShortHandJsRepresentation.set("H14", { char: "\uD83C\uDCB1", color: "#FF0000" });
cardShortHandJsRepresentation.set("H2", { char: "\uD83C\uDCB2", color: "#FF0000" });
cardShortHandJsRepresentation.set("H3", { char: "\uD83C\uDCB3", color: "#FF0000" });
cardShortHandJsRepresentation.set("H4", { char: "\uD83C\uDCB4", color: "#FF0000" });
cardShortHandJsRepresentation.set("H5", { char: "\uD83C\uDCB5", color: "#FF0000" });
cardShortHandJsRepresentation.set("H6", { char: "\uD83C\uDCB6", color: "#FF0000" });
cardShortHandJsRepresentation.set("H7", { char: "\uD83C\uDCB7", color: "#FF0000" });
cardShortHandJsRepresentation.set("H8", { char: "\uD83C\uDCB8", color: "#FF0000" });
cardShortHandJsRepresentation.set("H9", { char: "\uD83C\uDCB9", color: "#FF0000" });
cardShortHandJsRepresentation.set("H10", { char: "\uD83C\uDCBA", color: "#FF0000" });
cardShortHandJsRepresentation.set("H11", { char: "\uD83C\uDCBB", color: "#FF0000" });
cardShortHandJsRepresentation.set("H12", { char: "\uD83C\uDCBD", color: "#FF0000" });
cardShortHandJsRepresentation.set("H13", { char: "\uD83C\uDCBE", color: "#FF0000" });
cardShortHandJsRepresentation.set("D14", { char: "\uD83C\uDCC1", color: "#FF0000" });
cardShortHandJsRepresentation.set("D2", { char: "\uD83C\uDCC2", color: "#FF0000" });
cardShortHandJsRepresentation.set("D3", { char: "\uD83C\uDCC3", color: "#FF0000" });
cardShortHandJsRepresentation.set("D4", { char: "\uD83C\uDCC4", color: "#FF0000" });
cardShortHandJsRepresentation.set("D5", { char: "\uD83C\uDCC5", color: "#FF0000" });
cardShortHandJsRepresentation.set("D6", { char: "\uD83C\uDCC6", color: "#FF0000" });
cardShortHandJsRepresentation.set("D7", { char: "\uD83C\uDCC7", color: "#FF0000" });
cardShortHandJsRepresentation.set("D8", { char: "\uD83C\uDCC8", color: "#FF0000" });
cardShortHandJsRepresentation.set("D9", { char: "\uD83C\uDCC9", color: "#FF0000" });
cardShortHandJsRepresentation.set("D10", { char: "\uD83C\uDCCA", color: "#FF0000" });
cardShortHandJsRepresentation.set("D11", { char: "\uD83C\uDCCB", color: "#FF0000" });
cardShortHandJsRepresentation.set("D12", { char: "\uD83C\uDCCD", color: "#FF0000" });
cardShortHandJsRepresentation.set("D13", { char: "\uD83C\uDCCE", color: "#FF0000" });
cardShortHandJsRepresentation.set("C14", { char: "\uD83C\uDCD1", color: "#000000" });
cardShortHandJsRepresentation.set("C2", { char: "\uD83C\uDCD2", color: "#000000" });
cardShortHandJsRepresentation.set("C3", { char: "\uD83C\uDCD3", color: "#000000" });
cardShortHandJsRepresentation.set("C4", { char: "\uD83C\uDCD4", color: "#000000" });
cardShortHandJsRepresentation.set("C5", { char: "\uD83C\uDCD5", color: "#000000" });
cardShortHandJsRepresentation.set("C6", { char: "\uD83C\uDCD6", color: "#000000" });
cardShortHandJsRepresentation.set("C7", { char: "\uD83C\uDCD7", color: "#000000" });
cardShortHandJsRepresentation.set("C8", { char: "\uD83C\uDCD8", color: "#000000" });
cardShortHandJsRepresentation.set("C9", { char: "\uD83C\uDCD9", color: "#000000" });
cardShortHandJsRepresentation.set("C10", { char: "\uD83C\uDCDA", color: "#000000" });
cardShortHandJsRepresentation.set("C11", { char: "\uD83C\uDCDB", color: "#000000" });
cardShortHandJsRepresentation.set("C12", { char: "\uD83C\uDCDD", color: "#000000" });
cardShortHandJsRepresentation.set("C13", { char: "\uD83C\uDCDE", color: "#000000" });

cardShortHandJsRepresentation.set("closed", { char: "\uD83C\uDCA0", color: "#000000" });