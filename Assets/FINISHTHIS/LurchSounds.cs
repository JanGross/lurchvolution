using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurchSounds : MonoBehaviour {

    public AudioSource theSource;
    public AudioClip jump;
    public AudioClip land;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        theSource.clip = land;
        theSource.Play();
    }

    private void OnCollisionExit(Collision collision)
    {
        theSource.clip = jump;
        theSource.Play();
    }
}
