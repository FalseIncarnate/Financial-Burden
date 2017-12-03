using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : Item {

    public int value = 1;

    // Use this for initialization
    void Start() {
        item_name = "Money";
        is_consumable = true;
    }

    internal override void Consume(GameObject consumer, Inventory inv) {
        inv.AdjustCoins(value);
        base.Consume(consumer, inv);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
