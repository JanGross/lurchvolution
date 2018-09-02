using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurchMovement : MonoBehaviour {

    [Header("Mouse")]
    [Range(0.01f, 4.0f)]
    public float mouseSensitivity;
    public bool invertMouse;
    public bool lockMouse;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    [Header("Smooth Follow")]
    [Range(0.1f, 2)]
    public float speed;
    private Vector3 velocity = Vector3.zero;

    [Header("Lurch")]
    public Transform theLurch;
    private Rigidbody lurchBody;

    [Header("Jump Charge")]
    public float maxJumpForce;
    public float upForceModifier = 2.0f;
    private float currentJumpForce;
    private float currentJumpForwardForce;
    public float forceIncreaseSpeed;

    public Color normalColor;
    public Color fullyChargedColor;
    private Color lerpedColor;

    public Vector3 normalSize;
    [Range(0.1f, 1.0f)]
    public float chargedSizeModifier;
    private Vector3 chargedSize;
    private Vector3 currentSize;

    static float t = 0.0f;

    [Header("Jump n Stick")]
    public bool canStick;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private Transform stickPoint;
    private bool stickToGround = false;
    [SerializeField]
    private float stickyOffset = 1;


    [Header("Gliding")]
    public bool canGlide;
    private Vector3 glideSize;
    [Range(1.0f, 10.0f)]
    public float glideForce;

    private void Start()
    {
        lurchBody = theLurch.GetComponent<Rigidbody>();
        normalSize = theLurch.transform.localScale;
        chargedSize = theLurch.transform.localScale * chargedSizeModifier;
        glideSize = new Vector3(normalSize.x, normalSize.y * 0.5f, normalSize.z);
        stickPoint = new GameObject().transform;

    }

    void Update()
    {
        theLurch.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        theLurch.GetComponent<Renderer>().material.color = lerpedColor;
        theLurch.transform.localScale = currentSize;

        MouseControl();
        JumpControl();
        SmoothFollow();
        Gliding();
        StickToPoint();
    }

    public void DontStick()
    {
        stickToGround = false;
    }
    void MouseControl()
    {
        float mouseInputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseInputY;

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        if (!invertMouse)
        {
            mouseInputY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        }
        else
        {
            mouseInputY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        }

        yaw += mouseInputX;
        pitch -= mouseInputY;

        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void SmoothFollow()
    {
        Vector3 targetPosition = theLurch.TransformPoint(new Vector3(0, 0, 0));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, speed);
    }

    void JumpControl()
    {
        if (Input.GetButton("Fire1") && currentJumpForce < maxJumpForce)
        {
            currentJumpForce += forceIncreaseSpeed;
            lerpedColor = Color.Lerp(normalColor, fullyChargedColor, t);
            if (t < 1.0f)
            {
                t += 2 * Time.deltaTime;
            }

            currentSize = Vector3.Lerp(currentSize, chargedSize, Time.deltaTime * 1.5f);

        }
        else if(!Input.GetButton("Fire1"))
        {
            lerpedColor = normalColor;
            t = 0.0f;
            currentSize = normalSize;

        }

        if (cooldown > 0.0f)
        {
            cooldown -= Time.deltaTime;
            currentJumpForce = 0.0f;
        }

        if (Input.GetButtonUp("Fire1")) {
            cooldown = 0.5f;
            lurchBody.isKinematic = false;
            stickToGround = false;

            lurchBody.AddForce(theLurch.forward * currentJumpForce, ForceMode.Impulse);
            lurchBody.AddForce(theLurch.up * currentJumpForce * upForceModifier, ForceMode.Impulse);
        }
        else if (LurchOnGround() && Time.time > cooldown)
        {
            //stickToGround = true;
            lurchBody.isKinematic = true;
        }
        else if (!LurchOnGround() && Time.time > cooldown && !AmSticking())
        {
            currentJumpForce = 0.0f;
        }

        if (AmSticking())
        {
            lurchBody.isKinematic = true;
        }
        else
        {
            lurchBody.isKinematic = false;
        }

    }

    void Gliding()
    {

        if (canGlide && !lurchBody.isKinematic && Input.GetButton("Fire1") &&!LurchOnGround())
        {
            Debug.Log("Ich kann jetzt gleiten");
            theLurch.transform.localScale = glideSize;

            lurchBody.AddForce(theLurch.forward * glideForce);
            lurchBody.AddForce(theLurch.up * 0.08f, ForceMode.Impulse);

        }
        if (!Input.GetButton("Fire1"))
        {
            theLurch.transform.localScale = currentSize;
        }

    }

    public bool LurchOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(theLurch.position, Vector3.down, out hit, 0.34f) && cooldown <= 0.0f)
        {
            if (!stickToGround)
            {
                lurchBody.isKinematic = true;
                stickPoint.parent = hit.transform;
                stickPoint.position = hit.point;
                stickToGround = true;
            }
            return true;
        }

        return false;
    }

    bool AmSticking()
    {
        if (canStick) {
            RaycastHit hit;
            if (Physics.Raycast(theLurch.position, theLurch.forward, out hit, 1.4f))
            {
                return true;
            }
            if (lurchBody.isKinematic)
            {
                return true;
            }
        }

        return false;
    }

    public void StickToPoint()
    {
        if (stickToGround)
        {
            theLurch.position = stickPoint.position + new Vector3(0, stickyOffset, 0);
        }
    }
}
