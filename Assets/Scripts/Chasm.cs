using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasm : Interactable_Object {

    public Sprite bridged_sprite;
    public SpriteRenderer spriteRenderer;

    internal bool is_bridged = false;
    internal BoxCollider2D boxcollider;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        boxcollider = gameObject.GetComponent<BoxCollider2D>();
        requires_item = true;
	}

    internal override bool CheckItem(Inventory inv) {
        if(!base.CheckItem(inv)) {
            return false;
        } else {
            PlanksScript held_planks = inv.heldObject.GetComponent<PlanksScript>();
            if(!held_planks) {
                return false;
            }
            return true;
        }
    }

    internal override void DoInteract(Inventory inv) {
        is_bridged = true;
        spriteRenderer.sprite = bridged_sprite;
        boxcollider.enabled = false;
        inv.Expend();
    }

    internal override void DoDefaultInteract(Inventory inv) {
        if(is_bridged) {
            gm.ui.InteractText("This chasm is already bridged.");
        } else {
            gm.ui.InteractText("You need wood planks to bridge this!");
        }
        return;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
