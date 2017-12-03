using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactScript : MonoBehaviour {

    internal List<string> vulnerabilities;

	// Use this for initialization
	public virtual void Start () {
        vulnerabilities = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal virtual void CheckImpact(string proj_type) {
        bool effective = false;
        if(vulnerabilities.Count == 0) {
            effective = true;
        }
        foreach(string weakness in vulnerabilities) {
            if(weakness == proj_type) {
                effective = true;
                break;
            }
        }
        if(!effective) {
            return;
        }
        DoImpact();
    }

    internal virtual void DoImpact() {

    }
}
