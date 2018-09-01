using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLurch : MonoBehaviour {

    private enum states { standard, lit, unlit }
    [Header("Distances")]
    [SerializeField]
    private float standardRadius, standardIntensity;
    [SerializeField]
    private float tinyRadius, tinyIntensity;
    [SerializeField]
    private float maxRadius, maxIntensity;
    [Header("Emission")]
    [SerializeField]
    private float emissionMultiplier = 1;
    private float emissionValue;
    [SerializeField]
    private float maxEmission, minEmission, baseEmission = 1;

    private float intensitySnapshot;
    private Light lightSource;
    private Renderer lurchRenderer;

    [SerializeField]
    private KeyCode triggerButton;
    [SerializeField]
    private float litDuration;
    [SerializeField]
    private float unLitDuration;
    [SerializeField]
    private float animationSpeed;

    [Header("Debug")]
    [SerializeField]
    private states state;
    [SerializeField]
    private float t = 0;
    [SerializeField]
    private float ldCounter;
    private float llCounter;



	// Use this for initialization
	void Start () {
        state = states.standard;
        GameObject lurch = GetComponent<LurchMovement>().theLurch.gameObject;
        lightSource = lurch.GetComponent<Light>();
        lurchRenderer = lurch.GetComponent<Renderer>();
        lightSource.color = lurchRenderer.material.GetColor("_Color");
        intensitySnapshot = lightSource.intensity;
        lurchRenderer.material.SetColor("_EmissionColor", lightSource.color * (emissionMultiplier * emissionValue));
        emissionValue = baseEmission;
        lurchRenderer.material.EnableKeyword("_EMISSION");
    }
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime * animationSpeed;
        lurchRenderer.material.SetColor("_EmissionColor", lightSource.color * (emissionMultiplier * emissionValue));
        switch (state)
        {
            case states.standard:
                StandardGlow();
                break;
            case states.lit:
                LightUp();
                break;
            case states.unlit:
                GlowLow();
                break;
        }
        if (Input.GetKeyDown(triggerButton) && state == states.standard)
        {
            t = 0;
            ldCounter = litDuration;
            llCounter = animationSpeed;
            state = states.lit;
        }
        
	}

    private void StandardGlow()
    {
        lightSource.intensity = Mathf.Lerp(tinyIntensity, standardIntensity, t);
        lightSource.range = Mathf.Lerp(tinyRadius, standardRadius, t);
        emissionValue = Mathf.Lerp(minEmission, baseEmission, t);

    }

    private void LightUp()
    {
        lightSource.intensity = Mathf.Lerp(standardIntensity, maxIntensity, t);
        lightSource.range = Mathf.Lerp(standardRadius, maxRadius, t);
        emissionValue = Mathf.Lerp(baseEmission, maxEmission, t);

        if (ldCounter <= 0)
        {
            t = 0;
            state = states.unlit;
            ldCounter = unLitDuration;
            llCounter = animationSpeed;
            return;
        }
        ldCounter -= Time.deltaTime;

    }

    private void GlowLow()
    {
        lightSource.intensity = Mathf.Lerp(maxIntensity, tinyIntensity, t);
        lightSource.range = Mathf.Lerp(maxRadius, tinyRadius, t);
        emissionValue = Mathf.Lerp(maxEmission, minEmission, t);


        if (llCounter <= 0)
        {
            if (ldCounter <= 0)
            {
                t = 0;
                state = states.standard;
                llCounter = animationSpeed;
                return;
            }
            ldCounter -= Time.deltaTime;
        }
        llCounter -= Time.deltaTime;
    }
}
