using System.Collections;
using UnityEngine;

namespace WeaponLibrary
{
    public class LightExplositionAnimator : MonoBehaviour
    {
        private Light _light;

        public AnimationCurve IntensityCurve;

        public float MaxTime = 0.5f;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _light.intensity = 0f;
        }

        public void Play()
        {
            StartCoroutine(StartShoot());
        }
        
        private IEnumerator StartShoot()
        {
            var currentTime = 0f;

            while (currentTime < MaxTime)
            {
                _light.intensity = IntensityCurve.Evaluate(currentTime / MaxTime);
                currentTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _light.intensity = IntensityCurve.Evaluate(1f);
        }
    }
}