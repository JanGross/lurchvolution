using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateMate : MonoBehaviour {

    public bool mateStick;
    public bool mateGlide;
    public bool mateGlow;

    public GameObject cutscene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponentInChildren<LurchMovement>())
        {
            if(mateStick)
            other.transform.root.GetComponentInChildren<LurchMovement>().canStick = true;

            if(mateGlide)
            other.transform.root.GetComponentInChildren<LurchMovement>().canGlide = true;

            if(mateGlow)
            other.transform.root.GetComponentInChildren<LightLurch>().enabled = true;

            cutscene.SetActive(true);
        }
    }
}
