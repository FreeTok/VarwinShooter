using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varwin.Public;

public class damage_get : MonoBehaviour
{
    [HideInInspector] public damage_get_main_counter Counter;
    public float damageMultiplier = 1;

    public void TakeDamage(float damage, Element.EnElements element)
    {
        if(Counter != null)
        {
            switch (Counter.mob.GetComponent<Element>().v_Element)                                                                    //система урона в зависимости от элемента
            {
                case Element.EnElements.Dendro:
                    switch (element)
                    {
                        case Element.EnElements.Dendro:
                            damageMultiplier *= .5f;
                            break;
                        case Element.EnElements.Light:
                            damageMultiplier *= 1.5f;
                            break;
                        case Element.EnElements.Darkness:
                            damageMultiplier *= 2f;
                            break;
                        case Element.EnElements.Ice:
                            damageMultiplier *= 2f;
                            break;
                    }
                    break;
                case Element.EnElements.Ice:
                    switch (element)
                    {
                        case Element.EnElements.Dendro:
                            damageMultiplier *= .5f;
                            break;
                        case Element.EnElements.Light:
                            damageMultiplier *= 4f;
                            break;
                        case Element.EnElements.Darkness:
                            damageMultiplier *= 1f;
                            break;
                        case Element.EnElements.Ice:
                            damageMultiplier *= .5f;
                            break;
                    }
                    break;
                case Element.EnElements.Darkness:
                    switch (element)
                    {
                        case Element.EnElements.Dendro:
                            damageMultiplier *= .5f;
                            break;
                        case Element.EnElements.Light:
                            damageMultiplier *= 5f;
                            break;
                        case Element.EnElements.Darkness:
                            damageMultiplier *= 1f;
                            break;
                        case Element.EnElements.Ice:
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
