using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Varwin;
using Varwin.Public;

namespace WeaponLibrary
{
    public class SwitcherBehaviour : MonoBehaviour, IUseStartAware, IUseEndAware
    {
        public Transform[] StatesTransforms;
        public UnityEvent[] StatesEvents;
        public int CurrentIndex = 0;
        public Transform MainTransform;
        public float SwitchTimeCoef = 10f;

        private WeaponBehaviour _weaponBehaviour;
        public AudioClip SwitchAudioClip;

        private void Awake()
        {
            _weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
        }

        private void Update()
        {
            if (StatesTransforms == null || StatesTransforms.Length == 0)
            {
                return;
            }
            
            MainTransform.localPosition = Vector3.Lerp(MainTransform.localPosition,
                StatesTransforms[CurrentIndex].localPosition, Time.deltaTime * SwitchTimeCoef);

            MainTransform.localRotation = Quaternion.Lerp(MainTransform.localRotation,
                StatesTransforms[CurrentIndex].localRotation, Time.deltaTime * SwitchTimeCoef);
        }

        public void OnUseStart(UsingContext context)
        {
            CurrentIndex++;

            if (CurrentIndex >= StatesTransforms.Length)
            {
                CurrentIndex = 0;
            }

            StatesEvents[CurrentIndex]?.Invoke();
            
            _weaponBehaviour.PlayAudioClip(SwitchAudioClip);
        }

        public void OnUseEnd()
        {
            
        }
    }
}