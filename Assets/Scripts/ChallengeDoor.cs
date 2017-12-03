using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChallengeDoor : Door {

    public string color = "";

	// Use this for initialization
	protected override void Start () {
        base.Start();
        vert_door = true;
        if((int)Random.Range(0, 100) < 50) {
            requires_item = true;
        } else {
            coin_cost = (int)Random.Range(0, 30);
            is_moneysink = true;
        }
        if(requires_item && color == "") {
            int choice = (int) Random.Range(1, 3);
            if(choice == 1) {
                color = "red";
            } else if(choice == 2) {
                color = "blue";
            } else {
                color = "green";
            }
        }
        
	}

    internal override bool CheckItem(Inventory inv) {
        if(!base.CheckItem(inv)) {
            return false;
        } else {
            KeyScript held_key = inv.heldObject.GetComponent<KeyScript>();
            if(!held_key) {
                return false;
            }
            if(held_key.color != color) {
                return false;
            }
            return true;
        }
    }

    internal override void DoInteract(Inventory inv) {
        base.DoDefaultInteract(inv);
        if(requires_item) {
            inv.Expend();
        }
    }

    internal override void DoDefaultInteract(Inventory inv) {
        if(requires_item) {
            gm.ui.InteractText("This door requires a " + color + " key!");
        } else {
            gm.ui.InteractText("This door requires " + coin_cost.ToString() + " coins!");
        }
        return;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
