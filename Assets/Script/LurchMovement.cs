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
    public Material lurchMaterial;

    public float maxJumpForce;
    private float currentJumpForce;
    public float forceIncreaseSpeed;

    public Color normalColor;
    public Color fullyChargedColor;

    [Header("Jump n Stick")]
    public bool canStick;
    private float cooldown;

    private void Start()
    {
        lurchBody = theLurch.GetComponent<Rigidbody>();
    }

    void Update()
    {
        theLurch.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        MouseControl();
        JumpControl();
        SmoothFollow();


        Debug.DrawRay(theLurch.position, Vector3.down * 0.45f, Color.blue);

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
        }

        if (cooldown > 0.0f)
            cooldown -= Time.deltaTime;

        if (Input.GetButtonUp("Fire1")) {
            cooldown += 0.5f;
            lurchBody.isKinematic = false;
            Debug.Log("Ich springe");
            lurchBody.AddForce(theLurch.forward * currentJumpForce, ForceMode.Impulse);
            lurchBody.AddForce(theLurch.up * currentJumpForce, ForceMode.Impulse);
        }
        else if (LurchOnGround() && Time.time > cooldown)
        {
            lurchBody.isKinematic = true;
        }

        if (!LurchOnGround())
        {
            currentJumpForce = 0.0f;
            lurchBody.isKinematic = false;
        }
    }

    public bool LurchOnGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(theLurch.position, Vector3.down, out hit, 0.4f))
        {
            return true;
        }

        if (canStick)
        {

            Debug.DrawRay(theLurch.position, Vector3.forward* 1.6f);

            if (Physics.Raycast(theLurch.position, Vector3.forward, out hit, 1.6f))
            {
                return true;
            }
        }

        return false;
    }
}
