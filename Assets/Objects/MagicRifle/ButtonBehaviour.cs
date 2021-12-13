using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Varwin.Public;

namespace Varwin.Types.MagicRifle_bf6ae11eea9e4720b830fffc0560378a
{
    public class ButtonBehaviour : MonoBehaviour, IUseStartAware
    {
        public UnityEvent OnUse;
            
        public void OnUseStart(UsingContext context)
        {
            OnUse.Invoke();
        }
    }
}
