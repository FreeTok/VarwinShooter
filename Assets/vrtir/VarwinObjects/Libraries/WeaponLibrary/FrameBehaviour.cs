using UnityEngine;
using Varwin;
using Varwin.Public;

namespace WeaponLibrary
{
    public class FrameBehaviour : MonoBehaviour//, IUseStartAware
    {
        private WeaponBehaviour _weaponBehaviour;

        private void Awake()
        {
            _weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
        }

        // public void OnUseStart(UsingContext context)
        // {
        //     _weaponBehaviour.Reload();
        // }
    }
}