using UnityEngine;
using Varwin;
using Varwin.Public;

namespace Varwin.Types.BigEnemy_6cc896cffcff4e8d95a8367963486dd1
{
    [VarwinComponent(English: "Big Enemy")]
    public class BigEnemy : VarwinObject
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
    }
}
