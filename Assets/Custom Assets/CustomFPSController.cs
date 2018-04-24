using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFPSController : MonoBehaviour {

    //Cursor Lock


    // Variables in Controller
    public float speed = 2.0f;
    public float strafeSpeed = 1.0f;
    public float backSpeed = 1.0f;
    public float sprintMult = 2.0f;
    public float sensitivity = 2.0f;
    public float slideCooldown = 5.0f;
    public int inverted = -1;

    public bool hasJumped = false;
    public bool isCrouched = false;
    public bool cursorLocked = false;
    public bool isSprinting = false;
    public bool isSliding = false;

    CharacterController player;
    public GameObject eyes;

    private float moveForward, moveBack, moveStrafe, rotX, rotY, vertVelocity, slideTime = 0;
    public float jumpForce = 4.0f;

	// Use this for initialization
	void Start () {

        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

	}
	
	// Update is called once per frame
	void Update () {

        SprintCheck();

        Move();

        Slide();

        Crouch();

        Jump();

        ApplyGravity();


        // Put into function
        if (Input.GetButton("Cancel"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (!isCrouched && !isSliding)
        {
            player.height = 2;
        }
    }

    private void Jump ()
    {

        // NEED TO IMPLEMENT GROUNDCHECK!

        if (Input.GetButtonDown("Jump"))
        {
            vertVelocity = jumpForce;
            hasJumped = true;
        }
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

    private void Move()
    {

        // Get Movement Data
        if (isSprinting)
        {
            moveForward = Input.GetAxis("Vertical") * speed * sprintMult;
        }
        else
        {
            moveForward = Input.GetAxis("Vertical") * speed;
        }

        moveBack = Input.GetAxis("Vertical") * speed * backSpeed * inverted;
        moveStrafe = Input.GetAxis("Horizontal") * speed * strafeSpeed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity * inverted;


        transform.Rotate(0, rotX, 0);
        eyes.transform.Rotate(rotY, 0, 0);

        if (!isSliding)
        {
            Vector3 movement = new Vector3(moveStrafe, vertVelocity, moveForward);
            movement = transform.rotation * movement;
            player.Move(movement * Time.deltaTime);
        }
    }

    private void SprintCheck()
    {
        if (Input.GetButton("Sprint") && Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") == 0)
        {
            isSprinting = true;
            isCrouched = false;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void Slide()
    {
        // Fix this once you learn how to check for animation state/find better implementation. Cuz this sucks and is lazy.
        
        if (isSprinting && !isSliding && Input.GetButtonDown("Crouch"))
        {
            isSliding = true;
            slideTime = Time.time + slideCooldown;
            player.height = player.height / 2;
        }
        if (slideTime + slideCooldown < Time.time)
        {
            isSliding = false;
        }
        if (isSliding)
        {
            Vector3 movement = new Vector3 (0,0,speed * 4);
            movement = transform.rotation * movement;
            player.Move(movement * Time.deltaTime);
        }
    }

    private void Crouch()
    {
        //Implement crouch
        if (Input.GetButtonDown("Crouch") && !isSliding)
        {
            isCrouched = true;
            player.height = player.height / 2;
        }

        if (Input.GetButtonUp("Crouch") && !isSliding)
        {
            isCrouched = false;
            player.height = player.height * 2;
        }
    }
}
