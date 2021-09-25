using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varwin;

public class EnemyElementSwitch : MonoBehaviour
{
    public Element.EnElements EnemyClass;

    public GameObject MeshHolder;
    private MeshRenderer[] Meshs;
    
    public Material DendroMaterial, IceMaterial, LightMaterial, DarknessMaterial;

    void Start()
    {
        Meshs = MeshHolder.GetComponentsInChildren<MeshRenderer>();
    }
    
    public void SwitchEnemyClass()
    {
        switch (EnemyClass)
        {
            case Element.EnElements.Dendro:
            {
                SwitchClass(DendroMaterial);
                print("Dendro");
                break;
            }

            case Element.EnElements.Ice:
            {
                SwitchClass(IceMaterial);
                print("Ice");
                break;
            }

            case Element.EnElements.Light:
            {
                SwitchClass(LightMaterial);
                print("Light");
                break;
            }

            case Element.EnElements.Darkness:
            {
                SwitchClass(DarknessMaterial);
                print("Darkness");
                break;
            }
        }
    }
    
    public void RandomEnemyClass()
    {
        EnemyClass = (Element.EnElements)Random.Range(0, 3);
        print(EnemyClass);
        SwitchEnemyClass();
    }

    void SwitchClass(Material meshMaterial)
    {
        GetComponent<Element>().SwitchElement(EnemyClass);

        foreach (MeshRenderer Mesh in Meshs)
        {
            Mesh.material = meshMaterial;
        }
    }
}