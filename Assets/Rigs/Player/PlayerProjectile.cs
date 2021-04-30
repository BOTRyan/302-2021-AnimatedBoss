using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            hit.gameObject.GetComponent<BossMovement>().health -= 10;
            Destroy(gameObject);
        }
        else if (hit.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
