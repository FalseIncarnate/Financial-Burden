using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeyScript : Item  {

    public string color = "";

    public Sprite red_sprite;
    public Sprite blue_sprite;
    public Sprite green_sprite;

    protected SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(color == "") {
            int choice = (int)Random.Range(1, 3);
            if(choice == 1) {
                color = "red";
            } else if(choice == 2) {
                color = "blue";
            } else {
                color = "green";
            }
        }
        if(color == "red") {
            spriteRenderer.sprite = red_sprite;
        }else if(color == "blue") {
            spriteRenderer.sprite = blue_sprite;
        } else {
            spriteRenderer.sprite = green_sprite;
        }
        name = color + " key";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
