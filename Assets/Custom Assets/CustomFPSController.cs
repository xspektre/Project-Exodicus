﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFPSController : MonoBehaviour {


    // Variables in Controller
    public float speed = 2.0f;
    public float strafeSpeed = 1.0f;
    public float backSpeed = 1.0f;
    public float sprintMult = 2.0f;
    public float sensitivity = 2.0f;
    public float slideCooldown = 1.0f;
    public int inverted = -1;

    public bool hasJumped = false;
    public bool isCrouched = false;
    public bool cursorLocked = false;
    public bool isSprinting = false;
    public bool isSliding = false;

    Animator animator;
    CharacterController player;
    public GameObject eyes;

    private float moveForward, moveBack, moveStrafe, rotX, rotY, vertVelocity, slideTime = 0;
    private Vector3 slideDirection;
    public float jumpForce = 4.0f;

	// Use this for initialization
	void Start () {

        animator = GetComponent<Animator>();
        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

	}
	
	// Update is called once per frame
	void Update () {

        SprintCheck();

        Slide();

        Crouch();

        ApplyGravity();

        Jump();

        Move();


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

        // NEED TO IMPLEMENT GROUNDCHECK AND REAL-PHYSICS BASED JUMPING. DO NOT SET VELOCITY TO FIXED VALUES! MAKE IT ITERATIVE!

        if (Input.GetButtonDown("Jump") && !hasJumped)
        {
            vertVelocity = jumpForce;
            hasJumped = true;
        }
    }

    private void ApplyGravity ()
    {
        vertVelocity = player.velocity.y;
        //if (player.isGrounded)
        //{
        //    if (!hasJumped)
        //    {
        //        vertVelocity = Physics.gravity.y;
        //    }
        //}
        //else
        //{

        vertVelocity += Physics.gravity.y * Time.deltaTime;
        vertVelocity = Mathf.Clamp(vertVelocity, -50f, jumpForce);

        if (player.isGrounded)
        {
            hasJumped = false;
        }

        if (!hasJumped)
        {
            vertVelocity = Mathf.Clamp(vertVelocity, -50f, 1f);
        }
        //}
    }

    private void CursorLock ()
    {
        if (Input.GetButtonDown("")) ;
    }

    private void Move()
    {

        // Generate Forward Speed
        if (isSprinting)
        {
            moveForward = Input.GetAxis("Vertical") * speed * sprintMult;
        }
        else
        {
            moveForward = Input.GetAxis("Vertical") * speed;
        }
        // Generate Backwards Speed
        moveBack = Input.GetAxis("Vertical") * speed * backSpeed;
        moveStrafe = Input.GetAxis("Horizontal") * speed * strafeSpeed;
        // Generate Rotational Speed
        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity * inverted;

        // Rotate View
        transform.Rotate(0, rotX, 0);
        eyes.transform.Rotate(rotY, 0, 0);



        // If not sliding, let player control character
        if (!isSliding)
        {
            Vector3 movement;

            if (Input.GetAxis("Vertical") >= 0)
            {
                movement = new Vector3(moveStrafe, vertVelocity, moveForward);
            }
            else
            {
                movement = new Vector3(moveStrafe, vertVelocity, moveBack);
            }

            movement = transform.rotation * movement;
            player.Move(movement * Time.deltaTime);

            // Update animation controller
            animator.SetFloat("Speed", Input.GetAxis("Vertical"));
            print(animator.GetFloat("Speed"));
        }
    }

    private void SprintCheck()
    {
        if (Input.GetButton("Sprint") && Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") == 0)
        {
            isSprinting = true;
            animator.SetBool("Sprint", true);
            isCrouched = false;
        }
        else
        {
            isSprinting = false;
            animator.SetBool("Sprint", false);
        }
    }

    private void Slide()
    {
        // Fix this once you learn how to check for animation state/find better implementation. Cuz this sucks and is lazy.
        
        if (isSprinting && !isSliding && Input.GetButtonDown("Crouch"))
        {
            player.height = player.height / 2;
            isSliding = true;
            animator.SetBool("Slide", true);
            slideDirection = eyes.transform.forward;
            slideTime = Time.time + slideCooldown;
        }
        if (slideTime + slideCooldown < Time.time)
        {
            isSliding = false;
            animator.SetBool("Slide", false);
        }
        if (isSliding)
        {
            Vector3 neutralizeHeight = new Vector3(1, 0, 1);
            Vector3 movement = slideDirection * speed * 4;
            movement = Vector3.Scale(movement, neutralizeHeight);
            movement.y = vertVelocity;
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
