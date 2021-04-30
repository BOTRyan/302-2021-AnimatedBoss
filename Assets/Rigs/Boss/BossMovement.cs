using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private NavMeshAgent agent;
    public GameObject player;
    public GameObject bloodPrefab;

    public int health = 100;

    public float attackTimer = 0;

    void Start()
    {
        state = States.Idle;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (health <= 0) state = States.Dead;

        if (state != States.Dead && player)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 6 && attackTimer <= 0)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.SetDestination(transform.position);
                transform.LookAt(player.transform.position);

                if (attackTimer <= 0)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) <= 6)
                    {
                        Instantiate(bloodPrefab, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Quaternion.Euler(-90, 0, 0), player.transform);
                        player.GetComponent<PlayerMovement>().health -= 25;
                    }
                    attackTimer = 5;
                    state = States.Attacking;
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }

                if (attackTimer < 3.5f) state = States.Idle;
            }
        }
    }
}
