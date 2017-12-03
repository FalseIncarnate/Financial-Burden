using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{

    internal int coins = 0; //coins are money, but they also slow you down for each you carry!
    protected int last_coins = 0;
    public float coin_slow = -0.1f;
    internal GameObject heldObject;
    internal Item heldObjectScript;

    internal GameObject holder;
    internal Movable_Object holder_script;
    internal BoxCollider2D holder_collider;

    internal const int NORTH = 1;
    internal const int EAST = 2;
    internal const int SOUTH = 4;
    internal const int WEST = 8;

    internal GameManager gm;

    // Use this for initialization
    void Start() {
        holder = transform.gameObject;
        if(!holder) {
            Debug.Log("No holder gameObject!");
            return;
        }
        holder_script = holder.GetComponent<Movable_Object>();
        if(!holder_script) {
            Debug.Log("Holder does not have a Movable_Object component!");
        }
        holder_collider = holder.GetComponent<BoxCollider2D>();
        if(!holder_collider) {
            Debug.Log("Holder does not have a BoxCollider2D component!");
        }

        gm = FindObjectOfType<GameManager>();
        gm.player_inv = this;
    }

    // Update is called once per frame
    void Update() {
        if(heldObjectScript && heldObjectScript.held_update) {
            heldObjectScript.HeldUpdate();
        }
    }

    internal void UpdateCoinWeight() {
        int change = coins - last_coins;
        float speed_change = (float) change * coin_slow;
        holder_script.UpdateSpeed(speed_change);
        last_coins = coins;
    }

    internal void AdjustCoins(int amount = 0) {
        coins += amount;
        UpdateCoinWeight();
        gm.ui.UpdateUI();
    }

    internal void AttemptPickup() {
        Collider2D[] item_colliders = new Collider2D[1];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int results = Physics2D.OverlapCollider(holder_collider, filter, item_colliders);
        if(results == 0) {
            return; //found nothing
        }
        foreach(Collider2D other in item_colliders) {
            Item item_script = other.gameObject.GetComponent<Item>();
            if(!item_script) {
                continue;
            }
            if(heldObject && !item_script.is_consumable) {
                FailPickup();
                break;
            }
            item_script.OnPickup(holder, this);
        }
        gm.ui.UpdateUI();
    }

    internal void FailPickup() {
        gm.ui.InteractText("Your hands are full!");
    }

    internal void AttemptDrop() {
        if(heldObject) {
            heldObject.transform.position = holder.transform.position;
            heldObject.SetActive(true);
            heldObjectScript.OnDrop(holder);
        }
        heldObject = null;
        heldObjectScript = null;
        gm.ui.UpdateUI();
    }

    internal void Expend() {
        Destroy(heldObject);
        heldObject = null;
        heldObjectScript = null;
        gm.ui.UpdateUI();
    }

    internal void Interact() {
        if(heldObject) {
            heldObjectScript.OnInteract(this);
        } else {
            DefaultInteract();
        }
    }

    internal void DefaultInteract() {
        Vector3 target_dir = GetTargetDir(holder.transform.position);
        BoxCollider2D holder_collider = holder.GetComponent<BoxCollider2D>();
        holder_collider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(holder.transform.position, target_dir);
        holder_collider.enabled = true;
        if(hit.transform == null) {
            return;
        }
        Interactable_Object target = hit.collider.gameObject.GetComponent<Interactable_Object>();
        if(!target) {
            return;
        }
        target.AttemptInteract(this);
    }

    internal Vector3 GetTargetDir(Vector3 start) {
        Vector3 target_dir = start;
        switch(holder_script.facing_dir) {
            case NORTH:
                target_dir += Vector3.up;
                break;
            case SOUTH:
                target_dir += Vector3.down;
                break;
            case EAST:
                target_dir += Vector3.right;
                break;
            case WEST:
                target_dir += Vector3.left;
                break;
        }
        return target_dir;
    }
}
