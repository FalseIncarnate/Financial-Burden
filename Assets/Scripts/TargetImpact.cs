using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetImpact : ImpactScript {

    public GameObject lootDrop;

	// Use this for initialization
	public override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal override void DoImpact() {
        Instantiate(lootDrop, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
