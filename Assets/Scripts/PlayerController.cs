using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : Movable_Object {

    internal const int NORTH = 1;
    internal const int EAST = 2;
    internal const int SOUTH = 4;
    internal const int WEST = 8;

    internal string character = "Boy";

    private Inventory inv;

    protected GameManager gm;

	// Use this for initialization
	public override void Start () {
        base.Start();
        facing_dir = SOUTH;
        inv = transform.gameObject.GetComponent<Inventory>();
        gm = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        bool attempt_move = false;
        int new_facing_dir = facing_dir;

        if(Input.GetKey("escape")) {
            gm.ui.InteractText(" ");
        }

        if(tr.position == pos && !is_moving) {
            if(Input.GetKey("up")) {
                new_facing_dir = NORTH;
            } else if(Input.GetKey("down")) {
                new_facing_dir = SOUTH;
            } else if(Input.GetKey("right")) {
                new_facing_dir = EAST;
            } else if(Input.GetKey("left")) {
                new_facing_dir = WEST;
            } else if(Input.GetKey("w")) {
                new_facing_dir = NORTH;
                pos += Vector3.up;
                attempt_move = true;
            } else if(Input.GetKey("s")) {
                new_facing_dir = SOUTH;
                pos += Vector3.down;
                attempt_move = true;
            } else if(Input.GetKey("d")) {
                new_facing_dir = EAST;
                pos += Vector3.right;
                attempt_move = true;
            } else if(Input.GetKey("a")) {
                new_facing_dir = WEST;
                pos += Vector3.left;
                attempt_move = true;
            } else if(Input.GetKey("q")) {
                inv.AttemptPickup();
            } else if(Input.GetKey("e")) {
                inv.AttemptDrop();
            } else if(Input.GetKey("space")) {
                inv.Interact();
            }
        }

        if(new_facing_dir != facing_dir) {
            facing_dir = new_facing_dir;
            UpdateSprite();
        }

        if(attempt_move || is_moving) {
            AttemptMove<Wall>(pos);
        }
        
    }

    protected override void AttemptMove<T>(Vector2 endPos) {
        base.AttemptMove<T>(endPos);

        RaycastHit2D hit;

        if(CanMove(endPos, out hit)) {
            is_moving = true;
            tr.position = Vector3.MoveTowards(tr.position, endPos, Time.deltaTime * speed);
            if(tr.position == (Vector3)endPos) {
                is_moving = false;
            }
        } else {
            pos = tr.position;
            is_moving = false;
            T hitComponent = hit.transform.GetComponent<T>();
            OnCantMove(hitComponent);
        }
    }

    internal override void UpdateSprite() {
        switch(facing_dir) {
            case NORTH:
                spriteRenderer.sprite = NORTH_SPRITE;
                break;
            case SOUTH:
                spriteRenderer.sprite = SOUTH_SPRITE;
                break;
            case EAST:
                spriteRenderer.sprite = EAST_SPRITE;
                break;
            case WEST:
                spriteRenderer.sprite = WEST_SPRITE;
                break;
            default:
                spriteRenderer.sprite = SOUTH_SPRITE;
                break;
        }
    }


}
