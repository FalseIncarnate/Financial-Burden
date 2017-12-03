using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class LevelManager : MonoBehaviour {

    public int lvl_rows = 9;
    public int lvl_cols = 9;
    private int dirt_chance = 20;

    protected int coins_to_spawn = 5;

    public GameObject grass;
    public GameObject dirt;
    public GameObject coin;
    public GameObject stone;

    public GameObject room_door;
    public GameObject challenge_door;

    public GameObject shop_door;
    public GameObject shop_room;
    public GameObject shop_floor;

    public GameObject start_shop;

    public GameObject piggy_bank;
    public GameObject sale_counter;

    public GameObject[] challenge_rooms;

    public GameObject player;

    private Transform lvl_holder;
    internal int room_num = 0;
    internal int shop_num = 0;
    internal int challenges = 1;

    // Use this for initialization
    void Start() {
        challenge_rooms = Resources.LoadAll<GameObject>("ChallengeRooms");
        lvl_holder = new GameObject("Level").transform;
        build_level();
    }

    void build_level() {
        string room_name = "Room" + room_num.ToString();
        room_num++;
        Transform room = new GameObject(room_name).transform;

        for(int x = -1; x <= lvl_cols; x++) {
            for(int y = -1; y <= lvl_rows; y++) {
                GameObject to_place = grass;
                if(x == -1 || x == lvl_cols || y == -1 || y == lvl_rows) {
                    if(x == lvl_cols && y == (int)(lvl_rows / 2)) {
                        to_place = room_door;
                        GameObject door_floor = Instantiate(dirt, new Vector3(x, y, 0f), Quaternion.identity);
                        door_floor.transform.SetParent(room);
                    }else if(x == -1 && y == (int)(lvl_rows / 2)) {
                        to_place = shop_floor;
                    } else {
                        to_place = stone;
                    }
                } else if(Random.Range(0, 100) <= dirt_chance) {
                    to_place = dirt;
                }
                GameObject instance = Instantiate(to_place, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.SetParent(room);
            }
        }
        room.SetParent(lvl_holder);
        redistribute_wealth(room);  //first room always has coins

        build_start_shop();

        GameObject player_mob = Instantiate(player, new Vector3(0f, 0f, 0f), Quaternion.identity);
        Camera.main.transform.SetParent(player_mob.transform);
	}

    void redistribute_wealth(Transform room, float room_x = 0, float room_y = 0, bool vert_room = false) {
        int wealth = coins_to_spawn;
        if(vert_room) {
            wealth = (int)Random.Range(5, 25);
        }
        while(wealth > 0) {
            //these randomized coordinates get converted to int to ensure they always are on the center of a tile for ease of pickup
            float newX = (int) Random.Range(room_x, room_x + lvl_cols);
            float newY = (int) Random.Range(room_y, room_y + lvl_rows);
            GameObject new_coin = Instantiate(coin, new Vector3(newX, newY, 0f), Quaternion.identity);
            new_coin.transform.SetParent(room);
            wealth--;
        }
    }

    internal void build_start_shop() {
        string room_name = "Shop" + shop_num.ToString();
        shop_num++;
        Transform room = new GameObject(room_name).transform;
        GameObject instance = Instantiate(start_shop, new Vector3(-4f, 4f, 0f), Quaternion.identity);
        instance.transform.SetParent(room);
        room.SetParent(lvl_holder);
    }

    internal bool build_new_room(float newX, float newY, bool vert_room = false) {
        int x_offset = (int) newX;
        int y_offset = (int) newY;

        if(vert_room) {
            if(Random.Range(0, 100) < 35) {
                build_challenge_room(newX, newY);
                return true;
            }
        }

        string room_name = "Room" + room_num.ToString();
        room_num++;
        Transform room = new GameObject(room_name).transform;

        bool is_dead_end = false;
        bool is_branch_room = false;
        bool is_shop_room = false;

        Vector3 nextDoor = new Vector3(x_offset + lvl_cols, y_offset, 0f);
        if(vert_room) {
            nextDoor = new Vector3(x_offset, y_offset + lvl_rows, 0f);
            x_offset -= (int)(lvl_cols / 2);
        } else {
            y_offset -= (int)(lvl_rows / 2);
        }

        Vector3 branchDoor = new Vector3(-1f, -1f, 0f);
        is_branch_room = (Random.Range(0, 100) < 30 && !vert_room);
        if(is_branch_room) {
            branchDoor = new Vector3(x_offset + (int)(lvl_cols / 2), y_offset + lvl_rows, 0f);
        }

        Vector3 shopDoor = new Vector3(-1f, -1f, 0f);
        is_shop_room = (Random.Range(0, 100) < 20 && !vert_room);
        if(is_shop_room) {
            Debug.Log("Opening shop!");
            shopDoor = new Vector3(x_offset + (int)(lvl_cols / 2), y_offset - 1f, 0f);
        }

        if(vert_room) {
            if(Random.Range(0, 100) < 25) {
                is_dead_end = true;
            }
        }

        for(int x = (int) x_offset; x <= x_offset + lvl_cols; x++) {
            for(int y = (int) y_offset; y <= y_offset + lvl_rows; y++) {
                GameObject to_place = grass;
                if(x == x_offset + lvl_cols || y == y_offset + lvl_rows) {
                    if(x == nextDoor.x && y == nextDoor.y && !is_dead_end) {
                        to_place = room_door;
                        GameObject door_floor = Instantiate(dirt, new Vector3(x, y, 0f), Quaternion.identity);
                        door_floor.transform.SetParent(room);
                    } else if(is_branch_room && x == branchDoor.x && y == branchDoor.y && !is_dead_end) {
                        to_place = challenge_door;
                        GameObject door_floor = Instantiate(dirt, new Vector3(x, y, 0f), Quaternion.identity);
                        door_floor.transform.SetParent(room);
                    } else {
                        to_place = stone;
                    }
                } else if(Random.Range(0, 100) <= dirt_chance) {
                    to_place = dirt;
                }
                GameObject instance = Instantiate(to_place, new Vector3(x, y, 0f), Quaternion.identity);
                if(to_place == room_door) {
                    if(vert_room) {
                        Door doorScript = instance.GetComponent<Door>();
                        doorScript.vert_door = true;
                    }
                }
                instance.transform.SetParent(room);
            }
        }
        //fill in the fourth wall
        if(vert_room) {
            for(int y2 = y_offset; y2 <= y_offset + lvl_rows; y2++) {
                GameObject instance = Instantiate(stone, new Vector3(x_offset- 1, y2, 0f), Quaternion.identity);
                instance.transform.SetParent(room);
            }
        } else {
            for(int x2 = x_offset; x2 <= x_offset + lvl_cols; x2++) {
                if(is_shop_room && x2 == shopDoor.x) {
                    GameObject door_floor = Instantiate(shop_floor, new Vector3(x2, y_offset - 1, 0f), Quaternion.identity);
                    door_floor.transform.SetParent(room);
                    GameObject instance = Instantiate(shop_door, new Vector3(x2, y_offset - 1, 0f), Quaternion.identity);
                    instance.transform.SetParent(room);
                } else {
                    GameObject instance = Instantiate(stone, new Vector3(x2, y_offset - 1, 0f), Quaternion.identity);
                    instance.transform.SetParent(room);
                }
            }
        }

        float coin_chance = 33f;
        if(vert_room) {
            coin_chance = 66f;
        }
        if(Random.Range(0, 100) < coin_chance) {
            Debug.Log("Making it rain!");
            redistribute_wealth(room, x_offset, y_offset, vert_room);
        }

        room.SetParent(lvl_holder);
        return true;
    }

    internal bool build_shop_room(float newX, float newY) {
        int x_offset = (int)newX;
        int y_offset = (int)newY;

        string room_name = "Shop" + shop_num.ToString();
        shop_num++;
        Transform room = new GameObject(room_name).transform;
        GameObject instance = Instantiate(shop_room, new Vector3(x_offset, y_offset, 0f), Quaternion.identity);
        instance.transform.SetParent(room);
        room.SetParent(lvl_holder);
        return true;
    }

    internal bool build_challenge_room(float newX, float newY) {
        int x_offset = (int)newX;
        int y_offset = (int)newY + (int)(lvl_rows / 2) -1;

        string room_name = "Challenge" + challenges.ToString();
        challenges++;
        Transform room = new GameObject(room_name).transform;

        int index = (int)Random.Range(0, challenge_rooms.Length);
        GameObject instance = Instantiate(challenge_rooms[index], new Vector3(x_offset, y_offset, 0f), Quaternion.identity);
        instance.transform.SetParent(room);
        room.SetParent(lvl_holder);
        return true;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
