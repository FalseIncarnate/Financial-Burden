using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SaleCounter : Interactable_Object {

    public GameObject[] possible_wares;

    public GameObject myWares;
    public int min_cost = 0;
    public int max_cost = 0;

    public bool restocks = false;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        if(!myWares) {
            possible_wares = Resources.LoadAll<GameObject>("ShopItems");
            int index = (int)Random.Range(0, possible_wares.Length);
            myWares = possible_wares[index];
        }
        is_moneysink = true;
        accepts_bank = true;
        int my_cost = min_cost;
        if(max_cost > min_cost) {
            my_cost = (int) Random.Range(min_cost, max_cost);
        }
        coin_cost = my_cost;
        if(!restocks) {
            single_use = true;
        }
        restock();
	}

    internal void restock() {
        myWares = Instantiate(myWares, transform.position, Quaternion.identity);
    }
	
    internal override void DoInteract(Inventory inv) {
        myWares.transform.position = inv.holder.transform.position;
        if(restocks) {
            restock();
        } else {
            myWares = null;
            used = true;
        }
    }

    internal override void DoDefaultInteract(Inventory inv) {
        gm.ui.InteractText("This item costs " + coin_cost.ToString() + " coins.");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
