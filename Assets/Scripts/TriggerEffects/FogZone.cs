using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogZone : MonoBehaviour {

    [SerializeField]
    private Color fogColor = Color.white;
    [SerializeField]
    [Tooltip("Default: 0.01")]
    private float fogDensity = 0.01f;
    [SerializeField]
    private float fadeSpeed = 1;
    private float t = 0;
    private bool enabled = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (t < 1 && enabled)
        {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, fogColor, t);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, fogDensity, t);
        }
        t += fadeSpeed * Time.deltaTime;
    }

    void LerpFogSettings()
    {
        enabled = true;
        t = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Changing fog");
        LerpFogSettings();
    }

    private void OnTriggerExit(Collider other)
    {
        enabled = false;
    }
}
