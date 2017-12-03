using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGun : Item {

    internal bool is_firing = false;
    internal int ammo_cost = 1;
    public GameObject proj;

    protected int fire_rate = 10;   //number of updates before we can fire again.
    internal bool is_reloading = false;
    protected int update_cycle = 0;

	// Use this for initialization
	void Start () {
        name = "coin gun";
        held_update = true;
	}

    internal override void OnInteract(Inventory inv) {
        if(is_firing || is_reloading) {
            return;
        }
        if(inv.coins < ammo_cost) {
            inv.gm.ui.InteractText("You need coins to fire!");
            return;
        }
        OpenFire(inv);
    }

    internal virtual void OpenFire(Inventory inv) {
        Vector3 newTraj = inv.GetTargetDir(new Vector3(0f, 0f, 0f));
        GameObject fired_coin = Instantiate(proj, inv.holder.transform.position, Quaternion.identity);
        ProjScript proj_script = fired_coin.GetComponent<ProjScript>();
        proj_script.enabled = true;
        proj_script.origin_collider = inv.holder_collider;

        if(!proj_script.SetTraj(newTraj)){
            Destroy(fired_coin);
            Debug.Log("failed to set new trajectory");
            return;
        }
        is_firing = true;
        inv.AdjustCoins(-ammo_cost);
        proj_script.Aim();
        is_firing = false;
        is_reloading = true;
    }

    // Update is called once per frame
    void Update() {

    }

    internal override void HeldUpdate() {
        if(is_reloading) {
            update_cycle++;
            if(update_cycle > fire_rate) {
                is_reloading = false;
                update_cycle = 0;
            }
        }
	}
}
