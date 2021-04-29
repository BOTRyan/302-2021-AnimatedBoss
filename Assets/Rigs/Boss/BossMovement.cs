using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public enum States
    {
        Idle,
        Walk,
        Attacking,
        Dead
    }

    public States state { get; private set; }

    public int health = 100;
    private bool dieOnce = true;

    //GameObject ragDoll;

    void Start()
    {
        state = States.Idle;
    }

    void Update()
    {
        if (health <= 0) state = States.Dead;

        if (state == States.Dead && dieOnce)
        {
            //Instantiate(ragDoll, transform.position, transform.rotation, null);
            Destroy(gameObject);
            dieOnce = false;
        }

        if (state != States.Dead)
        {

        }
    }
}
