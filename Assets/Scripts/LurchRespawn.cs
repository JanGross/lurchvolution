using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurchRespawn : MonoBehaviour {

    //Spawnpoints
    [SerializeField]
    private List<GameObject> respawnPoints = new List<GameObject>();
    private GameObject lastRespawnPoint;
    private int lastRespawnPointIndex;

    //Refs
    [SerializeField]
    private Transform lowerBounds;

    //ExtRefs
    [SerializeField]
    private GameObject lurch;
    private LurchMovement compLurchMovement;

    //States
    private bool offGround = false;
    private Vector3 lastGroundPos;

    //Settings
    [Header("Settings")]
    [SerializeField]
    [Tooltip("Update the spawnpoint every n frames")]
    private int UpdateFrequenzy = 15;
    [SerializeField]
    private float maxFallDist = 100;
    [SerializeField]
    private KeyCode respawnKey = KeyCode.R;

    void Start () {
        if (respawnPoints.Count == 0)
        {
            this.enabled = false;
            throw new System.Exception("No respawn points assigned");
        }
        compLurchMovement = gameObject.GetComponent<LurchMovement>();
        lastGroundPos = lurch.transform.position;

        lastRespawnPoint = respawnPoints[0];

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(respawnKey))
        {
            RespawnLurch("Player respawned");
        }

        if(!offGround)
        {
            if(!compLurchMovement.LurchOnGround())
            {
                offGround = true;
                lastGroundPos = lurch.transform.position;
            }
        } else
        {
            if (compLurchMovement.LurchOnGround())
            {
                offGround = false;
                float fallDist = Vector3.Distance(lurch.transform.position, lastGroundPos);
                if (fallDist > maxFallDist)
                {
                    //We fell too far
                    RespawnLurch("Lurch fell too far");
                }
                lastGroundPos = lurch.transform.position;
            }
        }

        if(Time.frameCount % UpdateFrequenzy == 0)
        {
            UpdateRespawnPoint();
        }

        if (Time.frameCount % UpdateFrequenzy * 2 == 0)
        {
            CheckBounds();
        }

    }

    private void CheckBounds()
    {
        if(transform.position.y < lowerBounds.position.y)
        {
            RespawnLurch("Lurch went out of bounds");
        }
    }

    private void UpdateRespawnPoint()
    {
        float nextPointDist;
        float lastPointDist = Vector3.Distance(transform.position, lastRespawnPoint.transform.position);
        for (int i = lastRespawnPointIndex; i < respawnPoints.Count; i++)
        {
            nextPointDist = Vector3.Distance(transform.position, respawnPoints[i].transform.position);
            if (nextPointDist < lastPointDist)
            {
                lastRespawnPoint = respawnPoints[i];
                lastRespawnPointIndex = i;
            }
        }
    }
    
    public void RespawnLurch(string message = "Lurch respawned")
    {
        Debug.LogWarning("Lurch respawned: " + message);
        lurch.transform.position = lastRespawnPoint.transform.position;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < respawnPoints.Count; i++)
        {
            if(respawnPoints[i] == lastRespawnPoint)
            {
                Gizmos.DrawIcon(respawnPoints[i].transform.position, "RespawnActive.png");
            } else if (i < lastRespawnPointIndex)
            {
                Gizmos.DrawIcon(respawnPoints[i].transform.position, "RespawnInactive.png");
            } else
                Gizmos.DrawIcon(respawnPoints[i].transform.position, "Respawn.png");
        }
    }
}
