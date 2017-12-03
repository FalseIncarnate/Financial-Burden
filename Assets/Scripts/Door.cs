using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable_Object {

    internal LevelManager lm;
    public bool vert_door = false;
    public bool is_shop_door = false;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        lm = FindObjectOfType<LevelManager>();
        single_use = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal override void DoDefaultInteract(Inventory inv) {
        if(used) {
            return;
        }
        float newX = transform.position.x;
        float newY = transform.position.y;
        if(vert_door) {
            newY += 1f;
        } else if(is_shop_door) {
            newY -= 3f;
        } else {
            newX += 1f;
        }
        if(is_shop_door) {
            if(lm.build_shop_room(newX, newY)) {
                transform.gameObject.SetActive(false);
                used = true;
                Destroy(gameObject);
            }
        } else {
            if(lm.build_new_room(newX, newY, vert_door)) {
                transform.gameObject.SetActive(false);
                used = true;
                Destroy(gameObject);
            }
        }
    }
}
