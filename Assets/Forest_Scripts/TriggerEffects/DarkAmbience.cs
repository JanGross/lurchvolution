using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkAmbience : MonoBehaviour {

    private float defaultEnvLightingIntensity, defaultEnfReflectionIntensity;
    [SerializeField]
    private float targetEnvLightingIntensity, targetEnfReflectionIntensity;
    [SerializeField]
    private float lerpSpeed;
    [SerializeField]
    private bool isDark = false;
    private float t = 0;

    // Use this for initialization
    void Start () {
        defaultEnvLightingIntensity = RenderSettings.ambientIntensity;
        defaultEnfReflectionIntensity = RenderSettings.reflectionIntensity;
    }
	
    public void SwitchDarkness(bool dark)
    {
        isDark = dark;
        t = 0;
    }

    private void OnValidate()
    {
        t = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Player")
        {
            SwitchDarkness(true);
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            SwitchDarkness(false);
        }
    }
    // Update is called once per frame
    void Update () {

		if(isDark)
        {
            RenderSettings.ambientIntensity = Mathf.Lerp(defaultEnvLightingIntensity, targetEnvLightingIntensity, t);
            RenderSettings.reflectionIntensity = Mathf.Lerp(defaultEnfReflectionIntensity, targetEnfReflectionIntensity, t);
        }
        else if (t < 1)
        {
            RenderSettings.ambientIntensity = Mathf.Lerp(targetEnvLightingIntensity, defaultEnvLightingIntensity, t);
            RenderSettings.reflectionIntensity = Mathf.Lerp(targetEnfReflectionIntensity, defaultEnfReflectionIntensity , t);
        }
        t += Time.deltaTime * lerpSpeed;
    }
}
