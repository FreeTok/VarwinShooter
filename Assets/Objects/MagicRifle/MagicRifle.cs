using Varwin;
using Varwin.Public;
using WeaponLibrary;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

        private float _chargeDamage;

        private float _maxChargeDamage = 20f;

        [VarwinInspector(English: "Max charged damage per shot", Russian: "Максимальный дамаг за заряженный выстрел")]
        public float MaxChargeDamage
        {
            get => _maxChargeDamage;
            set => _maxChargeDamage = value;
        }

        private float _сhargeDamageMultiplayer = 1f;

        [VarwinInspector(English: "Damage that divides to charged damage",
            Russian: "Дамаг, который добавляется к заряженному выстрелу")]
        public float ChargeDamageAdder
        {
            get => _сhargeDamageMultiplayer;
            set => _сhargeDamageMultiplayer = value;
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

        public delegate void ShootEvent(Wrapper targetWrapper);

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
        
        private float _artilleryShootDelay = 5f;

        [VarwinInspector(English: "Delay between artillery shoots", Russian: "Задержка между артилирийными выстрелами")]
        public float AtrilleryShootDelay
        {
            get => _artilleryShootDelay;
            set => _artilleryShootDelay = value;
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
        
        private float _bulletMass = 0.01f;

        [VarwinInspector(English: "Bullet mass", Russian: "Масса пули")]
        public float BulletMass
        {
            get => _bulletMass;
            set => _bulletMass = value;
        }


        public BulletBehaviour.EnBulletMode fireMode;

        public GameObject predictLine;

        public bool isCharging;

        public Image elementPad;
        public Sprite[] elementSprites;
        
        public Image modesPad;
        public Sprite[] modesSprites;

        private void Awake()
        {
            _chargeDamage = _baseDamage;
            LastShoot = Time.time;
            SwitchPadElement();
            SwitchPadMode();
            //predictLine.SetActive(false);
        }

        public void Shoot()
        {
            if (Time.time - LastShoot >= _shootDelay)
            {
                if (fireMode == BulletBehaviour.EnBulletMode.DefaultShot)
                {
                    StartCoroutine(Shooting(_baseDamage));
                }

                if (fireMode == BulletBehaviour.EnBulletMode.ChargedShot)
                {
                    StartCharging();
                    //DoubleShot();
                }

                if (fireMode == BulletBehaviour.EnBulletMode.ArtilleryShot && Time.time - LastShoot >= _artilleryShootDelay)
                {
                    ArtilleryShot();
                }
            }
        }

        private void StartCharging()
        {
            isCharging = true;
            StartCoroutine(IncreaseDamage());
        }

        public void EndCharging()
        {
            if (isCharging)
            {
                StopAllCoroutines();
                StartCoroutine(Shooting(_chargeDamage));
                _chargeDamage = _baseDamage;
                isCharging = false;
            }
        }

        // private void DoubleShot()
        // {
        //     if (_chargeDamage <= _baseDamage) //Если зарядка атаки ещё не идёт, то он начинает зарядку
        //         StartCoroutine(IncreaseDamage());
        //     else //в обратном случае он стреляет и возвращает урон на изначальный уровень
        //     {
        //         StopAllCoroutines();
        //         StartCoroutine(Shooting(_chargeDamage)); 
        //         _chargeDamage = _baseDamage; 
        //     }
        // }

        IEnumerator IncreaseDamage()
        {
            _chargeDamage = _baseDamage;

            while (_chargeDamage < _maxChargeDamage)
            {
                _chargeDamage = (float) Math.Round(_chargeDamage + _сhargeDamageMultiplayer, 1);
                yield return new WaitForSeconds(.2f);
            }
            
            // for (_chargeDamage = _baseDamage; _chargeDamage < _maxChargeDamage; _chargeDamage += _сhargeDamageMultiplayer * Time.deltaTime) // За 2 секунды 2х урон (пока что)
            // {
            //     _chargeDamage = (float) Math.Round(_chargeDamage, 1);
            //     yield return new WaitForSeconds(.2f);
            // }
        }

        private void ArtilleryShot()
        {
            BulletBehaviourPrefab.Explode = true;
            StartCoroutine(Shooting(_baseDamage));
            BulletBehaviourPrefab.Explode = false;
        }

        private BulletBehaviour.EnBulletElement bulletElement;

        private void AddBullet(float damage)
        {
            var bulletPointTransform = BulletPoint.transform;
            var bullet = Instantiate(BulletBehaviourPrefab, bulletPointTransform.position,
                BulletBehaviourPrefab.gameObject.transform.rotation);

            bullet.bulletMode = fireMode;
            bullet.bulletElement = bulletElement;
            bullet.OnHitTargetEvent += OnBulletHit;

            bullet.gameObject.SetActive(true);

            bullet.GetComponent<Rigidbody>().mass = _bulletMass;
            bullet.GetComponent<Rigidbody>().AddForce(BulletPoint.transform.forward * BulletForce);

            bullet.Rifle = gameObject;
            bullet.WallHoleLifeTime = _wallHoleLifeTime;
            bullet.BaseDamage = (float) Math.Round(damage, 1);
            Debug.Log(damage + " damage");
            bullet.transform.localScale = new Vector3(damage / 10f, damage / 10f, damage / 10f);
        }

        private IEnumerator Shooting(float damage)
        {
            LastShoot = Time.time;
            PlayAudioClip(ShootAudioClip);
            ShootParticleSystem.Play();

            AddBullet(damage);

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
            if (bulletElement == BulletBehaviour.EnBulletElement.Darkness)
            {
                bulletElement = BulletBehaviour.EnBulletElement.Dendro;
            }
            else
            {
                bulletElement += 1;
            }

            SwitchPadElement();
        }

        private void SwitchPadElement()
        {
            print("Element is " + bulletElement);
            switch (bulletElement)
            {
                case BulletBehaviour.EnBulletElement.Darkness:
                    elementPad.sprite = elementSprites[0];
                    break;
                case BulletBehaviour.EnBulletElement.Dendro:
                    elementPad.sprite = elementSprites[1];
                    break;
                case BulletBehaviour.EnBulletElement.Ice:
                    elementPad.sprite = elementSprites[2];
                    break;
                case BulletBehaviour.EnBulletElement.Light:
                    elementPad.sprite = elementSprites[3];
                    break;
            }
        }

        public void NextRifleFireMode()
        {
            //predictLine.SetActive(fireMode == RifleFireMode.DoubleShot);

            if (fireMode == BulletBehaviour.EnBulletMode.ArtilleryShot)
            {
                fireMode = BulletBehaviour.EnBulletMode.DefaultShot;
            }
            else
            {
                fireMode += 1; 
            }
            SwitchPadMode();
        }
        
        private void SwitchPadMode()
        {
            print("Mode is " + fireMode);
            switch (fireMode)
            {
                case BulletBehaviour.EnBulletMode.DefaultShot:
                    modesPad.sprite = modesSprites[0];
                    break;
                
                case BulletBehaviour.EnBulletMode.ChargedShot:
                    modesPad.sprite = modesSprites[1];
                    break;
                
                case BulletBehaviour.EnBulletMode.ArtilleryShot:
                    modesPad.sprite = modesSprites[2];
                    break;
            }
        }
        
        private void OnBulletHit(Wrapper target)
        {
            OnShootToTarget?.Invoke(target);
        }
    }
}