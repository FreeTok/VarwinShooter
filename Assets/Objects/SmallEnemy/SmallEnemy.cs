using UnityEngine;
using Varwin;
using Varwin.Public;

namespace Varwin.Types.SmallEnemy_211023e0a08b4fe69ae12e0d1c19edba
{
    [VarwinComponent(English: "Small Enemy")]
    public class SmallEnemy : VarwinObject
    {
        public Element.EnEnemyElement EnemyClass;
        
        public MeshRenderer[] Meshs;

        public Material[] MeshMaterials;

        [Action(English: "Check mesh material")]
        public void CheckMeshMaterial()
        {
            switch(EnemyClass)
            {
                case Element.EnEnemyElement.Earth:
                {
                    SwitchMeshMaterial(MeshMaterials[0]);
                    break;
                }
                
                case Element.EnEnemyElement.Air:
                {
                    SwitchMeshMaterial(MeshMaterials[1]);
                    break;
                }
                
                case Element.EnEnemyElement.Fire:
                {
                    SwitchMeshMaterial(MeshMaterials[2]);
                    break;
                }
                
                case Element.EnEnemyElement.Water:
                {
                    SwitchMeshMaterial(MeshMaterials[3]);
                    break;
                }
                
                case Element.EnEnemyElement.Ice:
                {
                    SwitchMeshMaterial(MeshMaterials[4]);
                    break;
                }
                
                case Element.EnEnemyElement.Light:
                {
                    SwitchMeshMaterial(MeshMaterials[5]);
                    break;
                }
                
                case Element.EnEnemyElement.Darkness:
                {
                    SwitchMeshMaterial(MeshMaterials[6]);
                    break;
                }
                
            }
        }
        
        [Action(English: "Set random enemy class")]
        public void RandomEnemyClass()
        {
            EnemyClass = (Element.EnEnemyElement)Random.Range(0, 6);
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
