using UnityEngine;
using Varwin;
using Varwin.Public;
using Random = UnityEngine.Random;

namespace WeaponLibrary
{
    public class HandleMovementsBehaviour : MonoBehaviour, IGrabStartAware, IGrabEndAware
    {
        private WeaponBehaviour _weaponBehaviour;

        private Transform _weaponTransform;
        private Quaternion _deltaRotation;
        private Vector3 _deltaPosition;
        private Vector3 _startLocalPosition;
        private Quaternion _startLocalRotation;

        public bool IsGrabbed { get; set; }

        private float _inertiaVelocity = 0f;
        private float _inertiaAngle = 0f;

        public HandleMovementsBehaviour SupportHandle;

        public bool ApplyTransform = true;

        public float OffsetX = 33f;

        public delegate void OnGrabHandler();

        public event OnGrabHandler Grabbed;
        public event OnGrabHandler Ungrabbed;

        private void Awake()
        {
            if (SupportHandle)
            {
                var deltaVector = (SupportHandle.transform.position - transform.position).normalized;
                OffsetX = Vector3.Angle(deltaVector, transform.forward) *
                          Mathf.Sign(Vector3.Dot(deltaVector, transform.up));

                Debug.Log(OffsetX);
            }

            _weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
            _weaponTransform = _weaponBehaviour.transform;
            _weaponBehaviour.OnShoot += OnShoot;

            _deltaPosition = transform.InverseTransformPoint(_weaponTransform.position);
            _deltaRotation = Quaternion.Inverse(transform.rotation) * _weaponTransform.rotation;

            _startLocalPosition = transform.localPosition;
            _startLocalRotation = transform.localRotation;
        }

        private void OnShoot()
        {
            var coef = IsGrabbed && SupportHandle && SupportHandle.IsGrabbed ? _weaponBehaviour.CoefSlowInertiaByTwoHand : 1f;

            _inertiaVelocity =
                Random.Range(_weaponBehaviour.MaxVelocityInertia / 4f, _weaponBehaviour.MaxVelocityInertia) * coef;

            _inertiaAngle = Random.Range(_weaponBehaviour.MaxAngleInertia / 4f, _weaponBehaviour.MaxAngleInertia) *
                            coef;
        }

        public void OnGrabStart(GrabingContext context)
        {
            if (SupportHandle)
            {
                SupportHandle.ApplyTransform = false;
            }

            IsGrabbed = true;
            Grabbed?.Invoke();
        }

        public void OnGrabEnd()
        {
            if (SupportHandle)
            {
                SupportHandle.ApplyTransform = true;
            }

            IsGrabbed = false;
            Ungrabbed?.Invoke();
            transform.localPosition = _startLocalPosition;
            transform.localRotation = _startLocalRotation;
        }

        private void FixedUpdate()
        {
            if (!IsGrabbed)
            {
                return;
            }

            Quaternion supportRotation = Quaternion.identity;

            if (SupportHandle && SupportHandle.IsGrabbed)
            {
                var delta = (SupportHandle.transform.position - transform.position).normalized;
                supportRotation = Quaternion.LookRotation(delta, transform.up);
                supportRotation *= Quaternion.Euler(OffsetX, 0, 0);
            }

            if (ApplyTransform)
            {
                var delta = SupportHandle && SupportHandle.IsGrabbed
                    ? supportRotation
                    : transform.rotation;

                _weaponTransform.position = transform.position + delta * _deltaPosition;

                _weaponTransform.rotation = (SupportHandle && SupportHandle.IsGrabbed
                    ? supportRotation
                    : transform.rotation) * _deltaRotation * Quaternion.Euler(_inertiaAngle, 0, 0);

                _weaponTransform.Translate(Vector3.back * _inertiaVelocity);
            }

            _inertiaVelocity = Mathf.Lerp(_inertiaVelocity, 0, Time.deltaTime * 4f);
            _inertiaAngle = Mathf.Lerp(_inertiaAngle, 0, Time.deltaTime * 4f);
        }
    }
}