using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boots : Item {

    internal float speed_boost = 2.5f;

	// Use this for initialization
	void Start () {
        name = "speed boots";
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
