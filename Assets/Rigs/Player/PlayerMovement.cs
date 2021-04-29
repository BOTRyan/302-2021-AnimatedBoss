using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum States
    {
        Idle,
        Walk,
        Jumping,
        Attacking,
        Dead
    }

    private CharacterController pawn;
    public Transform cam;
    public float moveSpeed = 5;
    public float gravityMult = 30;
    public float jumpImpulse = 30;
    private float verticalVelocity = 0;
    private float timeLeftGrounded = 0;
    public bool isGrounded
    {
        get
        {
            return pawn.isGrounded || timeLeftGrounded > 0;
        }
    }

    public float stepSpeed = 5;
    public Vector3 walkScale = Vector3.one;

    public bool isSprinting = false;

    public float v;
    public float h;

    public AnimationCurve ankleRotationCurve;

    public States state { get; private set; }
    public Vector3 moveDir;

    public GameObject ragDollPrefab;
    public GameObject ragDollRef;
    private bool dieOnce = true;
    private int health = 100;
    private float attackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = States.Idle;
        pawn = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) state = States.Dead;

        if (state == States.Dead && dieOnce)
        {
            ragDollRef = Instantiate(ragDollPrefab, transform.position, transform.rotation, null) as GameObject;
            Destroy(gameObject);
            dieOnce = false;
        }

        if (state != States.Dead)
        {
            if (timeLeftGrounded > 0) timeLeftGrounded -= Time.deltaTime;

            if (!pawn.isGrounded)
            {
                state = States.Jumping;
                moveSpeed = 2;
            }

            if (Input.GetKey(KeyCode.R)) health--;

            if (Input.GetMouseButton(0))
            {
                Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1f))
                {
                    if (hit.collider.gameObject.tag == "Enemy" && attackTimer <= 0)
                    {
                        print("HIT");
                        hit.collider.GetComponent<BossMovement>().health -= 10;
                        attackTimer = 1;
                    }
                }
            }

            if (attackTimer > 0) attackTimer -= Time.deltaTime;

            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward);

            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            float z = Input.GetAxis("Jump");
            bool isJumpHeld = Input.GetButtonDown("Jump");

            isSprinting = Input.GetKey(KeyCode.LeftShift) ? true : false;

            if (isSprinting)
            {
                moveSpeed = 10;
                stepSpeed = 15;
                walkScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                moveSpeed = 5;
                stepSpeed = 7;
                walkScale = new Vector3(0.35f, 0.25f, 0.35f);
            }

            moveDir = transform.forward * v + transform.right * h;
            if (moveDir.sqrMagnitude > 1) moveDir.Normalize();

            verticalVelocity += gravityMult * Time.deltaTime;
            Vector3 moveDelta = moveDir * moveSpeed + verticalVelocity * Vector3.down;

            pawn.Move(moveDelta * Time.deltaTime);

            if (pawn.isGrounded)
            {
                if (moveDir.sqrMagnitude > 0.1f)
                {
                    state = States.Walk;
                    transform.localRotation = Quaternion.Euler(transform.rotation.x, cam.rotation.eulerAngles.y, transform.rotation.z);
                }
                else
                {
                    state = States.Idle;
                }
            }

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
}
