using UnityEngine;

namespace WeaponLibrary
{
    public class BulletBehaviour : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => GetRigidBody();

        public GameObject bullet;

        public float LiveTime = 5f;

        public AudioClip CollisionAudioClip;

        private bool _isCollided = true;

        public void PlaySound(AudioClip clip)
        {
            var source = GetComponent<AudioSource>();
            if (!source || !clip)
            {
                return;
            }

            source.volume = Mathf.Clamp(Rigidbody.velocity.magnitude / 2f, 0f, 0.5f);
            
            source.PlayOneShot(clip);
        }

        public void SetObject()
        {
            bullet.SetActive(true);
        }
        
        private void Update()
        {
            LiveTime -= Time.deltaTime;
            if (LiveTime < 0f)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isCollided)
            {
                return;
            }
            
            PlaySound(CollisionAudioClip);
            _isCollided = true;
        }

        private Rigidbody GetRigidBody()
        {
            if (!_rigidbody)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            return _rigidbody;
        }
    }
}