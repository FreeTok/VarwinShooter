using System;
using System.Collections.Generic;
using System.Diagnostics;
using WeaponLibrary;
using UnityEngine;
using Varwin;
using Varwin.Public;

namespace Varwin.Types.Enemy_fcb68f39c4314653b1448037ca5555b7
{
    [VarwinComponent(English: "Enemy")]
    public class SmallEnemy : VarwinObject
    {
        public enum EnEnemyClass
        {
            Wizard,
            Knight,
        };
        
        public EnEnemyClass EnemyClass;
        
        public GameObject WizardMesh, KnightMesh;

        void Start()
        {
            WizardMesh.SetActive(false);
            KnightMesh.SetActive(false);
            
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
