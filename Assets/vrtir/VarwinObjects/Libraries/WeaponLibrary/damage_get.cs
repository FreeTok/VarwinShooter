using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varwin.Public;

public class damage_get : MonoBehaviour
{
    [HideInInspector] public damage_get_main_counter Counter;
    public float damageMultiplier = 1;

    public void TakeDamage(float damage)
    {
        if(Counter != null)
        {
            Counter.TakeDamage(damage, damageMultiplier);
        }
    }
}
