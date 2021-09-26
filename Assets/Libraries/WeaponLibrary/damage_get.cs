using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varwin.Public;

public class damage_get : MonoBehaviour
{
    [HideInInspector] public damage_get_main_counter Counter;
    public float damageMultiplier = 1;

    public void TakeDamage(float damage, string element)
    {
        if(Counter != null)
        {
            switch (Counter.mob.GetComponent<Element>().enemyElement.ToString())                                                                    //система урона в зависимости от элемента
            {
                case "Dendro":
                    switch (element)
                    {
                        case "Dendro":
                            damageMultiplier *= .5f;
                            break;
                        case "Light":
                            damageMultiplier *= 1.5f;
                            break;
                        case "Darkness":
                            damageMultiplier *= 2f;
                            break;
                        case "Ice":
                            damageMultiplier *= 2f;
                            break;
                    }
                    break;
                case "Ice":
                    switch (element)
                    {
                        case "Dendro":
                            damageMultiplier *= .5f;
                            break;
                        case "Light":
                            damageMultiplier *= 4f;
                            break;
                        case "Darkness":
                            damageMultiplier *= 1f;
                            break;
                        case "Ice":
                            damageMultiplier *= .5f;
                            break;
                    }
                    break;
                case "Darkness":
                    switch (element)
                    {
                        case "Dendro":
                            damageMultiplier *= .5f;
                            break;
                        case "Light":
                            damageMultiplier *= 5f;
                            break;
                        case "Darkness":
                            damageMultiplier *= 1f;
                            break;
                        case "Ice":
                            damageMultiplier *= 2f;
                            break;
                    }
                    break;
            }
            Counter.TakeDamage(damage, damageMultiplier);
            damageMultiplier = 1;
        }
    }
}
