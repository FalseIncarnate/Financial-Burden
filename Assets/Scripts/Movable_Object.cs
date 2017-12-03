using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Movable_Object : MonoBehaviour {

    protected BoxCollider2D boxcollider;

    protected SpriteRenderer spriteRenderer;

    protected Transform tr;
    public Vector3 pos;

    public float truespeed = 2.5f;
    public float speed = 2.5f;
    public float maxspeed = 25f;

    public bool is_moving = false;

    public int facing_dir;

    //these will hold our "directional" sprites. Set these in the editor or they will get set to whatever is set as the current sprite
    public Sprite NORTH_SPRITE;
    public Sprite SOUTH_SPRITE;
    public Sprite EAST_SPRITE;
    public Sprite WEST_SPRITE;


	// Use this for initialization
	public virtual void Start () {
        boxcollider = GetComponent<BoxCollider2D>();

        spriteRenderer = transform.gameObject.GetComponent<SpriteRenderer>();

        tr = transform;
        pos = transform.position;

        //ensure all sprite directions have a sprite assigned. If they don't assign them whatever we are using currently
        if(!NORTH_SPRITE) {
            NORTH_SPRITE = spriteRenderer.sprite;
        }
        if(!SOUTH_SPRITE) {
            SOUTH_SPRITE = spriteRenderer.sprite;
        }
        if(!EAST_SPRITE) {
            EAST_SPRITE = spriteRenderer.sprite;
        }
        if(!WEST_SPRITE) {
            WEST_SPRITE = spriteRenderer.sprite;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual bool CanMove(Vector2 endPos, out RaycastHit2D hit) {
        Vector2 startPos = tr.position;
        boxcollider.enabled = false;
        hit = Physics2D.Linecast(startPos, endPos);
        boxcollider.enabled = true;
        if(hit.transform == null) {
            return true;
        }
        return hit.collider.isTrigger;
    }

    protected virtual void AttemptMove<T>(Vector2 endPos) where T : Component {
        RaycastHit2D hit;
        bool can_move = CanMove(endPos, out hit);

        if(hit.transform == null) {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if(!can_move && hitComponent != null) {
            pos = tr.position;
            is_moving = false;
            OnCantMove(hitComponent);
        }
    }

    protected virtual void OnCantMove<T>(T component) where T : Component {

    }

    internal virtual void UpdateSpeed(float amount = 0) {
        truespeed += amount;
        if(truespeed < 0.1f) {
            speed = 0.1f;
        } else if(truespeed > maxspeed) {
            speed = maxspeed;
        } else {
            speed = truespeed;
        }
    }

    internal virtual void UpdateSprite() {

    }

}
