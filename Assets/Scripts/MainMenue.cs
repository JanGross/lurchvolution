using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenue : MonoBehaviour {

    [Header("Slider")]
    public Scrollbar sliderAudio;

    [Header("Buttons")]
    public Button exitButton;
    public Button playButton;
    public string levelName;

    [Header("Loading")]
    public GameObject loadingStuff;
    public RectTransform loadingIcon;
    public GameObject finishedLoading;

    // Use this for initialization
    void Start () {
        loadingStuff.SetActive(false);
        finishedLoading.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Sliders();
        Buttons();
    }

    void Sliders()
    {
        AudioListener.volume = sliderAudio.value;
    }

    void Buttons()
    {

        if (playButton)
        {
            playButton.onClick.AddListener(TaskLoadScene);
        }

        exitButton.onClick.AddListener(TaskExit);
    }

    void TaskExit()
    {
        Application.Quit();
    }

    void TaskLoadScene()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            loadingStuff.SetActive(true);
            float rotPlus = 0.0f;
            rotPlus -= 0.03f;
            loadingIcon.Rotate(new Vector3(0, 0, rotPlus));

            if (asyncOperation.progress >= 0.9f)
            {
                finishedLoading.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
