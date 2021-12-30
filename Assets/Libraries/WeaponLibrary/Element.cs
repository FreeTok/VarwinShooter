using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public enum EnEnemyElement
    {
        Earth,
        Air,
        Fire,
        Water,
        Ice,
        Light,
        Darkness
    };
    
    public EnEnemyElement enemyElement;

    public void SwitchElement(string element)
    {
        enemyElement = (EnEnemyElement)System.Enum.Parse(typeof(EnEnemyElement), element);
    }
}
