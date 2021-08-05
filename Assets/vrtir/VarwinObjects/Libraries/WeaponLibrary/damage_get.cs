using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varwin.Public;

public class damage_get : MonoBehaviour
{
    [HideInInspector] public damage_get_main_counter Counter;
    private float damageMultiplier = 1;
    private bool canBeAttacked = true;
    private float cooldownTime = 1;

    private float cooldownTimer = 0;

    // public Transform t;
    // public GameObject hitP;

    public void TakeDamage(float damage)
    {
        if(Counter != null)
        {
            Counter.TakeDamage(damage*damageMultiplier);
        }
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     if(other.tag == "dealer")
    //     {
    //         if (!canBeAttacked) return;
    //
    //         Destroy(Instantiate(hitP,t.position, new Quaternion(0,0,0,0)) , 3);
    //         cooldownTimer = cooldownTime;
    //         canBeAttacked = false;
    //     }
    // }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            canBeAttacked = true;
        }
    }
}
