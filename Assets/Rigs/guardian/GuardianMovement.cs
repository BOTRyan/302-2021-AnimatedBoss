using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianMovement : MonoBehaviour
{

    public Animator anim; // Animator Component on Guardian

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float speed = Input.GetAxisRaw("Vertical");

        anim.SetFloat("current speed", speed);

        transform.position += transform.forward * speed * Time.deltaTime * 3;
    }
}
