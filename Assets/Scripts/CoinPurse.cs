using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPurse : Item {

    internal float speed_boost = 1.0f;

	// Use this for initialization
	void Start () {
        name = "greedy bag";
        is_consumable = true;
	}

    internal override void Consume(GameObject consumer, Inventory inv) {
        inv.holder_script.UpdateSpeed(speed_boost);
        base.Consume(consumer, inv);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
