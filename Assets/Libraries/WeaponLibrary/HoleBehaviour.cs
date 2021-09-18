using System.Collections;
using UnityEngine;

namespace WeaponLibrary
{
    public class HoleBehaviour : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        public float MaxTime = 2f;
        private Material _material;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _material = new Material(_spriteRenderer.material);
            _spriteRenderer.material = _material;

            StartCoroutine(StartFade());
        }

        private IEnumerator StartFade()
        {
            var currentTime = 0f;

            while (currentTime < MaxTime)
            {
                var color = _material.color;
                color.a = Mathf.Lerp(1f, 0f, currentTime / MaxTime);
                _material.color = color;
                currentTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            Destroy(gameObject);
        }
    }
}