using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinProjScript : ProjScript {


	// Use this for initialization
	public override void Start () {
        base.Start();
        proj_type = "coin";
        ignore_tags.Add("coin_pass");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
