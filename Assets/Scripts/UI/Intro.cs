using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

    [SerializeField]
    private List<ToastMessage> messages;
    [SerializeField]
    private UIManager uiManager;

	// Use this for initialization
	void Start () {
		foreach( ToastMessage msg in messages)
        {
            msg.SetUIManager(uiManager);
            uiManager.QueueMessage(msg);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
