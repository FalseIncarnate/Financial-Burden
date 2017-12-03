using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyBank : Interactable_Object {
    
	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal override void DoDefaultInteract(Inventory inv) {
        AttemptBank(inv);
    }

    internal void AttemptBank(Inventory inv) {
        int deposit = inv.coins;
        gm.BankCoins(deposit);
        inv.AdjustCoins(-deposit);
    }
}
