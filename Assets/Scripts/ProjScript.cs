using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjScript : MonoBehaviour {

    internal float proj_speed = 5.0f;
    internal float max_dist = 7f;
    internal Vector3 traj;
    internal BoxCollider2D origin_collider;

    protected Transform tr;
    public Vector3 pos;
    public Vector3 endPos;

    public bool is_moving = false;
    protected bool is_targeted = false;

    protected Transform target;

    public string proj_type = "";
    public List<string> ignore_tags;

    public void Awake() {
        Start();    //just in case
    }

    // Use this for initialization
    public virtual void Start () {
        traj = new Vector3(0f, 0f, 0f);
        tr = transform;
        pos = tr.position;
        ignore_tags.Add("ignore_proj");
	}

    // Update is called once per frame
    void Update() {

    }

    private void FixedUpdate() {
        DoMove();
	}

    void DoMove() {
        if(is_moving) {
            
            if(tr.position == endPos) {
                Impact();
                return;
            }
            if(is_targeted) {
                endPos = target.position;
            }
            tr.position = Vector3.MoveTowards(tr.position, endPos, Time.deltaTime * proj_speed);
        }
    }

    internal bool SetTraj(Vector3 newTraj) {
        if(is_moving) {
            return false;
        }
        traj = newTraj;
        return true;
    }

    internal void Aim() {
        RaycastHit2D[] all_hit;
        int hit_index = -1;
        pos = tr.position;
        bool hit_something = ScanTraj(pos, out all_hit, out hit_index);
        if(!hit_something || hit_index == -1) {
            //max range fire, nothing to hit
            DumbFire();
        } else {
            //line them up and knock them down
            target = all_hit[hit_index].transform;
            Fire(target);
        }
    }

    protected void DumbFire() {
        pos += (traj * max_dist);
        endPos = pos;
        is_moving = true;
    }

    protected void Fire(Transform target) {
        endPos = target.position;
        is_moving = true;
        is_targeted = true;
    }

    protected void Impact() {
        is_moving = false;
        if(is_targeted) {
            ImpactScript impact_script = target.gameObject.GetComponent<ImpactScript>();
            if(impact_script) {
                impact_script.CheckImpact(proj_type);
            }
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    protected bool ScanTraj(Vector3 origin, out RaycastHit2D[] all_hit, out int hit_index) {
        hit_index = -1;
        origin_collider.enabled = false;
        all_hit = Physics2D.RaycastAll(origin, traj, max_dist);
        origin_collider.enabled = true;
        if(all_hit.Length == 0) {
            return false;
        }
        bool hit_something = false;
        foreach(RaycastHit2D what in all_hit) {
            if(!what.collider.isTrigger) {
                if(ignore_tags.Contains(what.transform.tag.ToString())) {
                    continue;
                }
                hit_something = true;
                hit_index = System.Array.IndexOf(all_hit, what);
                break;
            }
        }
        return hit_something;
    }
}
