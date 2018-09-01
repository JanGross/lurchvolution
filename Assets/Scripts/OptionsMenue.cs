using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenue : MonoBehaviour {

    [Header("Basics")]
    public GameObject optionsMenue;
    public LurchMovement lurchScript;

    [Header("Slider")]
    public Scrollbar sliderSensitivity;
    public Scrollbar sliderAudio;

    [Header("Buttons")]
    public Button exitButton;
    public Button resumeButton;
    public Button playButton;
    public string levelName;

    // Use this for initialization
    void Start () {
        optionsMenue.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsMenue.SetActive(true);
            if (lurchScript) {
                lurchScript.enabled = false;
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Sliders();
        Buttons();
    }

    void Sliders()
    {
        lurchScript.mouseSensitivity = sliderSensitivity.value * 4;
        AudioListener.volume = sliderAudio.value;
    }

    void Buttons()
    {
        if (resumeButton) {
            resumeButton.onClick.AddListener(TaskResume);
        }

        if (playButton)
        {
            playButton.onClick.AddListener(TaskLoadScene);
        }

        exitButton.onClick.AddListener(TaskExit);
    }

    void TaskResume()
    {
        lurchScript.enabled =true;
        optionsMenue.SetActive(false);
    }

    void TaskExit()
    {
        Application.Quit();
    }

    void TaskLoadScene()
    {
        SceneManager.LoadScene(levelName);
    }
}
