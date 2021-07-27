using UnityEngine;
using Varwin.Public;
using WeaponLibrary;

namespace Varwin.Types.Target_20ebd3f95aff4938ad0cd90d4dd7c6b2
{
    public class Target : VarwinObject
    {
        
        public float HitForce
        {
            set
            {
                TargetBehaviour[] targets = GetComponentsInChildren<TargetBehaviour>();

                foreach (TargetBehaviour target in targets)
                {
                    target.HitForce = value;
                }
            }

            get
            {
                TargetBehaviour target = GetComponentInChildren<TargetBehaviour>();

                if (target)
                {
                    return target.HitForce;
                }

                return 0.0f;
            }
        }

    }
}
