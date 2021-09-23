using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public enum EnElements
    {
        Dendro,
        Ice,
        Light,
        Darkness
    };
    
    public EnElements v_Element;

    public void SwitchElement(EnElements element)
    {
        v_Element = element;
        print(v_Element);
    }
}
