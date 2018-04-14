using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFPSController : MonoBehaviour {

    // Variables in Controller
    public float speed = 2.0f;
    public float sprintMult = 2.0f;
    public float sensitivity = 2.0f;
    public int inverted = -1;

    public bool hasJumped = false;
    public bool isCrouched = false;
    public bool cursorLocked = false;

    CharacterController player;
    public GameObject eyes;

    private float moveForward, moveBack, moveStrafe, rotX, rotY, vertVelocity;
    public float jumpForce = 4.0f;

	// Use this for initialization
	void Start () {

        player = GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {


        // Get Movement Data
        moveForward = Input.GetAxis("Vertical") * speed;
        moveBack = Input.GetAxis("Vertical") * speed;
        moveStrafe = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity * inverted;

        Vector3 movement = new Vector3(moveStrafe, vertVelocity, moveForward);
        transform.Rotate(0,rotX,0);
        eyes.transform.Rotate(rotY, 0, 0);
        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            hasJumped = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            if (!isCrouched)
            {
                player.height = player.height / 2;
                isCrouched = true;
            }
            else
            {
                player.height = player.height * 2;
                isCrouched = false;
            }
        }

        ApplyGravity();
    }

    private void Jump ()
    {
        vertVelocity = jumpForce;
    }

    private void ApplyGravity ()
    {
        if (player.isGrounded)
        {
            if (!hasJumped)
            {
                vertVelocity = Physics.gravity.y;
            }
        }
        else
        {
            vertVelocity += Physics.gravity.y * Time.deltaTime;
            vertVelocity = Mathf.Clamp(vertVelocity, -50f, jumpForce);
            hasJumped = false;
        }
    }

    private void CursorLock ()
    {
        if (Input.GetButtonDown("")) ;
    }
}
