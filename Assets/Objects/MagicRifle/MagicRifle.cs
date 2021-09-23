using Varwin;
using Varwin.Public;
using WeaponLibrary;
using System;
using System.Collections;
using UnityEngine;

namespace Varwin.Types.MagicRifle_bf6ae11eea9e4720b830fffc0560378a
{
    [VarwinComponent(English: "Magic Rifle")]
    public class MagicRifle : VarwinObject
    {
        private float _baseDamage = 10;
        [VarwinInspector(English: "Base damage per shot", Russian: "Дамаг за выстрел")]
        public float BaseDamage
        {
            get => _baseDamage;
            set => _baseDamage = value;
        }
        
        private float _maxVelocityInertia = 0.2f;

        [VarwinInspector(English: "Inertia in m", Russian: "Максимальное отклонение в метрах")]
        public float MaxVelocityInertia
        {
            get => _maxVelocityInertia;
            set => _maxVelocityInertia = value;
        }

        private float _maxAngleInertia = -10f;

        [VarwinInspector(English: "Inertia in angles", Russian: "Максимальное отклонение в градусах")]
        public float MaxAngleInertia
        {
            get => _maxAngleInertia;
            set => _maxAngleInertia = value;
        }

        private bool _simpleMechanics;

        private float _coefSlowInertiaByTwoHand = 0.1f;

        [VarwinInspector(English: "Coef of slowing inertia by two hand",
            Russian: "Коэффициент снижения инерции от стрельбы двумя руками")]
        public float CoefSlowInertiaByTwoHand
        {
            get => _coefSlowInertiaByTwoHand;
            set => _coefSlowInertiaByTwoHand = value;
        }

        public AnimationClip IdleClip;

        public delegate void ShootEvent(int countMarks, Wrapper targetWrapper);

        private Rigidbody _rigidbody;

        [Event(English: "On shoot to target", Russian: "При попадании")]
        public event ShootEvent OnShootToTarget;

        [Event(English: "On shoot", Russian: "При выстреле")]
        public event Action OnShoot;

        public Rigidbody Rigidbody => GetRigidBody();

        public Transform ShootPoint;
        public HoleBehaviour HolePrefab;

        public bool OnSafety = false;

        public float ShootNormalizedTime = 0.5f;

        public AudioClip ShootAudioClip;
        public ParticleSystem ShootParticleSystem;

        private AudioSource _audioSource;
        private float _forceInertia = 3000;

        public float ForceRadius = 0.3f;
        private float _shootDelay = 0.5f;
        private float LastShoot;

        [VarwinInspector(English: "Delay between shooting", Russian: "Задержка между выстрелами")]
        public float ShootDelay
        {
            get => _shootDelay;
            set => _shootDelay = value;
        }

        [VarwinInspector(English: "Force inertia", Russian: "Сила инерции от выстрела")]
        public float ForceInertia
        {
            get => _forceInertia;
            set => _forceInertia = value;
        }

        public BulletBehaviour BulletBehaviourPrefab;
        public Transform BulletPoint;

        private float _bulletForce = 20f;

        [VarwinInspector(English: "Force bullet fly", Russian: "Сила вылета патрона")]
        public float BulletForce
        {
            get => _bulletForce;
            set => _bulletForce = value;
        }

        private float _wallHoleLifeTime = 200f;

        [VarwinInspector(English: "Life time of wall hole", Russian: "Время жизни отверстия в стене")]
        public float WallHoleLifeTime
        {
            get => _wallHoleLifeTime;
            set => _wallHoleLifeTime = value;
        }

        public void Shoot()
        {
            if (Time.time - LastShoot >= _shootDelay)
            {
                LastShoot = Time.time;
                StartCoroutine(Shooting());
            }
        }

        private Element.EnElements bulletElement;

        private void AddBullet()
        {
            var bulletPointTransform = BulletPoint.transform;
            var bullet = Instantiate(BulletBehaviourPrefab, bulletPointTransform.position,
                BulletBehaviourPrefab.gameObject.transform.rotation);

            bullet.bulletElement = bulletElement;
            
            bullet.gameObject.SetActive(true);

            bullet.GetComponent<Rigidbody>().AddForce(BulletPoint.transform.forward * BulletForce);

            bullet.Rifle = this.gameObject;
            bullet.WallHoleLifeTime = _wallHoleLifeTime;
            bullet.BaseDamage = _baseDamage;
        }

        private IEnumerator Shooting()
        {
            PlayAudioClip(ShootAudioClip);
            ShootParticleSystem.Play();

            AddBullet();

            Rigidbody.AddExplosionForce(ForceInertia, ShootPoint.transform.position, ForceRadius);
            OnShoot?.Invoke();

            yield return new WaitForSeconds(ShootDelay);
        }

        public void PlayAudioClip(AudioClip clip)
        {
            if (_audioSource && clip)
            {
                _audioSource.PlayOneShot(clip);
            }
        }

        private Rigidbody GetRigidBody()
        {
            if (!_rigidbody)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            return _rigidbody;
        }

        public void NextElement()
        {
            if (bulletElement == Element.EnElements.Darkness)
            {
                bulletElement = Element.EnElements.Dendro;
            }
            else
            {
                bulletElement += 1;
            }
        }
    }
}
