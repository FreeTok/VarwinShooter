using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Varwin;

public class EnemyElementSwitch : MonoBehaviour
{
    private Element.EnElements EnemyClass;

    public GameObject MeshHolder;
    private MeshRenderer[] Meshs;

    [VarwinInspector(English: "Enemy class", Russian: "Класс врага")]
    [Variable(English: "Enemy class")]
    public Element.EnElements EnemyClassPanel
    {
        get => EnemyClass;
    }

    public Material DendroMaterial, IceMaterial, LightMaterial, DarknessMaterial;

    void Start()
    {
        Meshs = MeshHolder.GetComponentsInChildren<MeshRenderer>();
    }

    [Action(English: "Switch enemy class")]
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

    [Action(English: "Set random enemy class")]
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