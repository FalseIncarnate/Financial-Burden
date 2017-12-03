using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public GameObject grass;
    public GameObject dirt;
    public GameObject stone;
    public GameObject coin;

    protected int dirt_chance = 30;
    protected int stone_chance = 20;

    protected int coin_chance = 10;

	// Use this for initialization
	void Start () {
        build_background();
	}

    void build_background() {
        Transform title_back = new GameObject("title_back").transform;
        for(int x = -20; x <= 20; x++) {
            for(int y = -10; y <= 10; y++) {
                Vector3 curPos = new Vector3(x, y, 0f);
                GameObject tile = grass;
                int chance = (int)Random.Range(0, 100);
                if(chance <= dirt_chance) {
                    tile = dirt;
                } else if(chance >= 100 - stone_chance) {
                    tile = stone;
                }
                GameObject instance = Instantiate(tile, curPos, Quaternion.identity);
                instance.transform.SetParent(title_back);

                if(Random.Range(0, 100) <= coin_chance) {
                    GameObject instance2 = Instantiate(coin, curPos, Quaternion.identity);
                    instance2.transform.SetParent(title_back);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
