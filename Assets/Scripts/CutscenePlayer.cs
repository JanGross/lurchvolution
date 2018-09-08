using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutscenePlayer : MonoBehaviour {

    public Sprite[] theSprites;
    public Image theBase;
    public float speed = 1.0f;
    private float TUNS = 0.0f;
    private int i = 0;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (i < theSprites.Length + 1) { 
            if (Time.time > TUNS)
            {
                i += 1;

                if(i < theSprites.Length)
                theBase.sprite = theSprites[i];

                TUNS = Time.time + speed;
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
	}
}
