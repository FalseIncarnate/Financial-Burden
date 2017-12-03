using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasScript : MonoBehaviour {

    public Text coin_text;
    public Text item_text;
    public Text explore_text;
    public Text interact_text;

    internal GameManager gm;

    protected int rooms = 0;
    protected int shops = 0;
    protected string held_obj = "None";
    protected int held_coins = 0;
    protected int saved_coins = 0;

	// Use this for initialization
	void Start () {
        gm = FindObjectOfType<GameManager>();
        InteractText("");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void UpdateUI() {
        held_coins = gm.player_inv.coins;
        saved_coins = gm.banked_coins;
        held_obj = "None";
        if(gm.player_inv.heldObject) {
            held_obj = gm.player_inv.heldObject.name;
        }
        rooms = gm.lm.room_num - 1;
        shops = gm.lm.shop_num - 1;

        coin_text.text = "Carried Coins: " + held_coins.ToString() + "\n" + "Banked Coins: " + saved_coins.ToString();
        item_text.text = "Held Item: " + held_obj;
        explore_text.text = "Rooms Found: " + rooms.ToString() + "\n" + "Shops Found: " + shops.ToString();
    }

    internal void InteractText(string message = "") {
        interact_text.text = message;
    }

    public void QuitGame() {
        Application.Quit();
    }

}
