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
            Wizard,
            Knight,
        };
        
        public EnEnemyClass EnemyClass;
        
        [VarwinInspector(English: "Enemy class", Russian: "Класс врага")]
        public EnEnemyClass EnemyClassPanel
        {
            get => EnemyClass;
            set => EnemyClass = value;
        }
        
        public GameObject WizardMesh, KnightMesh;
        
        void Start()
        {
            switch(EnemyClass)
            {
                case EnEnemyClass.Wizard:
                {
                    WizardMesh.SetActive(true);
                    print("Wizard");
                    break;
                }
        
                case EnEnemyClass.Knight:
                {
                    KnightMesh.SetActive(true);
                    print("Knight");
                    break;
                }
            }
        }
    }
}
