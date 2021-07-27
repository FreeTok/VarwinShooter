using System;
using UnityEngine;
using Varwin;
using Varwin.Public;

namespace WeaponLibrary
{
    public class HandleBehaviour : MonoBehaviour, IUseStartAware, IUseEndAware
    {
        public bool IsAutomaticMode = false;

        private WeaponBehaviour _weaponBehaviour;
        private bool _isUse = false;
        
        public Transform TriggerTransform;
        public Transform TriggerClickedTransform;
        public Transform TriggerNonClickedTransform;

        public AudioClip TriggerClip;

        private void Awake()
        {
            _weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
        }

        public void OnUseStart(UsingContext context)
        {
            _isUse = true;
            if (!IsAutomaticMode)
            {
                _weaponBehaviour.Shoot();
            }
            
            _weaponBehaviour.PlayAudioClip(TriggerClip);
        }

        public void OnUseEnd()
        {
            _isUse = false;
        }

        private void LateUpdate()
        {
            if (IsAutomaticMode && _isUse)
            {
                _weaponBehaviour.Shoot();
            }

            var targetRotation =
                _isUse ? TriggerClickedTransform.localRotation : TriggerNonClickedTransform.localRotation;

            TriggerTransform.localRotation =
                Quaternion.Lerp(TriggerTransform.localRotation, targetRotation, Time.deltaTime * 10f);
        }

        public void SetMode(bool automaticMode)
        {
            IsAutomaticMode = automaticMode;
        }
    }
}