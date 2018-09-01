using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToastMessage
{
    [SerializeField]
    private string messageText;
    [SerializeField]
    private float duration;
    private float timeLeft;
    [SerializeField]
    private Texture2D icon;
    [SerializeField]
    private GameObject toastPrefab;
    private GameObject toastElements;
    private UIManager mngr;

    public void SetUIManager(UIManager _mngr)
    {
        mngr = _mngr;
    }

    public float TimeTick()
    {
        return timeLeft = timeLeft - Time.deltaTime;
    }

    public GameObject CreateToastElements()
    {
        GameObject toastContainer = GameObject.Instantiate(toastPrefab,toastPrefab.transform.position, Quaternion.identity);
        toastContainer.transform.SetParent(mngr.worldCanvas.transform);
        Toast toast = toastContainer.GetComponent<Toast>();
        toast.textElement.text = this.messageText;
        Debug.Log(mngr);
        icon = mngr.defaultIcon;
        toast.iconElement.sprite = Sprite.Create(icon, new Rect(0.0f, 0.0f, icon.width, icon.height), Vector2.one);
        toastContainer.SetActive(false);
        toastElements = toastContainer;
        return toastContainer;
    }

    public void ShowToastElements()
    {
        if (toastElements != null)
        {
            toastElements.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Trying to show ToastElements before calling CreateToastElements() does not work.");
        }
    }

    public void DestroyToastElements()
    {
        if(toastElements != null)
        {
            GameObject.Destroy(toastElements);
        } else
        {
            Debug.LogWarning("Trying to destroy ToastElements before calling CreateToastElements() does not work.");
        }
    }
}

public class UIManager : MonoBehaviour {

    public  GameObject worldCanvas;
    public  Texture2D defaultIcon;


    private List<ToastMessage> messageQueue = new List<ToastMessage>();
    private ToastMessage currentMessage;

	// Use this for initialization
	void Start () {

	}
	
    private void DisplayMessage(ToastMessage msg)
    {
        msg.CreateToastElements();
        msg.ShowToastElements();
    }

    private ToastMessage PopNextMessage()
    {
        if (messageQueue[0] != null)
        {
            ToastMessage next = messageQueue[0];
            messageQueue.RemoveAt(0);
            return next;
        }
        return null;
    }

	// Update is called once per frame
	void Update () {
        if (messageQueue.Count == 0 && currentMessage == null)
        {
            return; //No messages 
        }


        if (currentMessage == null)
        {
            DisplayMessage(PopNextMessage());
        }
        else
        {
            float messageTimeLeft = currentMessage.TimeTick();
            if (messageTimeLeft <= 0)
            {
                currentMessage.DestroyToastElements();
                currentMessage = PopNextMessage();
                DisplayMessage(currentMessage);
            }
        }
	}

    private void SetCurrentMessage(ToastMessage messageObject)
    {
        this.currentMessage = messageObject;
    }

    public ToastMessage createMessageObject(string text, float duration, Texture2D icon)
    {
        return new ToastMessage();
    }

    public int QueueMessage(ToastMessage msg)
    {
        messageQueue.Add(msg);
        return messageQueue.Count;
    }
 
}
