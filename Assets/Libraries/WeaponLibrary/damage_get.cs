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
            switch (Counter.gameObject.GetComponent<Element>().enemyElement.ToString())                                                                    //система урона в зависимости от элемента
            {
                case "Earth":
                    switch (element)
                    {
                        case "Earth":
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
                        case "Water":
                            damageMultiplier *= 1.5f;
                            break;
                        case "Fire":
                            damageMultiplier *= 2f;
                            break;
                    }
                    break;
                case "Air":
                    switch (element)
                    {
                        case "Earth":
                            damageMultiplier *= 2f;
                            break;
                        case "Ice":
                            damageMultiplier *= 2f;
                            break;
                        case "Fire":
                            damageMultiplier *= 2f;
                            break;
                        case "Air":
                            damageMultiplier *= .5f;
                            break;
                    }
                    break;
                case "Fire":
                    switch (element)
                    {
                        case "Water":
                            damageMultiplier *= 2f;
                            break;
                        case "Ice":
                            damageMultiplier *= 1.5f;
                            break;
                        case "Fire":
                            damageMultiplier *= .5f;
                            break;
                        case "Air":
                            damageMultiplier *= .5f;
                            break;
                    }
                    break;
                case "Water":
                    switch (element)
                    {
                        case "Earth":
                            damageMultiplier *= 2f;
                            break;
                        case "Fire":
                            damageMultiplier *= 1.5f;
                            break;
                        case "Ice":
                            damageMultiplier *= 2f;
                            break;
                        case "Water":
                            damageMultiplier *= .5f;
                            break;
                    }
                    break;
                case "Ice":
                    switch (element)
                    {
                        case "Earth":
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
                        case "Fire":
                            damageMultiplier *= 2f;
                            break;
                    }
                    break;
                case "Light":
                    switch (element)
                    {
                        case "Earth":
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
                case "Darkness":
                    switch (element)
                    {
                        case "Earth":
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
