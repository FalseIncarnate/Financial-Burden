using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int banked_coins = 0;
    internal Inventory player_inv;
    internal LevelManager lm;
    internal CanvasScript ui;

	// Use this for initialization
	void Start () {
        lm = FindObjectOfType<LevelManager>();
        ui = FindObjectOfType<CanvasScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void BankCoins(int amount = 0) {
        banked_coins += amount;
        ui.UpdateUI();
    }

    internal bool SpendBankedCoins(int amount = 0) {
        if(banked_coins >= amount) {    //if we can spend from just the bank, do so
            BankCoins(-amount);
            return true;
        }else if (banked_coins + player_inv.coins >= amount) {  //if we can't foot the bill with JUST banked coins, spend banked coins first, then draw the rest from our pockets
            int pocket_change = amount - banked_coins;
            BankCoins(-banked_coins);
            player_inv.AdjustCoins(-pocket_change);
            return true;
        } else {    //not enough between bank and inventory to cover this cost
            return false;
        }
    }

}
