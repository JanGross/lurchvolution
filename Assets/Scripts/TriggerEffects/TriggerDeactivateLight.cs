using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeactivateLight : MonoBehaviour {

    public LightLurch theScript;
    public Light theLight;
    public bool activate;
    public bool deactivate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            if (deactivate)
            {
                theScript.enabled = false;
                theLight.enabled = false;
            }

            if (activate)
            {
                theScript.enabled = true;
                theLight.enabled = true;
            }
        }
    }
}
