using UnityEngine;
using Varwin;
using Varwin.Public;

namespace WeaponLibrary
{
    public class SafetyBehaviour : MonoBehaviour, IUseStartAware, IUseEndAware
    {
        private WeaponBehaviour _weaponBehaviour;

        public Transform SafetyTransform;
        public Transform OnSafetyTransform;
        public Transform OffSafetyTransform;
        
        public AudioClip SwitchAudioClip;

        private void Awake()
        {
            _weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
        }

        public void OnUseStart(UsingContext context)
        {
            _weaponBehaviour.OnSafety = !_weaponBehaviour.OnSafety;
            _weaponBehaviour.PlayAudioClip(SwitchAudioClip);
        }

        private void Update()
        {
            var destRotation = _weaponBehaviour.OnSafety
                ? OnSafetyTransform.localRotation
                : OffSafetyTransform.localRotation;

            SafetyTransform.localRotation =
                Quaternion.Lerp(SafetyTransform.localRotation, destRotation, Time.deltaTime * 10f);
        }

        public void OnUseEnd()
        {
            
        }
    }
}