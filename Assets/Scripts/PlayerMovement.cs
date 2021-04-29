using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController pawn;
    public Transform leg1;
    public Transform leg2;
    public float walkSpeed = 5;

    public float gravityMult = 30;
    public float jumpImpulse = 10;
    public float legRot = 0;

    private Camera cam;
    Vector3 inputDirection = new Vector3();

    private float verticalVelocity = 0;

    private float timeLeftGrounded = 0;

    public bool isDead = false;

    public bool isGrounded
    {
        get
        {
            return pawn.isGrounded || timeLeftGrounded > 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        pawn = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // countdown:
        if (timeLeftGrounded > 0) timeLeftGrounded -= Time.deltaTime;

        if (!isDead)
        {
            MovePlayer();
        }

        if (isDead) Die();
    }

    public void Die()
    {

    }

    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal"); // strafing?
        float v = Input.GetAxis("Vertical"); // forward / backward
        float z = Input.GetAxis("Jump"); // jump
        bool isJumpHeld = Input.GetButtonDown("Jump");

        bool isTryingToMove = (h != 0 || v != 0);

        if (isTryingToMove)
        {
            float camYaw = cam.transform.eulerAngles.y;
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(0, camYaw, 0), 0.02f);
        }

        inputDirection = transform.forward * v + transform.right * h;
        if (inputDirection.sqrMagnitude > 1) inputDirection.Normalize();

        // apply gravity
        verticalVelocity += gravityMult * Time.deltaTime;

        // sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            walkSpeed = 8;
        }
        else
        {
            walkSpeed = 5;
        }

        // adds lateral movement to vertical movement
        Vector3 moveDelta = inputDirection * walkSpeed + verticalVelocity * Vector3.down;

        // move pawn
        pawn.Move(moveDelta * Time.deltaTime);

        if (pawn.isGrounded)
        {
            verticalVelocity = 0;

            timeLeftGrounded = 0.2f;
        }

        if (isGrounded)
        {
            if (z != 0 && isJumpHeld)
            {
                verticalVelocity = -jumpImpulse;
                timeLeftGrounded = 0;
            }
        }
    }
}
