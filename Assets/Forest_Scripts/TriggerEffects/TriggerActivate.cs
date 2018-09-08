using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivate : MonoBehaviour {

    public AudioSource theAudio;
    public AudioClip theClip;
    public GameObject theObject;
    public Rigidbody theRigidBody;
    private Vector3 startPos;

	// Use this for initialization
	void Start () {
        if (theObject)
        {
            theObject.SetActive(false);
        }

        if (theRigidBody)
        {
            theRigidBody.isKinematic = true;
            startPos = theRigidBody.transform.position;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player") {
            if (theAudio)
            {
                theAudio.clip = theClip;
                theAudio.Play();
            }

            if (theObject)
            {
                theObject.SetActive(true);
            }

            if (theRigidBody)
            {
                theRigidBody.isKinematic = false;
                theRigidBody.transform.position = startPos;

            }
        }
    }
}
