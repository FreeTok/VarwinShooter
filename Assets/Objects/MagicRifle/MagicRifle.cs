using Varwin;
using Varwin.Public;
using WeaponLibrary;
using System;
using System.Collections;
using TMPro;
using UnityEditor;
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

        public delegate void ShootEvent(Wrapper targetWrapper);

        private Rigidbody _rigidbody;

        [Event(English: "On shoot to target", Russian: "При попадании")]
        public event ShootEvent OnShootToTarget;

        [Event(English: "On shoot", Russian: "При выстреле")]
        public event Action OnShoot;
        
        public Transform bulletExplosionPoint;

        private float _shootDelay = 0.5f;
        private float _lastShoot;

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

        public BulletBehaviour bulletBehaviourPrefab;
        public Transform bulletPoint;

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

        private bool isCharging;

        public Image elementPad;
        public Sprite[] elementSprites;
        
        public Image modesPad;
        public Sprite[] modesSprites;

        private GameObject[] _tpPoints;

        private bool isTpEnabled;
        private Wrapper TpPoint;
        
        public delegate void OnTpHandler(Wrapper target);
        
        [Event(English: "on tp event")]
        public event OnTpHandler OnTp;

        //TODO remove this after making it by raycast
        public void SetTpEnabled(bool enabled, Wrapper tppoint = null)
        {
            isTpEnabled = enabled;
            TpPoint = tppoint;
        }
        
        public AudioClip[] acRifleShots;
        private AudioClip _acRifleShot;

        public AudioClip[] acBulletShots;
        private AudioClip _acBulletShot;

        private float _acShotVolume = 100f;

        [VarwinInspector(English: "Shot audio volume", Russian: "Громкость выстрела")]
        public float ShotVolume
        {
            get => _acShotVolume;
            set => _acShotVolume = value;
        }

        
        private void Awake()
        {
            _chargeDamage = _baseDamage;
            _lastShoot = Time.time;
            
            SwitchPadElement();
            SwitchPadMode();

            _acRifleShot = acRifleShots[0];
            _acBulletShot = acBulletShots[0];
        }

        public void Shoot()
        {
            //TP
            RaycastHit hit;
            if (Physics.Raycast(bulletPoint.position, bulletPoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                Debug.Log("Did Hit");
                
                print(hit.collider.gameObject.GetComponent<TPcplliderBehaviour>());

                if (hit.collider.gameObject.GetComponent<TPcplliderBehaviour>())
                {
                    print("isTping");
                    OnTp?.Invoke(hit.collider.gameObject.GetWrapper());
                    
                    return;
                }
            }


            //TP
            if (isTpEnabled)
            {
                OnTp?.Invoke(TpPoint);
                return;
            }
            
            if (Time.time - _lastShoot >= _shootDelay)
            {
                if (fireMode == BulletBehaviour.EnBulletMode.DefaultShot)
                {
                    StartCoroutine(Shooting(_baseDamage));
                }
            
                if (fireMode == BulletBehaviour.EnBulletMode.ChargedShot)
                {
                    StartCharging();
                }
            
                if (fireMode == BulletBehaviour.EnBulletMode.ArtilleryShot && Time.time - _lastShoot >= _artilleryShootDelay)
                {
                    ArtilleryShot();
                }
            }
        }

        private void StartCharging()
        {
            isCharging = true;
            StartCoroutine(IncreaseDamage());

            GetComponent<AudioSource>().clip = acRifleShots[1];
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().loop = true;
        }

        public void EndCharging()
        {
            if (isCharging)
            {
                StopAllCoroutines();
                StartCoroutine(Shooting(_chargeDamage));
                _chargeDamage = _baseDamage;
                isCharging = false;
                
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().clip = null;
            }
        }

        IEnumerator IncreaseDamage()
        {
            _chargeDamage = _baseDamage;

            while (_chargeDamage < _maxChargeDamage)
            {
                _chargeDamage = (float) Math.Round(_chargeDamage + _сhargeDamageMultiplayer, 1);
                yield return new WaitForSeconds(.2f);
            }
        }

        private void ArtilleryShot()
        {
            bulletBehaviourPrefab.Explode = true;
            StartCoroutine(Shooting(_baseDamage));
            bulletBehaviourPrefab.Explode = false;
        }

        private BulletBehaviour.EnBulletElement _bulletElement;

        private void AddBullet(float damage)
        {
            //TODO Replace it to bulletBehaviour
            var bulletPointTransform = bulletPoint.transform;
            var bullet = Instantiate(bulletBehaviourPrefab, bulletPointTransform.position,
                bulletBehaviourPrefab.gameObject.transform.rotation);

            bullet.bulletMode = fireMode;
            bullet.bulletElement = _bulletElement;
            bullet.OnHitTargetEvent += OnBulletHit;

            bullet.gameObject.SetActive(true);

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.mass = _bulletMass;
            bulletRigidbody.AddForce(bulletExplosionPoint.transform.forward * BulletForce);

            bullet.Rifle = gameObject;
            bullet.WallHoleLifeTime = _wallHoleLifeTime;
            bullet.BaseDamage = (float) Math.Round(damage, 1);
            Debug.Log(damage + " damage");
            bullet.transform.localScale = new Vector3(damage / 10f, damage / 10f, damage / 10f);

            if (_acRifleShot && GetComponent<AudioSource>())
            {
                print(_acRifleShot);
                GetComponent<AudioSource>().PlayOneShot(_acRifleShot);
            }

            if (_acBulletShot)
            {
                print(_acBulletShot);
                bullet.GetComponent<BulletBehaviour>().acBulletShot = _acBulletShot;
                bullet.GetComponent<BulletBehaviour>().acShotVolume = _acShotVolume;
            }
        }

        private IEnumerator Shooting(float damage)
        {
            _lastShoot = Time.time;

            AddBullet(damage);

            OnShoot?.Invoke();

            yield return new WaitForSeconds(ShootDelay);
        }

        public void NextElement()
        {
            if (_bulletElement == BulletBehaviour.EnBulletElement.Darkness)
            {
                _bulletElement = BulletBehaviour.EnBulletElement.Earth;
            }
            else
            {
                _bulletElement += 1;
            }

            SwitchPadElement();
        }

        private void SwitchPadElement()
        {
            print("Element is " + _bulletElement);
            switch (_bulletElement)
            {
                case BulletBehaviour.EnBulletElement.Earth:
                    elementPad.sprite = elementSprites[0];
                    break;
                case BulletBehaviour.EnBulletElement.Air:
                    elementPad.sprite = elementSprites[1];
                    break;
                case BulletBehaviour.EnBulletElement.Fire:
                    elementPad.sprite = elementSprites[2];
                    break;
                case BulletBehaviour.EnBulletElement.Water:
                    elementPad.sprite = elementSprites[3];
                    break;
                case BulletBehaviour.EnBulletElement.Ice:
                    elementPad.sprite = elementSprites[4];
                    break;
                case BulletBehaviour.EnBulletElement.Light:
                    elementPad.sprite = elementSprites[5];
                    break;
                case BulletBehaviour.EnBulletElement.Darkness:
                    elementPad.sprite = elementSprites[6];
                    break;
            }
        }

        public void NextRifleFireMode()
        {
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

                    _acRifleShot = acRifleShots[0];
                    _acBulletShot = acBulletShots[0];
                    break;
                
                case BulletBehaviour.EnBulletMode.ChargedShot:
                    modesPad.sprite = modesSprites[1];

                    _acRifleShot = null;
                    _acBulletShot = acBulletShots[1];
                    break;
                
                case BulletBehaviour.EnBulletMode.ArtilleryShot:
                    modesPad.sprite = modesSprites[2];

                    _acRifleShot = acRifleShots[2];
                    _acBulletShot = acBulletShots[2];
                    break;
            }
        }
        
        private void OnBulletHit(Wrapper target)
        {
            OnShootToTarget?.Invoke(target);
        }

    }
}