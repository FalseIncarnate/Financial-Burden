using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawn : MonoBehaviour {

    public GameObject[] possible_loot;
    public GameObject to_spawn;

	// Use this for initialization
	void Start () {
        possible_loot = Resources.LoadAll<GameObject>("ShopItems");
        int index = (int)Random.Range(0, possible_loot.Length);
        to_spawn = possible_loot[index];

        Instantiate(to_spawn, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
