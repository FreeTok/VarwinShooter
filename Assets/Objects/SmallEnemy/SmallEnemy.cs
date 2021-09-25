using UnityEngine;
using Varwin;
using Varwin.Public;

namespace Varwin.Types.SmallEnemy_211023e0a08b4fe69ae12e0d1c19edba
{
    [VarwinComponent(English: "Small Enemy")]
    public class SmallEnemy : VarwinObject
    {
        [VarwinInspector(English: "Enemy class", Russian: "Класс врага")]
        [Variable(English: "Enemy class")]
        public Element.EnElements EnemyClassPanel
        {
            get => GetComponent<EnemyElementSwitch>().EnemyClass;
        }

        [Action(English: "Switch enemy class")]
        public void SwitchEnemyClass()
        {
            GetComponent<EnemyElementSwitch>().SwitchEnemyClass();
        }

        [Action(English: "Set random enemy class")]
        public void RandomEnemyClass()
        {
            GetComponent<EnemyElementSwitch>().RandomEnemyClass();
        }
    }
}
