using System;
using System.Collections;
using UnityEngine;
using Varwin;
using Varwin.Public;

namespace WeaponLibrary
{
    public class WeaponBehaviour : VarwinObject
    {
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

        [VarwinInspector(English: "Simple shooting mechanics",
            Russian: "Упрощенная механика стрельбы")]
        public bool SimpleMechanics
        {
            get => _simpleMechanics;
            set => _simpleMechanics = value;
        }

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

        public Animator MainAnimator;
        public JointPoint MagazineJointPoint;

        public MagazineBehaviour MagazineBehaviour => GetMagazine();

        public Rigidbody Rigidbody => GetRigidBody();

        private bool _isBusy = false;

        public bool IsBusy => !MainAnimator.GetCurrentAnimatorStateInfo(0).IsName(IdleClip.name) || _isBusy;

        public Transform ShootPoint;
        public HoleBehaviour HolePrefab;

        public bool HasAmmo = false;
        public bool OnSafety = false;

        public float ShootNormalizedTime = 0.5f;

        public AudioClip ShootAudioClip;
        public AudioClip ReloadAudioClip;
        public AudioClip ReturnFrameAudioClip;

        public AudioClip ConnectMagazineClip;
        public AudioClip DisconnectMagazineClip;

        public ParticleSystem ShootParticleSystem;
        public LightExplositionAnimator LightExplositionAnimator;

        private AudioSource _audioSource;
        private float _forceInertia = 3000;

        public float ForceRadius = 0.3f;
        private float _shootDelay = 0.5f;

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

        private float _bulletForce = 0.5f;

        private Vector3 _magazineDeltaPos;

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

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            StartCoroutine(InitMagazine());
        }

        private IEnumerator InitMagazine()
        {
            while (MagazineJointPoint.JointBehaviour == null)
            {
                yield return new WaitForEndOfFrame();
            }

            MagazineJointPoint.JointBehaviour.OnConnect += JointBehaviourOnConnect;
            MagazineJointPoint.JointBehaviour.OnDisconnect += JointBehaviourOnDisconnect;
        }

        private void JointBehaviourOnDisconnect()
        {
            PlayAudioClip(DisconnectMagazineClip);
        }

        private void JointBehaviourOnConnect()
        {
            PlayAudioClip(ConnectMagazineClip);

            _magazineDeltaPos = transform.InverseTransformPoint(MagazineBehaviour.transform.position);
        }

        public void Shoot()
        {
            if (IsBusy)
            {
                return;
            }
            
            if (!HasAmmo)
            {
                if (!_simpleMechanics)
                {
                    return;
                }
               
                if (MagazineBehaviour == null)
                {
                    return;
                }
            }

            if (OnSafety && !_simpleMechanics)
            {
                return;
            }

            StartCoroutine(Shooting());
        }

        public void Reload()
        {
            if (OnSafety)
            {
                return;
            }

            MainAnimator.SetTrigger("Reload");

            PlayAudioClip(IsBusy ? ReturnFrameAudioClip : ReloadAudioClip);

            if (HasAmmo)
            {
                AddBullet(false);
            }

            HasAmmo = MagazineBehaviour && MagazineBehaviour.GetAmmo();
        }

        private void AddBullet(bool used)
        {
            var bulletPointTransform = BulletPoint.transform;
            var bullet = Instantiate(BulletBehaviourPrefab, bulletPointTransform.position,
                bulletPointTransform.rotation);

            bullet.gameObject.SetActive(true);
            bullet.SetObject(used);

            bullet.Rigidbody.AddForce(BulletPoint.transform.forward * BulletForce);
        }

        private IEnumerator Shooting()
        {
            _isBusy = true;
            var animName = !MagazineBehaviour || MagazineBehaviour.IsEmpty() ? "ShootLastAmmo" : "Shoot";
            MainAnimator.SetTrigger(animName);

            PlayAudioClip(ShootAudioClip);
            ShootParticleSystem.Play();
            LightExplositionAnimator.Play();

            Raycast();

            while (MainAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < ShootNormalizedTime)
            {
                yield return new WaitForEndOfFrame();
            }

            Rigidbody.AddExplosionForce(ForceInertia, ShootPoint.transform.position, ForceRadius);
            OnShoot?.Invoke();

            AddBullet(true);

            HasAmmo = MagazineBehaviour && MagazineBehaviour.GetAmmo();

            yield return new WaitForSeconds(ShootDelay);
            _isBusy = false;
        }

        private void LateUpdate()
        {
            if (!MagazineBehaviour)
            {
                return;
            }
            
            MagazineBehaviour.FixPosition(transform.TransformPoint(_magazineDeltaPos));
        }

        public void PlayAudioClip(AudioClip clip)
        {
            if (_audioSource && clip)
            {
                _audioSource.PlayOneShot(clip);
            }
        }

        private void Raycast()
        {
            var ray = new Ray(ShootPoint.position, ShootPoint.forward);
            if (!Physics.Raycast(ray, out var hit))
            {
                return;
            }

            if (hit.collider.isTrigger || hit.collider.name != "Player")
            {
                var holeInstance = Instantiate(HolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                holeInstance.MaxTime = WallHoleLifeTime;
                var holeSpriteTransform = holeInstance.transform;
                
                holeSpriteTransform.Translate(Vector3.forward * 0.01f);
                holeSpriteTransform.parent = hit.collider.transform;

                holeInstance.gameObject.SetActive(true);
            }

            var target = hit.collider.GetComponentInParent<TargetBehaviour>();
            var wrapper = hit.collider.gameObject.GetWrapper();

            if (!target || wrapper == null)
            {
                return;
            }

            target.Hit(hit.point);
            Debug.Log("Hit the target with point: " + target.CountMarks);

            OnShootToTarget?.Invoke(target ? target.CountMarks : 0, wrapper);
        }

        private MagazineBehaviour GetMagazine()
        {
            if (!MagazineJointPoint || !MagazineJointPoint.ConnectedJointPoint)
            {
                return null;
            }

            return MagazineJointPoint.ConnectedJointPoint.JointBehaviour.GetComponent<MagazineBehaviour>();
        }

        private Rigidbody GetRigidBody()
        {
            if (!_rigidbody)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            return _rigidbody;
        }

        public void SetEnableSafety(bool safety)
        {
            OnSafety = safety;
        }

        public void ForceSetKinematic(bool state)
        {
            
        }
    }
}