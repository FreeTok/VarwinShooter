using UnityEngine;
using Varwin;
using Varwin.Public;

namespace Varwin.Types.SmallEnemy_211023e0a08b4fe69ae12e0d1c19edba
{
    [VarwinComponent(English: "Small Enemy")]
    public class SmallEnemy : VarwinObject
    {
        private Vector3 _startPosition;

        [Variable(English: "StartPosition", Russian: "Стартовая позиция")]
        public Vector3 StartPositionPanel
        {
            get => _startPosition;
            set => _startPosition = value;
        }
        
        private float _GetX;

        [Variable(English: "GetX", Russian: "Получить X")]
        public float GetXPanel
        {
            get => _GetX;
            set => _GetX = value;
        }
        
        private float _GetY;

        [Variable(English: "GetY", Russian: "Получить Y")]
        public float GetYPanel
        {
            get => _GetY;
            set => _GetY = value;
        }
        
        private float _GetZ;

        [Variable(English: "GetZ", Russian: "Получить Z")]
        public float GetZPanel
        {
            get => _GetZ;
            set => _GetZ = value;
        }
        
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
