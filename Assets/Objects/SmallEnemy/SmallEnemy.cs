using UnityEngine;
using Varwin;
using Varwin.Public;

namespace Varwin.Types.SmallEnemy_211023e0a08b4fe69ae12e0d1c19edba
{
    [VarwinComponent(English: "Small Enemy")]
    public class SmallEnemy : VarwinObject
    {
        [VariableGroup("panel start variables")]
        [Variable(English: "GetStartPositionX", Russian: "Получить стартовую позицию X")]
        public float GetStartPositionX
        {
            get => this.gameObject.transform.position.x;
        }
        
        [VariableGroup("panel start variables")]
        [Variable(English: "GetStartPositionY", Russian: "Получить стартовую позицию Y")]
        public float GetStartPositionY
        {
            get => this.gameObject.transform.position.y;
        }
        
        [VariableGroup("panel start variables")]
        [Variable(English: "GetStartPositionZ", Russian: "Получить стартовую позицию Z")]
        public float GetStartPositionZ
        {
            get => this.gameObject.transform.position.z;
        }
        
        private float _startPositionX;

        [Variable(English: "Start position X", Russian: "Стартовая позиция X")]
        public float GetXPanel
        {
            get => _startPositionX;
            set => _startPositionX = value;
        }
        
        private float _startPositionY;

        [Variable(English: "Start position Y", Russian: "Стартовая позиция Y")]
        public float GetYPanel
        {
            get => _startPositionY;
            set => _startPositionY = value;
        }
        
        private float _startPositionZ;

        [Variable(English: "Start position Z", Russian: "Стартовая позиция Z")]
        public float GetZPanel
        {
            get => _startPositionZ;
            set => _startPositionZ = value;
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////

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
