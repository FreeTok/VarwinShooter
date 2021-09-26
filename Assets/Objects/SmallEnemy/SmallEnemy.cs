using UnityEngine;
using Varwin;
using Varwin.Public;

namespace Varwin.Types.SmallEnemy_211023e0a08b4fe69ae12e0d1c19edba
{
    [VarwinComponent(English: "Small Enemy")]
    public class SmallEnemy : VarwinObject
    {
        public enum EnEnemyClass
        {
            Dendro,
            Ice,
            Light,
            Darkness
        };
        
        public EnEnemyClass EnemyClass;

        public GameObject MeshHolder;
        private MeshRenderer[] Meshs;
        
        [VarwinInspector(English: "Enemy class", Russian: "Класс врага")]
        [Variable(English: "Enemy class")]
        public EnEnemyClass EnemyClassPanel
        {
            get => EnemyClass;
            set => EnemyClass = value;
        }
        
        public Material DendroMaterial, IceMaterial, LightMaterial, DarknessMaterial;
        
        void Start()
        {
            Meshs = MeshHolder.GetComponentsInChildren<MeshRenderer>();
        }

        [Action(English: "Check mesh material")]
        public void CheckMeshMaterial()
        {
            switch(EnemyClass)
            {
                case EnEnemyClass.Dendro:
                {
                    SwitchMeshMaterial(DendroMaterial);
                    print("Dendro");
                    break;
                }
                
                case EnEnemyClass.Ice:
                {
                    SwitchMeshMaterial(IceMaterial);
                    print("Ice");
                    break;
                }
                
                case EnEnemyClass.Light:
                {
                    SwitchMeshMaterial(LightMaterial);
                    print("Light");
                    break;
                }
                
                case EnEnemyClass.Darkness:
                {
                    SwitchMeshMaterial(DarknessMaterial);
                    print("Darkness");
                    break;
                }
            }
        }
        
        [Action(English: "Set random enemy class")]
        public void RandomEnemyClass()
        {
            EnemyClass = (EnEnemyClass)Random.Range(0, 3);
            print(EnemyClass);
            CheckMeshMaterial();
        }

        void SwitchMeshMaterial(Material meshMaterial)
        {
            GetComponent<Element>().SwitchElement(EnemyClass.ToString());
            
            foreach (MeshRenderer Mesh in Meshs)
            {
                Mesh.material = meshMaterial;
            }
        }
    }
}
