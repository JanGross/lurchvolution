using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminatePlayer : MonoBehaviour {

    [SerializeField]
    private string name = "Generic trap";
    [SerializeField]
    private float resapwnDelay = 0;
    [Tooltip("Optional particle effect. Spawned when player enters the trigger")]
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private LurchRespawn player;
    [SerializeField]
    private bool killQueued = false;
    [SerializeField]
    private float rdCounter = 0;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (rdCounter <= 0 && killQueued)
        {
            player.RespawnLurch(name);
            killQueued = false;
        } else
        {
            rdCounter -= Time.deltaTime;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            rdCounter = resapwnDelay;
            killQueued = true;
            if(effect != null)
            {
                GameObject.Instantiate(effect);
            }
        }
    }
}
